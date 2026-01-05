using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ozeki
{
    /// <summary>
    /// Applies self attention to a some number of vectors a lot of times in different heads.
    /// </summary>
    public partial class OzAIMultiHeadAttn : OzAIArchComp
    {
        List<OzAIAttnGroup> Groups;

        public override string Name => "OzAIMultiHeadAttn";

        protected override bool InitInner(out string error)
        {
            Groups = new List<OzAIAttnGroup>();

            for (int i = 0; i < HParams.GroupCount; i++)
            {
                if (!createGroup(i, out error))
                    return false;
            }

            error = null;
            return true;
        }

        bool createGroup(int i, out string error)
        {
            var group = new OzAIAttnGroup();

            var groupMem = createGroupMem(i);
            var groupParams = new OzAICompParams()
            {
                Mem = groupMem,
                IParams = IParams.GroupParams[i],
                HParams = HParams.GroupParams,
            };

            if (!group.Init(groupParams, out error))
                return false;

            Groups.Add(group);
            return true;
        }

        OzAIAttnGroup.CompMem createGroupMem(int i)
        {
            var groupOuts = new OzAIMemNode[HParams.GroupParams.HeadCount];
            for (long j = 0; j < groupOuts.LongLength; j++)
            {
                groupOuts[j] = new OzAIMemNode();
            }
            return new OzAIAttnGroup.CompMem()
            {
                Inputs = Mem.Inputs,
                Outputs = groupOuts
            };
        }


        public override bool Forward(out string error)
        {
            var exec = IParams.ExecManager;
            if (!exec.GetProcMode(out var mode, out error))
                return false;

            for (int i = 0; i < Groups.Count; i++)
            {
                var group = Groups[i];
                for (long j = 0; j < group.Mem.Outputs.LongLength; j++)
                {
                    group.Mem.Outputs[j].Clear();
                    if (!group.Mem.Outputs[j].AddVecs(mode, HParams.GroupParams.HeadParams.ValLen, Mem.Inputs.Count, out error))
                        return false;
                }
                if (!group.Forward(out error))
                    return false;
            }

            if (!mergeGroupOuts(mode, out var res, out error))
                return false;

            var vecs = res.ToArray();

            var outs = Mem.Outputs.GetArray();
            if (!exec.MatMul(vecs, IParams.OutputWeights, outs, out error))
                return false;

            error = null;
            return true;
        }

        bool mergeGroupOuts(OzAIProcMode mode, out List<OzAIVector> res, out string error)
        {
            res = new List<OzAIVector>();
            var valLen = HParams.GroupParams.HeadParams.ValLen;
            var vecLen = valLen * HParams.GroupParams.HeadCount * HParams.GroupCount;

            var count = Mem.Inputs.GetList().Count;
            for (int i = 0; i < count; i++)
            {
                if (!CombineVec(i, (int)valLen, vecLen, mode, res, out error))
                    return false;
            }

            error = null;
            return true;
        }

        bool CombineVec(int i, int valLen, ulong vecLen, OzAIProcMode mode, List<OzAIVector> res, out string error)
        {
            if (!OzAIVector.Create(mode, out var resVec, out error))
                return false;
            if (!resVec.GetBytesPerBlock(out var bpb, out error))
                return false;
            if (!resVec.GetNumsPerBlock(out var npb, out error))
                return false;
            var size = (vecLen / npb) * bpb;
            var vals = new byte[size];
            var valSize = (valLen / (int)npb) * (int)bpb;
            var offset = 0;
            for (int j = 0; j < Groups.Count; j++)
            {
                for (int k = 0; k < Groups[j].Mem.Outputs.Length; k++)
                {
                    if (!Groups[j].Mem.Outputs[k].GetList()[i].ToBytes(out var headFloats, out error))
                        return false;
                    Buffer.BlockCopy(headFloats, 0, vals, offset, valSize);
                    offset += valSize;
                }
            }
            if (!resVec.Init(vals, 0, size, out error))
                return false;
            res.Add(resVec);
            return true;
        }
    }
}
