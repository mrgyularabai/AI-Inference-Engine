using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    partial class OzAIMultiHeadAttn
    {
        public OzAICompIOMem_Unary Mem;
        public CompIParams IParams;
        public class CompIParams : OzAICompIParams
        {
            public OzAIAttnGroup.CompIParams[] GroupParams;
            public OzAIMatrix OutputWeights;

            public override bool IsPossible(out string error)
            {
                List<object> objs = [ExecManager, GroupParams, OutputWeights];
                List<string> names = ["ExecManager", "GroupParams", "Outputs"];
                if (!CheckIfNull(objs, names, out error))
                    return false;

                for (int i = 0; i < GroupParams.Length; i++)
                {
                    var group = GroupParams[i];
                    if (group == null)
                    {
                        error = $"No group parameters provided for group number {i}.";
                        return false;
                    }
                    if (!group.IsPossible(out error))
                    {
                        error = $"No group parameters number {i} not possible: " + error;
                        return false;
                    }
                }

                return true;
            }
        }

        public CompHParams HParams;
        public class CompHParams : OzAICompHParams
        {
            public uint GroupCount;
            public OzAIAttnGroup.CompHParams GroupParams;

            public override bool SetDefaults(OzAIProcMode mode, out string error)
            {
                return GroupParams.SetDefaults(mode, out error);
            }

            public override bool IsPossible(out string error)
            {
                if (GroupCount == 0)
                {
                    error = "Group count needs to be non zero";
                    return false;
                }
                if (!CheckIfNull(GroupParams, "GroupParams", out error)) return false;
                if (!GroupParams.IsPossible(out error)) return false;
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
                error = "Instance parameters required to be in a format for a OzAIMultiHeadAttn.CompIParams.";
                return false;
            }
            if (Params.HParams is not CompHParams)
            {
                error = "Hyperparameters required to be in a format for a OzAIMultiHeadAttn.CompHParams.";
                return false;
            }

            Mem = Params.Mem as OzAICompIOMem_Unary;
            IParams = Params.IParams as CompIParams;
            HParams = Params.HParams as CompHParams;

            if (IParams.GroupParams.Length != HParams.GroupCount)
            {
                error = "Number of group parameters provided does not match the group count hparam.";
                return false;
            }

            error = null;
            return true;
        }
    }
}
