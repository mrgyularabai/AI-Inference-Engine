using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public partial class OzAIRMSNorm : OzAIArchComp
    {
        public override string Name => "OzAIRMSNorm";

        OzAIMemNode _f32Vecs;

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
            if (!exec.RMS(HParams.Epsilon, IParams.Part, mem, mem, out error))
                return false;
            if (!exec.Had(mem, IParams.Gain, mem, out error))
                return false;
            var outs = Mem.Outputs.GetList();
            var outDtype = outs[0].GetNumType();
            
            for (int i = 0; i < (int)Mem.Outputs.Count; i++)
            {
                if (!mem[i].ToDType(outDtype, out var l, out error))
                    return false;
                outs[i] = l;
            }
            error = null;
            return true;
        }
    }
}
