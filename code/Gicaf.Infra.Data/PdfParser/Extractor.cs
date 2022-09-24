//using Microsoft.AspNetCore.Hosting;
using Gicaf.Domain.Entities.Fornecedores;
using Gicaf.Domain.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace Gicaf.Infra.Data.PdfParser
{
    public class PdfInfoExtractor: IDocumentInfoExtractor
    {
        string sinonimoBalanco;
        string sinonimoDemonstracao;
        Parser parser;
        public Encoding CSVENCODING = new UTF8Encoding(true, true);

        public PdfInfoExtractor(string rootPath)
        {
            sinonimoBalanco = Path.Combine(rootPath, "SinonimosBalanco.csv");
            sinonimoDemonstracao = Path.Combine(rootPath, "SinonimosDemonstracao.csv");

            parser = new Parser();

            parser.models.Add(new Parser.Model(
                "B", 
                "BALANÇO PATRIMONIAL\n",
                "Descrição Saldo Final Saldo Inicial",
                "Este relatório foi gerado pelo Sistema Público de Escrituração Digital – Sped",
                sinonimoBalanco
                ));

            parser.models.Add(new Parser.Model(
                "D", 
                "DEMONSTRAÇÃO DE RESULTADO DO EXERCÍCIO\n",
                "Descrição Valor Valor da última DRE",
                "Este relatório foi gerado pelo Sistema Público de Escrituração Digital – Sped",
                sinonimoDemonstracao));
        }

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') | c == '.' || c == '_')
                {
                    sb.Append(c);
                } else
                {
                    sb.Append(" ");
                }
            }
            return sb.ToString();
        }

        static string ToCamelCase(string str)
        {
            if (str == null) return str;
            if (str.Length < 2) return str.ToUpper();
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }

        static string ToJsonCamelCase(string name)
        {
            string[] words = RemoveSpecialCharacters(RemoveDiacritics(name))
                .ToLower().Replace("  ", " ").Replace(" ", "_").Split('_');
            string newName = "";
            foreach (string word in words)
            {
                newName += ToCamelCase(word);
            }
            newName = newName.Substring(0, 1).ToLower() + newName.Substring(1);
            return newName;
        }

        private JObject WriteJSON(List<Parser.ModelOutput> modelOutputs)
        {
            var arOutputs = new JArray();
            if (modelOutputs != null)
            {
                Dictionary<string, int> labels = new Dictionary<string, int>();

                foreach (Parser.ModelOutput modelOutput in modelOutputs)
                {
                    if (!labels.ContainsKey("dataReferencia"))
                    {
                        labels.Add("dataReferencia", 0);
                        labels.Add("cnpj", 1);
                        labels.Add("entidade", 2);
                        labels.Add("tipo", 3);
                        var at = 4;
                        foreach (Parser.ModelRules syns in modelOutput.model.rules)
                            labels.Add(ToJsonCamelCase(syns.syns[0]), at++);
                    }

                    var item = new JObject();
                    foreach (string label in labels.Keys)
                    {
                        var at = labels[label];
                        item.Add(label, modelOutput.output[at]);
                    }

                    arOutputs.Add(item);
                }
            }

            var json = new JObject();
            json.Add("items", arOutputs);

            return json;
        }

        internal JObject Extract(string pdfBase64)
        {
            byte[] pdf = Convert.FromBase64String(pdfBase64);
            return WriteJSON(parser.ParseFromBytes(pdf));
        }

        internal JObject Extract(byte[] pdf)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44308/")
            };
            /*
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.PostAsJsonAsync("api/v1/crawly/sped", new { pdf = Convert.ToBase64String(pdf) }).Result;
            response.EnsureSuccessStatusCode();
            var result = response.Content.ReadAsStringAsync().Result;
            return JObject.Parse(result);
            */
            return WriteJSON(parser.ParseFromBytes(pdf));
        }

        public DadosBalancoPatrimonial RetornarDadosBalancoPatrimonial(byte[] conteudo)
        {
            var dadosBalanco = ExtractToObject<DadosBalancoPatrimonial>(conteudo, out string camposLidosPeloRobo);
            dadosBalanco.DadosCalculadosPeloRobo = camposLidosPeloRobo;
            return dadosBalanco;
        }

        public DadosDRE RetornarDadosDRE(byte[] conteudo)
        {
            var dadosDre = ExtractToObject<DadosDRE>(conteudo, out string camposLidosPeloRobo);
            dadosDre.DadosCalculadosPeloRobo = camposLidosPeloRobo;
            return dadosDre;
        }

        private T ExtractToObject<T>(byte[] conteudo, out string returnedFields) where T: class, new()
        {
            var jObject = Extract(conteudo);
            List<string> fieldsList = new List<string>();
            var itemList = (JArray)jObject.GetValue("items");
            
            if(itemList.Count > 0)
            {
                var item = itemList[0];
                if(item != null)
                {
                    foreach (JProperty jProperty in item.ToCollection())
                    {
                        if((jProperty.Value as JValue)?.Value != null)
                        {
                            var objProperty = typeof(T).GetProperty(jProperty.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                            if (objProperty != null)
                            {
                                fieldsList.Add(objProperty.Name);
                            }
                        }
                    }
                    returnedFields = string.Join(",", fieldsList);
                    return item.ToObject<T>();
                }
            }
            returnedFields = null;
            return new T();            
        }
    }
}