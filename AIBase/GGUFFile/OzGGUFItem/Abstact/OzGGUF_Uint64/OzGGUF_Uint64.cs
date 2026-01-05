using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzGGUF_UInt64 : OzGGUF_Item
    {
        public UInt64 Value;

        public override bool Parse(Stream input, out string error)
        {
            Bytes = new byte[8];
            var bytesRead = input.Read(Bytes, 0, 8);

            if (bytesRead != 8)
            {
                error = "Could not read UInt64. End of stream reached";
                return false;
            }

            Value = BitConverter.ToUInt64(Bytes, 0);
    
            error = null;
            return true;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
