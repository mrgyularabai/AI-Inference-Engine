using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public partial class OzAIUnembedding : OzAIArchComp
    {
        public override string Name => "OzAIUnembedding";

        public override bool Forward(out string error)
        {
            var exec = Params.IParams.ExecManager;
            if (!exec.GetProcMode(out var mode, out error))
                return false;

            var inps = Mem.Inputs.GetArray();
            var last = inps.Last();
            if (!OzAIVectorRange.Create(mode, HParams.VocabSize, out var scores, out error))
                return false;

            var inputs = new OzAIVector[HParams.VocabSize];
            for (ulong i = 0; i < HParams.VocabSize; i++)
            {
                inputs[i] = last;
            }

            if (!exec.Dot(inputs, IParams.Embeddings, scores, out error))
                return false;

            if (!OzAIScalar.CreateInt(0, mode, out var idx, out error))
                return false;

            if (!exec.Max(scores, idx, out error))
                return false;

            Mem.Output = (OzAIIntVec)idx.Vector;

            error = null;
            return true;
        }
    }
}
