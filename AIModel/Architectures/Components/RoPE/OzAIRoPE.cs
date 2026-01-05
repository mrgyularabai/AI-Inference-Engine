using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public abstract class OzAIRoPE : OzAIArchComp
    {
        public OzAICompIOMem_Unary Mem;

        public CompHParams HParams;
        public class CompHParams : OzAICompHParams
        {
            public OzAIScalar ThetaBase;
            /// <summary>
            /// Like part for RMS norm but instead of a percentage of the total number of dimensions, <br/>
            /// the number of the first few dimensions is specified, for where rope should be applied.
            /// </summary>
            public uint DimCount;
            public OzAIRoPE_ScalingType Scaling = OzAIRoPE_ScalingType.Linear;
            public OzAIRoPE_Scaling.ScalingParams ScalingParams;

            public override bool SetDefaults(OzAIProcMode mode, out string error)
            {
                if (!OzAIScalar.Create(10000, mode, out ThetaBase, out error))
                    return false;
                return true;
            }

            public override bool InitFromFile(OzAIProcMode mode, OzGGUFFile file, out string error)
            {
                // Base
                if (!file.GetMDFloat32($"{file.Architecture}.rope.freq_base", out var thetaBaseF, out error, false, 10000.0f) && error != null)
                    return false;

                if (!OzAIScalar.CreateFloat(thetaBaseF, mode, out ThetaBase, out error))
                    return false;

                // DimCount
                if (!file.GetMDUInt32($"{file.Architecture}.context_length", out var contextLen, out error))
                    return false;

                if (!file.GetMDUInt32($"{file.Architecture}.embedding_length", out var embedLen, out error))
                    return false;

                var defaultDimCount = embedLen / contextLen;
                if (!file.GetMDUInt32($"{file.Architecture}.rope.dimension_count", out DimCount, out error, false, defaultDimCount) && error != null)
                    return false;

                // Scaling
                if (!OzAIRoPE_Scaling.ScalingParams.CreateFromFile(file, out ScalingParams, out Scaling, out error))
                    return false;

                return true;
            }

            public override bool IsPossible(out string error)
            {
                if (!CheckIfNull(ThetaBase, "theta base", out error))
                    return false;
                if (!ThetaBase.Vector.GetNth(ThetaBase.Offset, out var thetaBaseVal, out error))
                    return false;
                if (thetaBaseVal < 0)
                {
                    error = "Cannot calculate RoPE thetas with a base less than 0";
                    return false;
                }
                if (thetaBaseVal == 0)
                {
                    error = "Cannot calculate RoPE thetas with a base of 0 (this will always equal to 1)";
                    return false;
                }
                if (DimCount == 0)
                {
                    error = "Cannot calculate RoPE for 0 features of a vector";
                    return false;
                }
                error = null;
                return true;
            }
        }

        OzAIRoPE_Scaling Scaling;
    }
}
