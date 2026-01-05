
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Ozeki
{
    public class OzGGUF_MD : OzGGUF_Item
    {
        public OzGGUF_String MDName;

        public OzGGUF_MDType MDType;

        public OzGGUF_Item MDValue;

        public override bool Parse(Stream s, out string error)
        {
            MDName = new OzGGUF_String();
            MDType = new OzGGUF_MDType();
            if (!MDName.Parse(s, out error)) return false;
            if (!MDType.Parse(s, out error)) return false;
            if (!ParseMDValue(s, MDType, out MDValue, out error)) return false;
            return true;
        }

        public static bool ParseMDValue(Stream s, OzGGUF_MDType mdType, out OzGGUF_Item mdValue,  out string error)
        {
            switch (mdType.Format)
            {
                case OzGGUF_MDType.MDTFormat.STRING: //8
                    mdValue = new OzGGUF_String();
                    mdValue.Parse(s, out error);
                    break;

                case OzGGUF_MDType.MDTFormat.INT32: //5
                    mdValue = new OzGGUF_Int32();
                    if (!mdValue.Parse(s, out error))
                        return true;
                    break;

                case OzGGUF_MDType.MDTFormat.UINT32: //4
                    mdValue = new OzGGUF_UInt32();
                    mdValue.Parse(s, out error);
                    break;

                case OzGGUF_MDType.MDTFormat.FLOAT32: //6
                    mdValue = new OzGGUF_Float32();
                    mdValue.Parse(s, out error);
                    break;

                case OzGGUF_MDType.MDTFormat.ARRAY: //9
                    mdValue = new OzGGUF_Array();
                    mdValue.Parse(s, out error);
                    break;


                case OzGGUF_MDType.MDTFormat.UINT64: //10
                    mdValue = new OzGGUF_UInt64();
                    mdValue.Parse(s, out error);
                    break;


                
                // Less frequently used.
                case OzGGUF_MDType.MDTFormat.UINT8: //0
                    mdValue = new OzGGUF_UInt8();
                    mdValue.Parse(s, out error);
                    break;

                case OzGGUF_MDType.MDTFormat.INT8: //1
                    mdValue = new OzGGUF_Int8();
                    mdValue.Parse(s, out error);
                    break;

                case OzGGUF_MDType.MDTFormat.UINT16: //2
                    mdValue = new OzGGUF_UInt16();
                    mdValue.Parse(s, out error);
                    break;

                case OzGGUF_MDType.MDTFormat.INT16: //3
                    mdValue = new OzGGUF_Int16();
                    mdValue.Parse(s, out error);
                    break;

                case OzGGUF_MDType.MDTFormat.BOOL: //7
                    mdValue = new OzGGUF_Bool();
                    mdValue.Parse(s, out error);
                    break;

                case OzGGUF_MDType.MDTFormat.INT64: //11
                    mdValue = new OzGGUF_Int64();
                    mdValue.Parse(s, out error);
                    break;

                case OzGGUF_MDType.MDTFormat.FLOAT64: //12
                    mdValue = new OzGGUF_Float64();
                    break;

                default:
                    error = "Unknown Meta Data format";
                    mdValue = null;
                    return false;
            }

            error = "";
            return true;
        }

        public override string ToString()
        {
            if (MDName == null) return base.ToString();

            var s = new StringBuilder();
            s.Append(MDName.ToString());
            s.Append(": ");
            s.Append(MDValue.ToString());
            return s.ToString();
        }
    }
}
