using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    partial class OzAIEmbedding
    {
        public CompMem Mem;
        public class CompMem : OzAICompIOMem
        {
            public OzAIIntVec Input;
            public OzAIMemNode Outputs;
            public override bool IsPossible(out string error)
            {
                return CheckIfNull(Outputs, "Outputs", out error);
            }
        }

        public CompIParams IParams;
        public class CompIParams : OzAICompIParams
        {
            public OzAIVector[] Embeddings;

            public override bool IsPossible(out string error)
            {
                if (!CheckForExec(out error)) return false;
                return CheckIfNull(Embeddings, "Embeddings", out error);
            }
        }

        public override bool IsPossible(out string error)
        {
            if (Params.Mem is not OzAICompIOMem)
            {
                error = "IO memory required to be in a format for a unary op OzAIEmbedding.OzAICompIOMem.";
                return false;
            }
            if (Params.IParams is not CompIParams)
            {
                error = "Instance parameters required to be in a format for a OzAIEmbedding.CompIParams.";
                return false;
            }

            Mem = Params.Mem as CompMem;
            IParams = Params.IParams as CompIParams;

            error = null;
            return true;
        }
    }
}
