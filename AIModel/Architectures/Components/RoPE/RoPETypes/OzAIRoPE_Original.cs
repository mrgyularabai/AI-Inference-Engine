using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    /// <summary>
    /// This type of RoPE is based on the original paper that proposed taking the consecutive elements as pairs for 2D rotation of theta * position radians where theta = Base^(di/2): d is the number of pairs of elements and i is the index of the pair of elements. <br/>
    /// </summary>
    public class OzAIRoPE_Original : OzAIRoPE
    {
        OzAIMemNode _f32Vecs;

        public override string Name => "OzAIRoPE_Original";

        protected override bool InitInner(out string error)
        {
            _f32Vecs = new OzAIMemNode();
            error = null;
            return true;
        }

        public override bool Forward(out string error)
        {
            var exec = Params.IParams.ExecManager;
            if (!exec.GetProcMode(out var mode, out error))
                return true;
            if (!_f32Vecs.Clone(Mem.Inputs, out error))
                return false;
            var mem = _f32Vecs.GetArray();
            for (long i = 0; i < mem.LongLength; i++)
            {
                if (!mem[i].ToDType(OzAINumType.Float32, out mem[i], out error))
                    return false;
            }
            if (!exec.RoPE(mem, HParams.ThetaBase, mem, out error))
                return false;
            var outputs = Mem.Outputs.GetList();
            var outDtype = outputs[0].GetNumType();
            for (ulong i = 0; i < Mem.Outputs.Count; i++)
            {
                if (!mem[i].ToDType(outDtype, out var l, out error))
                    return false;
                outputs[(int)i] = l;
            }
            error = null;
            return true;
        }

        public override bool IsPossible(out string error)
        {
            if (Params.Mem is not OzAICompIOMem_Unary)
            {
                error = "IO memory required to be in a format for a unary op OzAICompIOMem_Unary.";
                return false;
            }
            if (Params.HParams is not CompHParams)
            {
                error = "Hyperparameters required to be in a format for a OzAIRoPE.CompHParams.";
                return false;
            }

            Mem = Params.Mem as OzAICompIOMem_Unary;
            HParams = Params.HParams as CompHParams;

            error = null;
            return true;
        }
    }
}
