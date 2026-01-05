using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public partial class OzGGUF_NumType : OzGGUF_UInt32
    {
        public NTFormat Format
        {
            get { return (NTFormat)Value; }
            set { Value = (uint)value; }
        }

        public enum NTFormat : uint
        {
            F32 = 0,
            F16 = 1,
            Q4_0 = 2,
            Q4_1 = 3,
            Q5_0 = 6,
            Q5_1 = 7,
            Q8_0 = 8,
            Q8_1 = 9,
            Q2_K = 10,
            Q3_K = 11,
            Q4_K = 12,
            Q5_K = 13,
            Q6_K = 14,
            Q8_K = 15, 
            IQ2_XXS = 16,
            IQ2_XS = 17,
            IQ3_XXS = 18,
            IQ1_S = 19,
            IQ4_NL = 20,
            IQ3_S = 21,
            IQ2_S = 22,
            IQ4_XS = 23,
            I8 = 24,
            I16 = 25,
            I32 = 26,
            I64 = 27,
            F64 = 28,
            IQ1_M = 29,
            BF16 = 30,
            Q4_0_4_4 = 31,
            Q4_0_4_8 = 32,
            Q4_0_8_8 = 33,
            UNKNOWN = int.MaxValue
        }

        public static OzGGUF_NumType CreateFromString(string s)
        {
            var ret = new OzGGUF_NumType();
            ret.Format = StringToNTFormat(s);
            return ret;
        }

        public static NTFormat StringToNTFormat(string s)
        {
            switch (s)
            {
                case "F32": 
                    return NTFormat.F32;
                case "F16":
                    return NTFormat.F16;
                case "Q4_0":
                    return NTFormat.Q4_0;
                case "Q4_1":
                    return NTFormat.Q4_1;
                case "Q5_0":
                    return NTFormat.Q5_0;
                case "Q5_1":
                    return NTFormat.Q5_1;
                case "Q8_0":
                    return NTFormat.Q8_0;
                case "Q8_1":
                    return NTFormat.Q8_1;
                case "Q2_K":
                    return NTFormat.Q2_K;
                case "Q3_K":
                    return NTFormat.Q3_K;
                case "Q4_K":
                    return NTFormat.Q4_K;
                case "Q5_K":
                    return NTFormat.Q5_K;
                case "Q6_K":
                    return NTFormat.Q6_K;
                case "Q8_K":
                    return NTFormat.Q8_K;
                case "IQ2_XXS":
                    return NTFormat.IQ2_XXS;
                case "IQ2_XS":
                    return NTFormat.IQ2_XS;
                case "IQ3_XXS":
                    return NTFormat.IQ3_XXS;
                case "IQ1_S":
                    return NTFormat.IQ1_S;
                case "IQ4_NL":
                    return NTFormat.IQ4_NL;
                case "IQ3_S":
                    return NTFormat.IQ3_S;
                case "IQ2_S":
                    return NTFormat.IQ2_S;
                case "IQ4_XS":
                    return NTFormat.IQ4_XS;
                case "I8":
                    return NTFormat.I8;
                case "I16":
                    return NTFormat.I16;
                case "I32":
                    return NTFormat.I32;
                case "I64":
                    return NTFormat.I64;
                case "F64":
                    return NTFormat.F64;
                case "IQ1_M":
                    return NTFormat.IQ1_M;
                case "BF16":
                    return NTFormat.BF16;
                case "Q4_0_4_4":
                    return NTFormat.Q4_0_4_4;
                case "Q4_0_4_8":
                    return NTFormat.Q4_0_4_8;
                case "Q4_0_8_8":
                    return NTFormat.Q4_0_8_8;
                default:
                    break;
            }
            return NTFormat.UNKNOWN;
        }

        public OzAINumType ToAINumType()
        {
            switch ((NTFormat)Value)
            {
                case NTFormat.F32:
                    return OzAINumType.Float32;
                case NTFormat.F16:
                    return OzAINumType.Float16;
                case NTFormat.Q4_0:
                    return OzAINumType.Q4_0;
                case NTFormat.Q4_1:
                    return OzAINumType.Q4_1;
                case NTFormat.Q5_0:
                    return OzAINumType.Q5_0;
                case NTFormat.Q5_1:
                    return OzAINumType.Q5_1;
                case NTFormat.Q8_0:
                    return OzAINumType.Q8_0;
                case NTFormat.Q8_1:
                    return OzAINumType.Q8_1;
                case NTFormat.Q2_K:
                    return OzAINumType.Q2_K;
                case NTFormat.Q3_K:
                    return OzAINumType.Q3_K;
                case NTFormat.Q4_K:
                    return OzAINumType.Q4_K;
                case NTFormat.Q5_K:
                    return OzAINumType.Q5_K;
                case NTFormat.Q6_K:
                    return OzAINumType.Q6_K;
                case NTFormat.Q8_K:
                    return OzAINumType.Q8_K;
                case NTFormat.IQ2_XXS:
                    return OzAINumType.IQ2_XXS;
                case NTFormat.IQ2_XS:
                    return OzAINumType.IQ2_XS;
                case NTFormat.IQ3_XXS:
                    return OzAINumType.IQ3_XXS;
                case NTFormat.IQ1_S:
                    return OzAINumType.IQ1_S;
                case NTFormat.IQ4_NL:
                    return OzAINumType.IQ4_NL;
                case NTFormat.IQ3_S:
                    return OzAINumType.IQ3_S;
                case NTFormat.IQ2_S:
                    return OzAINumType.IQ2_S;
                case NTFormat.IQ4_XS:
                    return OzAINumType.IQ4_XS;
                case NTFormat.I8:
                    return OzAINumType.Int8;
                case NTFormat.I16:
                    return OzAINumType.Int16;
                case NTFormat.I32:
                    return OzAINumType.Int32;
                case NTFormat.I64:
                    return OzAINumType.Int64;
                case NTFormat.F64:
                    return OzAINumType.Float64;
                case NTFormat.IQ1_M:
                    return OzAINumType.IQ1_M;
                case NTFormat.BF16:
                    return OzAINumType.BrainFloat16;
                case NTFormat.Q4_0_4_4:
                    return OzAINumType.Q4_0_4_4;
                case NTFormat.Q4_0_4_8:
                    return OzAINumType.Q4_0_4_8;
                case NTFormat.Q4_0_8_8:
                    return OzAINumType.Q4_0_8_8;
            }

            return OzAINumType.Int8;
        }

        public override string ToString()
        {
            if ((int)Format < 0 || (int)Format > 33)
                return "Unknown";

            return Format.ToString();
        }
    }
}
