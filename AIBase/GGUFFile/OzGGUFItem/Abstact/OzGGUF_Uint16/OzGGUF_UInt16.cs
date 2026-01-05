using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzGGUF_UInt16 : OzGGUF_Item
    {

        public UInt16 Value;

        public override bool Parse(Stream input, out string error)
        {
            Bytes = new byte[2];
            var bytesRead = input.Read(Bytes, 0, 2);
            if (bytesRead != 2)
            {
                error = "Could not read Uint16. End of stream reached";
                return false;
            }

            Value = BitConverter.ToUInt16(Bytes, 0);

            error = null;
            return true;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
