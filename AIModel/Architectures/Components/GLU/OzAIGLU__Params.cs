using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    partial class OzAIGLU
    {
        public OzAICompIOMem_Unary Mem;

        public CompHParams HParams;
        public class CompHParams : OzAICompHParams
        {
            public ulong FFNLength = 8192;
            public OzAICompHParams ActivationParams;
            public override bool IsPossible(out string error)
            {
                if (FFNLength == 0)
                {
                    error = "Cannot compute a gated linear unit with 0 Feed Forward Length.";
                    return false;
                }
                if (FFNLength >= int.MaxValue)
                {
                    error = "Cannot compute a gated linear unit with Feed Forward Length greater than the max int 32 limit.";
                    return false;
                }
                if (ActivationParams != null)
                {
                    if (!ActivationParams.IsPossible(out error)) return false;
                }
                error = null;
                return true;
            }
        }

        public CompIParams IParams;
        public class CompIParams : OzAICompIParams
        {
            public OzAIMatrix WeightsTop;
            public OzAIMatrix WeightsGate;
            public OzAIMatrix WeightsBottom;
            public OzAIVector BiasTop;
            public OzAIVector BiasBottom;
            public OzAIActivation Activation;
            public OzAICompIParams ActivationParams;

            public override bool IsPossible(out string error)
            {
                List<object> objs = [ExecManager, Activation, ActivationParams, WeightsBottom, WeightsGate, WeightsTop];
                List<string> names = ["ExecManager", "Activation", "ActivationIParams", "WeightsBottom", "WeightsGate", "WeightsTop"];
                if (!CheckIfNull(objs, names, out error)) return false;
                if (!ActivationParams.IsPossible(out error)) return false;
                return true;
            }
        }

        public override bool IsPossible(out string error)
        {
            if (Params.Mem is not OzAICompIOMem_Unary)
            {
                error = "IO memory required to be in a format for a unary op OzAICompIOMem_Unary.";
                return false;
            }
            if (Params.IParams is not CompIParams)
            {
                error = "Instance parameters required to be in a format for a OzAIEmbedding.CompIParams.";
                return false;
            }
            if (Params.HParams is not CompHParams)
            {
                error = "Hyperparameters required to be in a format for a OzAIEmbedding.CompHParams.";
                return false;
            }

            Mem = Params.Mem as OzAICompIOMem_Unary;
            IParams = Params.IParams as CompIParams;
            HParams = Params.HParams as CompHParams;

            if (!IParams.WeightsGate.GetHeight(out var gateH, out error))
                return false;
            if (HParams.FFNLength != gateH)
            {
                error = "Feed forward length differs from the height of the gate weights matrix.";
                return false;
            }

            if (!IParams.WeightsTop.GetHeight(out var topH, out error))
                return false;
            if (HParams.FFNLength != topH)
            {
                error = "Feed forward length differs from the height of the top weights matrix.";
                return false;
            }

            if (!IParams.WeightsBottom.GetWidth(out var bottomW, out error))
                return false;
            if (HParams.FFNLength != bottomW)
            {
                error = "Feed forward length differs from the width of the bottom weights matrix.";
                return false;
            }

            error = null;
            return true;
        }
    }
}
