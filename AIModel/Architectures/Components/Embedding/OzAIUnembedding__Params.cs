using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    partial class OzAIUnembedding
    {
        public CompMem Mem;
        public class CompMem : OzAICompIOMem
        {
            public OzAIMemNode Inputs;
            public OzAIIntVec Output;
            public override bool IsPossible(out string error)
            {
                List<object> objs = [Inputs, Output];
                List<string> names = ["Inputs", "Output"];
                return CheckIfNull(objs, names, out error);
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

        public CompHParams HParams;
        public class CompHParams : OzAICompHParams
        {
            public ulong VocabSize;

            public override bool IsPossible(out string error)
            {
                if (VocabSize == 0)
                {
                    error = "Cannot compute unembeddings with a vocab size of 0.";
                    return false;
                }
                error = null;
                return true;
            }
        }

        public override bool IsPossible(out string error)
        {
            if (Params.Mem is not OzAICompIOMem)
            {
                error = "IO memory required to be in a format for a OzAIUnembedding.OzAICompIOMem.";
                return false;
            }
            if (Params.IParams is not CompIParams)
            {
                error = "Instance parameters required to be in a format for a OzAIUnembedding.CompIParams.";
                return false;
            }
            if (Params.HParams is not CompHParams)
            {
                error = "Hyperparameters required to be in a format for a OzAIUnembedding.CompHParams.";
                return false;
            }

            Mem = Params.Mem as CompMem;
            IParams = Params.IParams as CompIParams;
            HParams = Params.HParams as CompHParams;

            if (HParams.VocabSize != (ulong)IParams.Embeddings.LongLength)
            {
                error = "Vocab size differs from the number of embeddings provided.";
                return false;
            }

            error = null;
            return true;
        }
    }
}
