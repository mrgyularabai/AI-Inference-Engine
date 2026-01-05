using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzGGUF_Bool : OzGGUF_Item
    {

        public bool Value;

        public override bool Parse(Stream input, out string error)
        {
            Bytes = new byte[1];
            var bytesRead = input.Read(Bytes, 0, 1);
            if (bytesRead != 1)
            {
                error = "Could not read bool. End of stream reached.";
                return false;
            }

            if (Bytes[0] == 0)
                Value = false;
            else if (Bytes[0] == 0)
                Value = true;
            else 
            { 
                Value = false;
                error = "Invalid bool value";
                return false;
            }

                error = null;
            return true;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
