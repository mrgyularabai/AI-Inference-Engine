using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ozeki
{
    public partial class OzAITokenizer
    {
        public List<OzAIToken> Tokens;
        public PriorityQueue<int, int> Len2ID;
        public OzAITokenTree TokenTree;
        public int MaxTokenLen;
        public float AvgTokenLen;

        bool getTokData(OzGGUFFile file, out string error)
        {
            if (!GetTokenList(file, out error)) return false;
            //if (!getTokenScores(file, out error)) return false;
            //if (!getTokenTypes(file, out error)) return false;

            return true;
        }
        public class ByteArrayComparer : IEqualityComparer<byte[]>
        {
            public bool Equals(byte[] left, byte[] right)
            {
                if (left == null || right == null)
                {
                    return left == right;
                }
                if (left.Length != right.Length)
                {
                    return false;
                }
                for (int i = 0; i < left.Length; i++)
                {
                    if (left[i] != right[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            public int GetHashCode(byte[] key)
            {
                return key.GetHashCode();
            }
        }


        public bool GetTokenList(OzGGUFFile file, out string error)
        {
            OzGGUF_Item item;
            if (!file.GetMD($"tokenizer.ggml.tokens", out item, out error, false))
                return error == null;

            try
            {
                OzGGUF_Array array = item as OzGGUF_Array;

                Tokens = new List<OzAIToken>((int)array.Count.Value);
                Len2ID = new PriorityQueue<int, int>((int)array.Count.Value);

                for (int i = 0; i < (int)array.Count.Value; i++)
                {
                    var token = new OzAIToken();

                    var text = array.Value[i] as OzGGUF_String;
                    var escaped = text.Value;
                    var unescaped = UnescapeSpace(escaped);
                    token.Text = Encoding.UTF8.GetBytes(unescaped);

                    token.ID = i;
                    Tokens.Add(token);
                    Len2ID.Enqueue(token.ID, token.Text.Length);

                    MaxTokenLen = Math.Max(MaxTokenLen, token.Text.Length);
                    AvgTokenLen += token.Text.Length;
                }
                AvgTokenLen /= array.Count.Value;

            }
            catch (Exception e)
            {
                error = "Failed to read the list of tokens: " + e.Message;
                return false;
            }

            return true;
        }

        //bool getTokenScores(OzGGUFFile file, out string error)
        //{
        //    OzGGUF_Item item;
        //    if (!file.GetMD($"tokenizer.ggml.scores", out item, out error, false))
        //        return error == null;

        //    try
        //    {
        //        OzGGUF_Array array = item as OzGGUF_Array;

        //        for (int i = 0; i < (int)array.Count.Value; i++)
        //        {
        //            var score = array.Value[i] as OzGGUF_Float32;
        //            Tokens[i].Score = score.Value;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        error = "Failed to read the token scores";
        //        return false;
        //    }

        //    return true;
        //}

        //bool getTokenTypes(OzGGUFFile file, out string error)
        //{
        //    OzGGUF_Item item;
        //    if (!file.GetMD($"tokenizer.ggml.token_type", out item, out error, false))
        //        return error == null;

        //    try
        //    {
        //        OzGGUF_Array array = item as OzGGUF_Array;
        //        for (int i = 0; i < (int)array.Count.Value; i++)
        //        {
        //            var type = array.Value[i] as OzGGUF_Int32;
        //            TokenList[i].Type = type.Value;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        error = "Failed to read the token types";
        //        return false;
        //    }

        //    return true;
        //}
    }
}
