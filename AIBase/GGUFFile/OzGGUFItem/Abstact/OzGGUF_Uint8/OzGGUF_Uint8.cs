using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzGGUF_UInt8 : OzGGUF_Item
    {

        public Byte Value;

        public override bool Parse(Stream input, out string error)
        {
            Bytes = new byte[1];
            var bytesRead = input.Read(Bytes, 0, 1);

            if (bytesRead != 1)
            {
                error = "Could not read Uint8. End of stream reached";
                return false;
            }

            Value = (Byte)Bytes[0];
            error = null;
            return true;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
