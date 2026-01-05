//https://hexed.it/
//https://github.com/ggerganov/ggml/blob/master/docs/gguf.md
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public partial class OzGGUFFile
    {
        public static Stopwatch MyStopwatch = new Stopwatch();
        public string FileName;
        public string Architecture;
        long _tensDataOffset;

        public OzGGUF_MagicNumber MagicNumber;
        public OzGGUF_VersionNumber VersionNumber;

        public static bool Create(string file, out OzGGUFFile result, out string error)
        {
            result = new OzGGUFFile();
            result.FileName = file;
            var res = result.LoadHeaders(out error);
            return res;
        }

        public OzGGUFFile()
        {
        }

        public OzGGUFFile(string file)
        {
            FileName = file;
        }

        public bool LoadHeadersAndTensors(out string error)
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
                    if (!LoadHeadersFromStream(s, out error)) return false;
                    if (!LoadTensorFromStream(s, out error)) return false;
                    return true;
                }
            }
            catch (Exception e)
            {
                error = "Error while parsing GGUF file. " + e.Message;
                return false;
            }
        }

        //**********************************************
        // Parse Item
        //**********************************************
        bool ReadTo<T>(Stream s, out T i, out string error) where T : OzGGUF_Item
        {
            i = Activator.CreateInstance<T>();
            return i.TryParse(s, out error);
        }

        //**********************************************
        // Parse Arch
        //**********************************************
        bool parseArch(Stream s, out string errorMessage)
        {
            var bytes = Encoding.UTF8.GetBytes("general.architecture");
            if (!MDs.ContainsKey(bytes))
            {
                errorMessage = "Architecture not found in meta descripton";
                Architecture = "";
                return false;
            }

            var md = MDs[bytes].MDValue as OzGGUF_String;
            Architecture = md.Value;
            errorMessage = null;
            return true;
        }

        //**********************************************
        // Get Tensor Data Alignment
        //**********************************************
        void parseDataAlignment()
        {
            var bytes = Encoding.UTF8.GetBytes("general.alignment");
            if (!MDs.ContainsKey(bytes))
            {
                DataAlignement = 32;
                return;
            }

            var md = MDs[bytes].MDValue as OzGGUF_UInt32;
            DataAlignement = md.Value;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(FileName))
                return FileName;
            return base.ToString();
        }
    }
}
