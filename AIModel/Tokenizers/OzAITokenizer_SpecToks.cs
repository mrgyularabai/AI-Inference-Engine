using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ozeki
{
    public partial class OzAITokenizer
    {
        // Special token IDs
        public int EndOfSequence; //eos
        public int BeginningOfSequence; //bos
        public int Unkown; //unk
        public int Separator; //sep
        public int Padding; //pad
        public int Classification; //cls
        public int Mask;
        public int Linefeed;
        public int Prefix;
        public int Suffix;
        public int Middle;
        public int EndOfText; //eot
        public int EndOfMessage; //eom
        public int Space;

        public bool HasSpaceToken;
        public bool RemoveExtraWhiteSpaces;
        public bool AddSpacePrefix;
        public bool AddBOS;
        public bool AddEOS;

        bool handleSepecialTokens(OzGGUFFile file, out string error)
        {
            if (!checkMDs(file, out error)) return false;
            if (!seachForEOT(out error)) return false;
            HasSpaceToken = TryString2Token(" ", out Space);
            if (!seachForEOM(out error)) return false;

            error = null;
            return true;
        }

        bool checkMDs(OzGGUFFile file, out string error)
        {
            // Special tokens
            if (!ReadTokenIDFromMD(file, $"tokenizer.ggml.bos_token_id", ref BeginningOfSequence, out error))
                return false;
            if (!ReadTokenIDFromMD(file, $"tokenizer.ggml.eos_token_id", ref EndOfSequence, out error))
                return false;
            if (!ReadTokenIDFromMD(file, $"tokenizer.ggml.unknown_token_id", ref Unkown, out error))
                return false;
            if (!ReadTokenIDFromMD(file, $"tokenizer.ggml.seperator_token_id", ref Separator, out error))
                return false;
            if (!ReadTokenIDFromMD(file, $"tokenizer.ggml.padding_token_id", ref Padding, out error))
                return false;
            if (!ReadTokenIDFromMD(file, $"tokenizer.ggml.cls_token_id", ref Classification, out error))
                return false;
            if (!ReadTokenIDFromMD(file, $"tokenizer.ggml.mask_token_id", ref Mask, out error))
                return false;
            if (!ReadTokenIDFromMD(file, $"tokenizer.ggml.prefix_token_id", ref Prefix, out error))
                return false;
            if (!ReadTokenIDFromMD(file, $"tokenizer.ggml.suffix_token_id", ref Suffix, out error))
                return false;
            if (!ReadTokenIDFromMD(file, $"tokenizer.ggml.middle_token_id", ref Middle, out error))
                return false;
            if (!ReadTokenIDFromMD(file, $"tokenizer.ggml.eot_token_id", ref EndOfText, out error))
                return false;
            if (!ReadTokenIDFromMD(file, $"tokenizer.ggml.eom_token_id", ref EndOfMessage, out error))
                return false;

            // Special Operations
            if (!file.GetMDBool($"tokenizer.ggml.remove_extra_whitespaces", out RemoveExtraWhiteSpaces, out error, false, RemoveExtraWhiteSpaces) && error != null)
                return false;
            if (!file.GetMDBool($"tokenizer.ggml.add_space_prefix", out AddSpacePrefix, out error, false, AddSpacePrefix) && error != null)
                return false;
            if (!file.GetMDBool($"tokenizer.ggml.add_bos_token", out AddEOS, out error, false, AddEOS) && error != null)
                return false;
            if (!file.GetMDBool($"tokenizer.ggml.add_eos_token", out AddBOS, out error, false, AddBOS) && error != null)
                return false;

            return true;
        }

        public bool ReadTokenIDFromMD(OzGGUFFile file, string name, ref int res, out string error)
        {
            uint val;

            if (!file.GetMDUInt32(name, out val, out error, false) && error != null)
            {
                res = -1;
                return false;
            }

            if (val == uint.MaxValue)
                return true;

            res = (int)val;
            return true;
        }

        // if eot was still not defined then search for it in the token list
        // llama.cpp does linear seach on tokenslist but this is faster:
        bool seachForEOT(out string error)
        {
            //llama.cpp: TODO: convert scripts should provide this token through the KV metadata LLAMA_KV_TOKENIZER_EOT_ID
            //llama.cpp:       for now, we apply this workaround to find the EOT token based on its text
            //llama.cpp: TODO: gemma "<end_of_turn>" is exported as a normal token, so the following check [is the token type control] does not work
            //llama.cpp:       need to fix convert script
            // The last one is fixed automatically by the code below
            error = null;

            if (EndOfText != -1) return true;
            if (TryString2Token("<|eot_id|>", out EndOfText))
                return true;
            if (TryString2Token("<|im_end|>", out EndOfText))
                return true;
            if (TryString2Token("<|end|>", out EndOfText))
                return true;
            if (TryString2Token("<|end_of_turn|>", out EndOfText))
                return true;
            if (TryString2Token("<|endoftext|>", out EndOfText))
                return true;
            error = "Could not find end of text token!";
            return false;
        }

        // if eom was still not defined then search for it in the token list
        bool seachForEOM(out string error)
        {
            //llama.cpp: TODO: convert scripts should provide this token through the KV metadata LLAMA_KV_TOKENIZER_EOM_ID
            //llama.cpp:       for now, we apply this workaround to find the EOM token based on its text

            error = null;

            if (EndOfMessage != -1) return true;
            if (TryString2Token("<|eom_id|>", out EndOfMessage))
                return true;
            error = "Could not find end of message token!";
            return false;
        }

        bool TryString2Token(string input, out int token)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            return TokenTree.Get(bytes, out token);
        }
    }
}
