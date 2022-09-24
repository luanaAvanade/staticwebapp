using ICSharpCode.SharpZipLib.Checksum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace cemig_pdf_extract
{
    class CRCInput
    {
        [Serializable]
        public class CRCData {
            public string crc;
            public string fileName;
        }

        List<CRCData> inputs;

        public CRCInput()
        {
            Load();
        }

        public string CRCDataFile()
        {
            return Path.ChangeExtension(Environment.GetCommandLineArgs()[0], ".crc");
        }

        public void Load()
        {
            var dataFn = CRCDataFile();
            if (File.Exists(dataFn))
            {
                using (var file = File.OpenRead(dataFn))
                {
                    var reader = new BinaryFormatter();
                    inputs = (List<CRCData>)reader.Deserialize(file);
                }
            } else
            {
                inputs = new List<CRCData>();
            }
        }

        public void Save()
        {
            using (var file = File.OpenWrite(CRCDataFile()))
            {
                var writer = new BinaryFormatter();
                writer.Serialize(file, inputs); 
            }
        }

        public string CRCFromFile(string fileName)
        {
            Crc32 crc = new Crc32();
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var len = fs.Length;
                var step = Math.Max(0, fs.Length / 1024);

                while (fs.Position < len)
                {
                    var buffer = fs.ReadByte();
                    crc.Update(buffer);
                    fs.Seek(step, SeekOrigin.Current);
                }

            }

            return crc.Value.ToString("x");
        }

        public int CRCIndexOf(string crcValue, string fileName)
        {
            for (int i = 0; i < inputs.Count; i++)
                if (inputs[i].crc.Equals(crcValue) && 
                    inputs[i].fileName.Equals(fileName))
                    return i;
            return - 1;
        }

        public CRCData GetCRCData(string file)
        {
            var crc = CRCFromFile(file);
            var fileName = Path.GetFileName(file).ToLower();
            return new CRCData()
            {
                crc = crc,
                fileName = fileName
            };
        }

        public Boolean isNewInput(CRCData input)
        {
            var indexOf = CRCIndexOf(input.crc, input.fileName);
            if (indexOf == -1)
            {
                return true;
            }

            return false;
        }

        public void AddInput(CRCData input)
        {
            inputs.Add(new CRCData()
            {
                crc = input.crc,
                fileName = input.fileName
            });
            Save();
        }
    }
}
