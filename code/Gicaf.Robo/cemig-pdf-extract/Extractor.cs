using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace cemig_pdf_extract
{
    class Extractor
    {
        public string PDFdir;
        public string PDFparsed;
        public string PDFaudit;
        public string CSVoutput;

        public string sinonimoBalanco;
        public string sinonimoDemonstracao;

        private bool DebugMode;

        public CRCInput crcInput;
        public Parser parser;

        public string executionIdent;

        public Encoding CSVENCODING = new UTF8Encoding(true, true);

        public Extractor()
        {
            executionIdent = uniqueId();

            PDFdir = ConfigurationManager.AppSettings["PDFdir"];
            PDFparsed = ConfigurationManager.AppSettings["PDFparsed"];
            PDFaudit = ConfigurationManager.AppSettings["PDFaudit"];
            CSVoutput = ConfigurationManager.AppSettings["CSVoutput"];

            sinonimoBalanco = ConfigurationManager.AppSettings["sinonimoBalanco"];
            sinonimoDemonstracao = ConfigurationManager.AppSettings["sinonimoDemonstracao"];

            if (PDFparsed[PDFparsed.Length - 1] != Path.DirectorySeparatorChar)
                PDFparsed += Path.DirectorySeparatorChar;

            if (PDFaudit[PDFaudit.Length - 1] != Path.DirectorySeparatorChar)
                PDFaudit += Path.DirectorySeparatorChar;

            if (CSVoutput[CSVoutput.Length - 1] != Path.DirectorySeparatorChar)
                CSVoutput += Path.DirectorySeparatorChar;

            if (!Directory.Exists(PDFparsed))
                Directory.CreateDirectory(PDFparsed);

            if (!Directory.Exists(PDFaudit))
                Directory.CreateDirectory(PDFaudit);

            if (!Directory.Exists(CSVoutput))
                Directory.CreateDirectory(CSVoutput);

            DebugMode = ConfigurationManager.AppSettings["DebugMode"].Equals("1");

            crcInput = new CRCInput();
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

        public string uniqueId()
        {
            return Guid.NewGuid().ToString("N");
        }

        public void fileToAudit(string file)
        {
            File.Copy(file, PDFaudit + Path.GetFileName(file));
        }

        public void fileToParsed(string file, string classification, string cnpj, string data)
        {
            // 12876987000112 - D - 31_12_2019 - 1.pdf
            cnpj = Regex.Replace(cnpj, @"[^\d]", "");
            string baseName = PDFparsed + String.Format("{0} - {1} - {2}", cnpj, classification, data.Replace("/", "_"));// DateTime.Now.ToString("dd_MM_yyyy"));
            int baseNumber = 1;
            while (File.Exists(String.Format("{0} - {1}.pdf", baseName, baseNumber)))
                baseNumber++;

            File.Copy(file, String.Format("{0} - {1}.pdf", baseName, baseNumber));
        }

        public void Error(Exception e)
        {
            if (this.DebugMode)
            {
                Log("[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] ERROR: " + e.Message);
            }
        }

        public void Log(string data)
        {
            Console.WriteLine(data);
        }

        public void Run()
        {
            foreach (FileInfo file in (new DirectoryInfo(PDFdir)).GetFiles("*.pdf"))
            {
                CRCInput.CRCData crcData = crcInput.GetCRCData(file.FullName);
                if (crcInput.isNewInput(crcData))
                {
                    try
                    {
                        Log("Parsing " + crcData.crc + ": " + crcData.fileName);
                        if (this.DebugMode)
                        {
                            parser.PdfToText(file.FullName, Path.ChangeExtension(file.FullName, ".txt"));
                        }
                        List<Parser.ModelOutput> data = parser.Parse(file.FullName);
                        if ((data != null) && (data.Count > 0))
                        {
                            string fileIdentification = "";
                            string cnpj = "";
                            string firstdate = "";
                            foreach (Parser.ModelOutput modelOutput in data)
                            {
                                if (fileIdentification.IndexOf(modelOutput.model.prefix) == -1)
                                    fileIdentification = fileIdentification + modelOutput.model.prefix;
                                cnpj = modelOutput.cnpj;
                                if (firstdate == "")
                                    firstdate = modelOutput.data;
                                WriteCSVLines(modelOutput);
                            }
                            fileToParsed(file.FullName, fileIdentification, cnpj, firstdate);
                        }
                        else
                        {
                            fileToAudit(file.FullName);
                        }

                        crcInput.AddInput(crcData);
                    }
                    catch (Exception e)
                    {
                        Error(e);
                    }
                }
            }
        }

        public void WriteCSVLines(Parser.ModelOutput modelOutput)
        {
            string csv = CSVoutput + modelOutput.model.prefix + "_" + executionIdent + ".csv";
            if (!File.Exists(csv))
            {
                List<string> header = new List<string>();
                header.Add("Data de Referência");
                header.Add("CNPJ");
                header.Add("Entidade");
                foreach (List<string> syns in modelOutput.model.outputs)
                    header.Add(syns[0]);

                File.AppendAllText(csv, String.Join(";", header) + "\n", CSVENCODING);
            }
            
            File.AppendAllText(csv, String.Join(";", modelOutput.output) + "\n", CSVENCODING);
        }

        static void Main(string[] args)
        {
            var extractor = new Extractor();
            extractor.Run();
        }
    }
}
