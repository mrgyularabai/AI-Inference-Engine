using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzGGUF_String : OzGGUF_Item
    {
        public string Value 
        {
            get
            {
                return Encoding.UTF8.GetString(Bytes);
            }
            set
            {
                Bytes = Encoding.UTF8.GetBytes(value);
            }
        }

        public override bool Parse(Stream s, out string error)
        {
            var stringLength = new OzGGUF_UInt64();
            if (!stringLength.Parse(s, out error)) return false;

            Bytes = new byte[stringLength.Value];
            var bytesRead = s.Read(Bytes, 0, (int)stringLength.Value);

            if (bytesRead != (int)stringLength.Value)
            {
                error = "Could not read Int64. End of stream reached.";
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
