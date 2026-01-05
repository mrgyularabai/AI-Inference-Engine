using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAIToken
    {

        public int ID;
        public byte[] Text;
        public float Score; // the 'frequency' of a token

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(ID);
            sb.Append(": \"");
            sb.Append(Encoding.UTF8.GetString(Text));
            sb.Append("\"");
            return sb.ToString();
        }

        //public const int UNDEFINED = 0;
        //public const int UNKNOWN = 1 << 0;
        //public const int UNUSED = 1 << 1;
        //public const int NORMAL = 1 << 2;
        //public const int CONTROL = 1 << 3;  //llama.cpp: SPECIAL?
        //public const int USER_DEFINED = 1 << 4;
        //public const int BYTE = 1 << 5;
        //public const int NORMALIZED = 1 << 6;
        //public const int LSTRIP = 1 << 7;
        //public const int RSTRIP = 1 << 8;
        //public const int SINGLE_WORD = 1 << 9;
        //public int Type; // the type of a Token (Token Flags)

        /*public void SetType(int attr, bool value)
        {
            Type = value ? (Type | attr) : (Type & ~attr);
        }*/
    }
}
