using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzGGUF_UInt32 : OzGGUF_Item
    {

        public UInt32 Value;

        public override bool Parse(Stream input, out string error)
        {
            Bytes = new byte[4];
            var bytesRead = input.Read(Bytes, 0, 4);
            if (bytesRead != 4)
            {
                error = "Could not read Uint32. End of stream reached";
                return false;
            }

            Value = BitConverter.ToUInt32(Bytes, 0);

            error = null;
            return true;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
