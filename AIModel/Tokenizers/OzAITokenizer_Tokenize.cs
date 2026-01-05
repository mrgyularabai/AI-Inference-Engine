using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public partial class OzAITokenizer
    {
        public bool GetTokens(string text, out List<int> tokens, out string times, out string error, bool allowUnk = false)
        {
            tokens = new List<int>();
            times = null;

            if (!addBOS(tokens, out error)) return false;
            if (AddSpacePrefix)
                text = " " + text;
            if (!Tokenize(text, tokens, out times, out error, allowUnk && Unkown != -1)) return false;
            if (!addEOS(tokens, out error)) return false;

            return true;
        }

        bool addBOS(List<int> tokens, out string error)
        {
            if (AddBOS)
            {
                if (BeginningOfSequence == -1)
                {
                    error = "No BOS token found.";
                    tokens = null;
                    return false;
                }
                tokens.Add(BeginningOfSequence);
            }

            error = null;
            return true;
        }

        protected abstract bool Tokenize(string text, List<int> tokens, out string times, out string error, bool allowUnk);

        bool addEOS(List<int> tokens, out string error)
        {
            if (AddEOS)
            {
                if (EndOfSequence == -1)
                {
                    error = "No BOS token found.";
                    tokens = null;
                    return false;
                }
                tokens.Add(EndOfSequence);
            }

            error = null;
            return true;
        }

        public bool ByteArray2Tokens(byte[] values, out List<int> res, out string error)
        {
            res = new List<int>();

            for (int i = 0; i < values.Length; i++)
            {
                if (!Byte2TokenID(values[i], out var val, out error))
                    return false;
                res.Add(val);
            }

            error = null;
            return true;
        }

        public bool Byte2TokenID(byte value, out int res, out string error)
        {
            if (TokenTree.Get([value], out res))
            {
                error = null;
                return true;
            }

            // Check Sentence Piece
            string hex = "0123456789ABCDEF";
            char[] buf = ['<', '0', 'x', hex[value >> 4], hex[value & 15], '>'];
            var text = new string(buf);
            var bytes = Encoding.UTF8.GetBytes(text);
            if (TokenTree.Get(bytes, out res))
            {
                var newToken = new OzAIToken()
                {
                    ID = res,
                    Text = [value]
                };
                TokenTree.Add(newToken);
                Tokens[res].Text = [value];
                error = null;
                return true;
            }

            error = $"Token list does not contain given byte in neither the form {text} nor as a utf-8 byte.";
            return false;
        }

        public string VisualizeTokens(List<int> tokens)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[ ");
            foreach (var tok in tokens)
            {
                if (tok == Unkown)
                {
                    sb.Append("(");
                    sb.Append(Tokens[Unkown]);
                    sb.Append(") ");
                }
                sb.Append(" {");
                sb.Append(Tokens[tok].ToString());
                sb.Append("}, ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Replace("\r", "\\r");
            sb.Replace("\n", "\\n");
            sb.Append("  ]");
            return sb.ToString();
        }

        public string GetStringsRaw(List<int> tokens)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < tokens.Count; i++)
            {
                var id = tokens[i];
                var tok = Tokens[id];
                var val = Encoding.UTF8.GetString(tok.Text);
                sb.Append(val);
            }
            return sb.ToString();
        }

        public string GetStrings(List<int> tokens)
        {
            int start = AddBOS ? 1 : 0;
            int end = AddEOS ? 1 : 0;
            StringBuilder sb = new StringBuilder();
            if (AddSpacePrefix)
            {
                var id = tokens[start];
                var tok = Tokens[id];
                var val = Encoding.UTF8.GetString(tok.Text, 1, tok.Text.Length-1);
                sb.Append(val);
                start++;
            }
            for (int i = start; i < tokens.Count - end; i++)
            {
                var id = tokens[i];
                var tok = Tokens[id];
                var val = Encoding.UTF8.GetString(tok.Text);
                sb.Append(val);
            }
            return sb.ToString();
        }
    }
}
