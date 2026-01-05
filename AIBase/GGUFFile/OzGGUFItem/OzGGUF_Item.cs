using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzGGUF_Item
    {
        public byte[] Bytes;

        protected static byte[] Read(Stream s, int count)
        {
            return new byte[count];
        }

        public virtual bool Parse(Stream input, out string error)
        {
            error = "Not implemented";
            return false;
        }

        public bool TryParse(Stream s, out string error)
        {
            try
            {
                return Parse(s, out error); 
            }
            catch (Exception e)
            {
                error = "Could not parse item "+GetType().Name+" "+ e.Message;
                return false;
            }
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
