using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAIRoPE_Linear : OzAIRoPE_Scaling
    {
        public override string Name => throw new NotImplementedException();

        public class Params : ScalingParams
        {
            public override bool IsPossible(out string error)
            {
                throw new NotImplementedException();
            }

            protected override bool ScalingTypeInit(OzGGUFFile file, out string error)
            {
                error = null;
                return true;
            }
        }

        public override bool Forward(out string error)
        {
            error = null;
            return true;
        }

        public override ScalingParams GetScalingParams()
        {
            return new Params();
        }

        protected override bool InitInner(out string error)
        {
            throw new NotImplementedException();
        }

        public override bool IsPossible(out string error)
        {
            throw new NotImplementedException();
        }
    }
}
