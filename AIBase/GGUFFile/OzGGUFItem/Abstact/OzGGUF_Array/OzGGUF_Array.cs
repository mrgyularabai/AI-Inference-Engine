using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzGGUF_Array : OzGGUF_Item
    {
        public OzGGUF_MDType MDType;
        public OzGGUF_UInt64 Count;
        public List<OzGGUF_Item> Value;

        public override bool Parse(Stream s, out string error)
        {
            MDType = new OzGGUF_MDType(); 
            if (!MDType.Parse(s, out error)) return false;

            Count = new OzGGUF_UInt64();
            if (!Count.Parse(s, out error)) return false;

            Value = new List<OzGGUF_Item>((int)Count.Value);


            for (UInt64 i = 0; i < Count.Value; i++)
            {
                if (!OzGGUF_MD.ParseMDValue(s, MDType, out var mdValue, out error)) return false;
                Value.Add(mdValue);
            }
    
            error = null;
            return true;
        }

        public bool ParseInt(Stream s, out string error)
        {
            for (UInt64 i = 0; i < Count.Value; i++)
            {
                if (!OzGGUF_MD.ParseMDValue(s, MDType, out var mdValue, out error)) return false;
                Value.Add(mdValue);
            }
            error = null;
            return true;
        }

        public override string ToString()
        {
            if (MDType == null) return base.ToString();
            var ret = new StringBuilder();
            ret.Append("Array of " + MDType.ToString()+" with "+Count.ToString()+" items");
            return ret.ToString();
        }
    }
}
