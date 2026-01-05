//https://hexed.it/
//https://github.com/ggerganov/ggml/blob/master/docs/gguf.md

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public partial class OzGGUFFile
    {
        // Metadata Entries
        public OzGGUF_MDCount MDCount;
        public List<OzGGUF_MD> MDList;
        public Dictionary<byte[], OzGGUF_MD> MDs;


        bool LoadHeaders(out string error)
        {

            if (!File.Exists(FileName))
            {
                error = "GGUF file does not exist: " + FileName;
                return false;
            }

            try
            {
                using (var s = new FileStream(FileName, FileMode.Open))
                {
                    var res = LoadHeadersFromStream(s, out error);
                    return res;
                }
            }
            catch (Exception e)
            {
                error = "Error while parsing GGUF header. " + e.Message;
                return false;
            }

        }


        public bool LoadHeadersFromStream(FileStream s, out string error)
        {

            //Parse Header
            if (!ReadTo(s, out MagicNumber, out error)) return false;
            if (!ReadTo(s, out VersionNumber, out error)) return false;
            if (!ReadTo(s, out TensorCount, out error)) return false;
            if (!ReadTo(s, out MDCount, out error)) return false;

            //Parse Metadata
            if (!parseMDs(s, out error)) return false;

            //Parse Tensors
            if (!parseTensors(s, out error)) return false;

            _headerEnd = s.Position;

            //Optional
            parseArch(s, out error);

            return true;
        }

        //**********************************************
        // Parse MetaData
        //**********************************************

        bool parseMDs(Stream s, out string errorMessage)
        {
            MDs = new Dictionary<byte[], OzGGUF_MD>(new OzMDStrComparer());
            MDList = new List<OzGGUF_MD>();

            int count = (int)MDCount.Value;

            for (int i = 0; i < count; i++)
            {
                var md = new OzGGUF_MD();
                if (!md.TryParse(s, out errorMessage)) return false;

                MDs.Add(md.MDName.Bytes, md);
                MDList.Add(md);
            }

            errorMessage = null;
            return true;
        }

        //**********************************************
        // ToString
        //**********************************************

        public string MetaDescriptions()
        {
            var s = new StringBuilder();
            foreach (var md in MDList)
            {
                var str = md.ToString();
                str = str.Replace("\n", string.Empty).Replace("\r", string.Empty);
                string strToPrint;
                if (md.MDName.Value == "tokenizer.chat_template")
                    strToPrint = str;
                else 
                    strToPrint = GetFrontWithDots(str, 1000);
                s.AppendLine(strToPrint);
            }
            return s.ToString();
        }

        static string GetFrontWithDots(string source, int len)
        {
            if (string.IsNullOrEmpty(source)) return "";

            if (source.Length <= len) return source;
            else return source.Substring(0, len) + "...";
        }
    }
}
