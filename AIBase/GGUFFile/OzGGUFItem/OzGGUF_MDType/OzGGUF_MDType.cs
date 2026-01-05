using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzGGUF_MDType : OzGGUF_UInt32
    {
        public MDTFormat Format
        {
            get { return (MDTFormat)Value; }
        }

        public enum MDTFormat : uint
        {
            UINT8 = 0,
            INT8 = 1,
            UINT16 = 2,
            INT16 = 3,
            UINT32 = 4,
            INT32 = 5,
            FLOAT32 = 6,
            BOOL = 7,
            STRING = 8,
            ARRAY = 9,
            UINT64 = 10,
            INT64 = 11,
            FLOAT64 = 12
        }

        public override string ToString()
        {
            switch(Format)
            {
                case MDTFormat.UINT8: return "UInt8";
                case MDTFormat.INT8: return "Int8";
                case MDTFormat.UINT16: return "UInt16";
                case MDTFormat.INT16: return "Int16";
                case MDTFormat.UINT32: return "UInt32";
                case MDTFormat.INT32: return "Int32";
                case MDTFormat.FLOAT32: return "Float32";
                case MDTFormat.BOOL: return "Bool";
                case MDTFormat.STRING: return "String";
                case MDTFormat.ARRAY: return "Array";
                case MDTFormat.UINT64: return "UInt64";
                case MDTFormat.INT64: return "Int64";
                case MDTFormat.FLOAT64: return "Float64";
                default: return "Unknown";
            }
        }
    }
}
