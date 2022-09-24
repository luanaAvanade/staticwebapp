using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Gicaf.Infra.Data.PdfParser
{
    class Parser
    {
        public class ModelRules
        {
            public List<string> syns = new List<string>();
            public List<string> excludeds = new List<string>();
            public List<string> parents = new List<string>();
        }

        public class Model
        {
            public string prefix;
            public string identification;
            public string dataStartAt;
            public string dataEndAt;
            public List<ModelRules> rules;

            public Model(string prefix, string identification, string dataStartAt, string dataEndAt, string inputsFileName)
            {
                this.prefix = prefix;
                this.identification = identification;
                this.dataStartAt = dataStartAt;
                this.dataEndAt = dataEndAt;

                this.rules = new List<ModelRules>();

                var inputs = File.ReadAllText(inputsFileName);
                foreach (string lines in inputs.Split('\n'))
                {
                    List<string> syns = new List<string>();
                    foreach (string syn in lines.Trim().Split(';'))
                        if (!syn.Trim().Equals(""))
                            syns.Add(syn);

                    if (syns.Count > 0)
                    {
                        ModelRules modelSyns = new ModelRules();

                        int excludedAt = ArrayIndexOf(syns, new List<string> { "^", "\"^\"" });
                        int parentsAt = ArrayIndexOf(syns, new List<string> { "@", "\"@\"" });

                        int endSyns = (excludedAt > -1) ? excludedAt : syns.Count;
                        for (int i = 0; i < endSyns; i++)
                            if (syns[i].Trim().Length > 0)
                                modelSyns.syns.Add(syns[i]);

                        if (excludedAt > -1)
                        {
                            int endExcluded = (parentsAt > -1) ? parentsAt : syns.Count;
                            for (int i = excludedAt + 1; i < endExcluded; i++)
                                if (syns[i].Trim().Length > 0)
                                    modelSyns.excludeds.Add(syns[i]);
                        }

                        if (parentsAt > -1)
                        {
                            int endParents = syns.Count;
                            for (int i = parentsAt + 1; i < endParents; i++)
                                if (syns[i].Trim().Length > 0)
                                    modelSyns.parents.Add(syns[i]);
                        }

                        this.rules.Add(modelSyns);
                    }
                }
            }

            public int ArrayIndexOf(List<string> strArray, List<string> indexs)
            {
                foreach (string syn in indexs)
                {
                    int at = strArray.IndexOf(syn);
                    if (at > -1)
                        return at;
                }
                return -1;
            }
        }

        public class ModelData {
            public string headerIdent = "";
            public List<string> header = new List<string>();
            public List<string> lines = new List<string>();
        }

        public class ModelOutput
        {
            public Model model;
            public string cnpj;
            public string data;
            public List<string> output;
        }

        public List<Model> models;

        public Parser()
        {
            models = new List<Model>();
        }

        public bool IsMatchLine(List<string> lines, int lineIdx, ModelRules synsRules)
        {
            int synIdx = -1;

            bool synFound = false;
            for (int i = 0; i < synsRules.syns.Count; i++)
            {
                string syn = synsRules.syns[i];
                if (lines[lineIdx].LastIndexOf(syn, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    synIdx = i;
                    synFound = true;
                    break;
                }
            }

            if (synFound)
            {
                foreach (string exc in synsRules.excludeds)
                {
                    if (lines[lineIdx].LastIndexOf(exc, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        synFound = false;
                        break;
                    }
                }
            }

            if (synFound && (synsRules.parents.Count > 0) && (lineIdx > 0))
            {
                bool parentCheck = true;
                foreach (string prt in synsRules.parents)
                    if (synsRules.syns[synIdx].IndexOf(prt, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        parentCheck = false;
                        break;
                    }

                if (parentCheck)
                {
                    synFound = false;
                    foreach (string prt in synsRules.parents)
                    {
                        if (lines[lineIdx - 1].LastIndexOf(prt, StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            synFound = true;
                            break;
                        }
                    }
                }                
            }

            return synFound;
        }

        public ModelOutput ParseWithModel(List<string> header, List<string> lines, Model model)
        {
            Boolean hasOne = false;
            string cnpj = "";
            string dataExe = "";
            List<string> output = new List<string>();
            foreach (ModelRules rules in model.rules)
                output.Add(null);

            int at = 0;
            foreach (ModelRules rules in model.rules)
            {
                Boolean match = false;
                for (var i = 0; i < lines.Count; i++)
                {
                    if (IsMatchLine(lines, i, rules))                    
                    {
                        string[] data = lines[i].Split(new string[] { "R../../", "null" }, StringSplitOptions.None);
                        try
                        {
                            string value = data[1].Trim();
                            if ((value.IndexOf("(") == 0) && (value.IndexOf(")") == value.Length - 1))
                            {
                                value = (Convert.ToDecimal(value.Substring(1, value.Length - 2), CultureInfo.GetCultureInfo("pt-br")) * -1).ToString();
                            }
                            else
                            {
                                value = Convert.ToDecimal(value, CultureInfo.GetCultureInfo("pt-br")).ToString();
                            }
                            output[at] = value;
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                        hasOne = true;
                        match = true;
                        break;
                    }

                    if (match)
                        break;
                }
                at++;
            }
            
            if (hasOne)
            {
                int startSeekAt = 0;
                while (true)
                {
                    int inicioEntidade = header.IndexOf("Período Selecionado:", startSeekAt);
                    if (inicioEntidade > -1)
                    {
                        int fimEntidade = inicioEntidade;
                        for (int i = inicioEntidade; i < header.Count; i++)
                            if (Regex.Replace(header[i], @"[^\d\/\s\.\-a]", "").Equals(header[i]))
                            {
                                fimEntidade = i;
                                break;
                            }
                        string entidade = "";
                        for (int i = inicioEntidade + 1; i < fimEntidade; i++)
                            entidade += header[i];

                        dataExe = header[fimEntidade + 1].Substring(header[fimEntidade + 1].IndexOf(" a ") + 3);
                        cnpj = header[fimEntidade].Substring(24);
                        //output.Insert(0, DateTime.Parse(dataExe, CultureInfo.GetCultureInfo("pt-br")).ToString("dd-MM-yyyy"));
                        output.Insert(0, DateTime.Parse(dataExe, CultureInfo.GetCultureInfo("pt-br")).ToString("yyyy-MM-dd"));
                        output.Insert(1, cnpj);
                        output.Insert(2, entidade);          
                        output.Insert(3, model.prefix);

                        startSeekAt = fimEntidade;
                    }
                    else
                        break;
                }
            }

            return (hasOne) 
                ? new ModelOutput() { model = model, cnpj = cnpj, data = dataExe, output = output }
                : null;
        }

        public List<ModelOutput> ParseFromBytes(byte[] pdf)
        {
            var reader = new PdfReader(pdf);

            List<Model> validModels = new List<Model>();
            List<string> data = new List<string>();
            for (var i = 1; i <= reader.NumberOfPages; i++)
            {
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                var lines = PdfTextExtractor.GetTextFromPage(reader, i, strategy).Replace("\r", "");
                foreach (Model model in models)
                    if (lines.Trim().IndexOf(model.identification) > -1)
                        if (validModels.IndexOf(model) == -1)
                            validModels.Add(model);

                if (validModels.Count > 0)
                    foreach (string s in lines.Split('\n'))
                        data.Add(s.Trim());
            }

            if (data.Count > 0)
            {
                List<ModelOutput> outputs = new List<ModelOutput>();
                foreach(Model validModel in validModels)
                {
                    Boolean hasHeader = false;
                    Boolean headerStart = false;
                    List<string> headerData = new List<string>();
                    List<string> validData = new List<string>();
                    Boolean startValid = false;

                    int startAt = 0;
                    Boolean hasStart = false;
                    for (var i = startAt; i < data.Count; i++)
                        if (data[i].IndexOf(validModel.identification.Trim()) > -1)
                        {
                            startAt = i;
                            hasStart = true;
                            break;
                        }

                    if (hasStart)
                    {
                        List<ModelData> modelDatas = new List<ModelData>();

                        ModelData currData = new ModelData();
                        for (var i = startAt; i < data.Count; i++)
                        {
                            string d = data[i];

                            if (d.IndexOf(validModel.dataStartAt) > -1)
                            {
                                hasHeader = true;
                                headerStart = true;
                                startValid = true;
                            }
                            else if (startValid)
                            {
                                if (d.Replace("\u0096", "–").IndexOf(validModel.dataEndAt) > -1)
                                {
                                    if (currData.header.Count > 0)
                                    {
                                        if (currData.header[0].IndexOf(validModel.identification.Trim()) > -1)
                                        {
                                            currData.headerIdent = String.Join("\r", currData.header.ToArray());
                                            modelDatas.Add(currData);
                                        }
                                    }
                                    currData = new ModelData();
                                    startValid = false;
                                    hasHeader = false;
                                    headerStart = false;
                                    i += 1;
                                }
                                else
                                {
                                    currData.lines.Add(d);
                                }
                            }
                            else if (!hasHeader)
                            {
                                if (d.IndexOf(validModel.identification.Trim()) > -1)
                                    headerStart = true;

                                if (headerStart)
                                    currData.header.Add(d);
                            }
                        }

                        List<ModelData> modelGroupedDatas = new List<ModelData>();
                        foreach (ModelData modelData in modelDatas)
                        {
                            ModelData theModelData = null;
                            foreach (ModelData gmodelData in modelGroupedDatas)
                                if (gmodelData.headerIdent.Equals(modelData.headerIdent))
                                {
                                    theModelData = gmodelData;
                                    break;
                                }

                            if (theModelData == null)
                            {
                                modelGroupedDatas.Add(modelData);
                            }
                            else
                            {
                                theModelData.lines.AddRange(modelData.lines);
                            }
                        }

                        foreach (ModelData gmodelData in modelGroupedDatas)
                        {
                            ModelOutput output = ParseWithModel(gmodelData.header, gmodelData.lines, validModel);
                            if (output != null)
                                outputs.Add(output);
                        }
                    }                    
                }

                return (outputs.Count > 0)? outputs : null;
            }

            return null;
        }
    }
}
