using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Ozeki.OzGGUF_NumType;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ozeki
{
    public static partial class NumTypeExtender
    {
        public static bool CreateMat(this OzAINumType self, OzAIProcMode mode, out OzAIMatrix res, out string error)
        {
            res = null;
            switch (self)
            {
                case OzAINumType.Int8:
                    break;
                case OzAINumType.Int16:
                    break;
                case OzAINumType.Int32:
                    break;
                case OzAINumType.Int64:
                    break;
                case OzAINumType.BrainFloat16:
                    break;
                case OzAINumType.Float16:
                    if (!OzAIHalfMat.Create(mode, out var resfp16, out error))
                        return false;
                    res = resfp16;
                    return true;
                case OzAINumType.Float32:
                    if (!OzAIFloatMat.Create(mode, out var resfp32, out error))
                        return false;
                    res = resfp32;
                    return true;
                case OzAINumType.Float64:
                    break;
                case OzAINumType.Q4_0:
                    break;
                case OzAINumType.Q4_0_4_4:
                    break;
                case OzAINumType.Q4_0_4_8:
                    break;
                case OzAINumType.Q4_0_8_8:
                    break;
                case OzAINumType.Q4_1:
                    break;
                case OzAINumType.Q5_0:
                    break;
                case OzAINumType.Q5_1:
                    break;
                case OzAINumType.Q8_0:
                    break;
                case OzAINumType.Q8_1:
                    break;
                case OzAINumType.Q2_K:
                    break;
                case OzAINumType.Q3_K:
                    break;
                case OzAINumType.Q4_K:
                    break;
                case OzAINumType.Q5_K:
                    break;
                case OzAINumType.Q6_K:
                    break;
                case OzAINumType.Q8_K:
                    break;
                case OzAINumType.IQ1_S:
                    break;
                case OzAINumType.IQ1_M:
                    break;
                case OzAINumType.IQ2_XXS:
                    break;
                case OzAINumType.IQ2_XS:
                    break;
                case OzAINumType.IQ2_S:
                    break;
                case OzAINumType.IQ3_XXS:
                    break;
                case OzAINumType.IQ3_S:
                    break;
                case OzAINumType.IQ4_XS:
                    break;
                case OzAINumType.IQ4_NL:
                    break;
                default:
                    break;
            }
            res = null;
            error = $"Cannot not create matrix of type {self}, because it is not implemented yet.";
            return false;
        }
    }
}
