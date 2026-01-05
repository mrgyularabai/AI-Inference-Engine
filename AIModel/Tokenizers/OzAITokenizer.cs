using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ozeki
{
    public abstract partial class OzAITokenizer
    {
        // Space Escaping
        public string SpaceEscape{

            get 
            {
                return GetSpaceEscape();
            }
        }
        protected abstract string GetSpaceEscape();
        public string UnescapeSpace(string text)
        {
            return text.Replace(SpaceEscape, " ");
        }

        // Initialization
        public static bool CreateFromFile(OzGGUFFile file, out OzAITokenizer res, out string error)
        {
            res = null;

            // Get tokenizer model name
            string model;
            if (!file.GetMDString($"tokenizer.ggml.model", out model, out error))
                return false;

            //llama.cpp supports: 'no_vocab', 'llama', 'bert', 't5'
            switch (model)
            {
                case "gpt2":
                    res = new OzAITokenizer_BPE();
                    break;
                case "llama":
                    res = new OzAITokenizer_SPM();
                    break;
                default:
                    error = $"Tokenizer model {model} not recognized.";
                    return false;
            }

            if (!res.InitFromFile(file, out error)) return false;

            return true;
        }

        protected abstract bool ModelInit(OzGGUFFile file, out string error);
        public bool InitFromFile(OzGGUFFile file, out string error)
        {
            if (!ModelInit(file, out error)) return false;

            if (!getTokData(file, out error)) return false;
            if (!ReadTokenIDFromMD(file, $"tokenizer.ggml.unknown_token_id", ref Unkown, out error))
                return false;
            buildTokTree();
            if (!handleSepecialTokens(file, out error)) return false;
            
            //if (!handleTokenAttrs(file, out error)) return false;

            return true;
        }

        public void buildTokTree()
        {
            TokenTree = new OzAITokenTree(Unkown);
            while (Len2ID.Count != 0)
            {
                var val = Len2ID.Dequeue();
                var tok = Tokens[val];
                TokenTree.Add(tok);
            }
        }
    }
}
