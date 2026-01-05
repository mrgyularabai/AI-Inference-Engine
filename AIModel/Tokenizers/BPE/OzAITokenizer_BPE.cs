using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAITokenizer_BPE : OzAITokenizer
    {
        protected override string GetSpaceEscape()
        {
            return "Ġ";
        }

        protected override bool ModelInit(OzGGUFFile file, out string error)
        {
            BeginningOfSequence = 11;
            EndOfSequence = 11;
            Unkown = -1;
            Separator = -1;
            Padding = -1;
            Classification = -1;
            Mask = -1;
            AddSpacePrefix = false;
            AddBOS = true;
            error = null;
            return true;
        }

        Stopwatch sw = new Stopwatch();
        protected override bool Tokenize(string text, List<int> tokens, out string times, out string error, bool allowUnk)
        {
            times = null;
            sw.Start();

            if (!mergeBytes(text, tokens, allowUnk, out error)) return false;

            sw.Stop();
            var tokensPerSec = Math.Round(tokens.Count / (sw.Elapsed.TotalMilliseconds / 1000));
            times = tokensPerSec.ToString();

            error = null;
            return true;
        }

        bool mergeBytes(string text, List<int> tokens, bool allowUnks, out string error)
        {
            error = null;
            if (text == null || text.Length == 0)
                return true;

            var bytes = Encoding.UTF8.GetBytes(text);

            for (int i = 0; i < bytes.Length; i++)
            {
                var len = Math.Min(bytes.Length - i, MaxTokenLen);
                var val = new byte[MaxTokenLen];
                Buffer.BlockCopy(bytes, i, val, 0, len);
                if (!TokenTree.Get(val, out var res))
                {
                    if (!resolveUnk(val[0], allowUnks, tokens, out error))
                        return false;
                    continue;
                }
                tokens.Add(res);
                i += Tokens[res].Text.Length - 1;
            }
            return true;
        }

        bool resolveUnk(byte text, bool allowUnks, List<int> res, out string error)
        {
            if (allowUnks)
            {
                res.Add(Unkown);
                error = null;
                return true;
            }

            if (!Byte2TokenID(text, out int id, out error))
                return false;

            res.Add(id);

            error = null;
            return true;
        }

    }
}
