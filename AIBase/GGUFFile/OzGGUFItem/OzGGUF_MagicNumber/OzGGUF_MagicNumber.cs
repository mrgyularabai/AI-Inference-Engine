using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzGGUF_MagicNumber : OzGGUF_Item
    {
        public const string Value = "GGUF";
        public int Version;
        public override bool Parse(Stream input, out string error)
        {
            var res = new byte[4];
            input.Read(res, 0, 4);

            var val = Encoding.ASCII.GetBytes(Value);

            for (int i = 0; i < 4; i++)
            {
                if (res[i] != val[i] )
                {
                    error = "File's Magic Number is not as required. This is not a GGUF file.";
                    return false;
                }
            }

            error = null;
            return true;
        }
    }
}
