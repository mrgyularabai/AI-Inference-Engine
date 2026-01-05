using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzGGUF_Int8 : OzGGUF_Item
    {

        public sbyte Value;

        public override bool Parse(Stream input, out string error)
        {
            Bytes = new byte[1];
            var bytesRead = input.Read(Bytes, 0, 1);
            if (bytesRead != 1)
            {
                error = "Could not read Int8. End of stream reached.";
                return false;
            }

            Value = (sbyte)Bytes[0];
            error = null;
            return true;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
