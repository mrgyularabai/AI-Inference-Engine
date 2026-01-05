using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Ozeki
{
    public partial class OzAIEmbedding : OzAIArchComp
    {
        public override string Name => "OzAIEmbedding";

        public override bool Forward(out string error)
        {
            var idxs = Mem.Input;
            if (!idxs.GetNumCount(out var len, out error))
                return false;

            var res = new OzAIVector[len];

            for (ulong i = 0; i < len; i++)
            {
                if (!idxs.GetNthInt(i, out var idx, out error))
                    return false;
                var resVec = IParams.Embeddings[idx];
                if (!resVec.Clone(out res[i], out error))
                    return false;
            }

            Mem.Outputs.Clear();
            Mem.Outputs.Add(res);

            error = null;
            return true;
        }
    }
}
