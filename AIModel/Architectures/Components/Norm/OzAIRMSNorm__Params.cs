using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    partial class OzAIRMSNorm
    {

        public OzAICompIOMem_Unary Mem;

        public CompIParams IParams;
        public class CompIParams : OzAICompIParams
        {
            public OzAIScalar Part;
            public OzAIVector Gain;

            public override bool SetDefaults(OzAIProcMode mode, out string error)
            {
                if (!OzAIScalar.Create(1f, mode, out Part, out error))
                    return false;
                error = null;
                return true;
            }

            public override bool IsPossible(out string error)
            {
                if (!CheckForExec(out error)) return false;
                List<object> objs = [Part, Gain];
                List<string> names = ["Part", "Gain Weights"];
                if (!CheckIfNull(objs, names, out error))
                    return false;
                if (!Part.IsValid(out error))
                    return false;

                if (!Part.Vector.GetNth(Part.Offset, out var partVal, out error))
                    return false;

                if (partVal == 0)
                {
                    error = "Cannot apply RMS to 0 elements of a vector.";
                    return false;
                }

                return true;
            }
        }

        public CompHParams HParams;
        public class CompHParams : OzAICompHParams
        {
            public OzAIScalar Epsilon;

            public override bool SetDefaults(OzAIProcMode mode, out string error)
            {
                if (!OzAIScalar.Create(float.MinValue, mode, out Epsilon, out error))
                    return false;
                error = null;
                return true;
            }

            public override bool IsPossible(out string error)
            {
                return CheckIfNull(Epsilon, "Epsilon", out error);
            }
        }

        public override bool IsPossible(out string error)
        {
            if (Params.Mem is not OzAICompIOMem_Unary)
            {
                error = "IO memory required to be in a format for a unary op.";
                return false;
            }
            if (Params.IParams is not CompIParams)
            {
                error = "Instance parameters required to be in a format for a OzAIRMSNorm.CompIParams.";
                return false;
            }
            if (Params.HParams is not CompHParams)
            {
                error = "Hyperparameters required to be in a format for a OzAIRMSNorm.CompHParams.";
                return false;
            }

            Mem = Params.Mem as OzAICompIOMem_Unary;
            IParams = Params.IParams as CompIParams;
            HParams = Params.HParams as CompHParams;

            error = null;
            return true;
        }
    }
}
