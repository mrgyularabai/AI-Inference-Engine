using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public enum OzAIRoPE_ScalingType
    {
        None = 0,
        Linear = 1,
        Yarn = 2
    }

    public static class OzAIRoPE_ScalingTypeExt
    {
        public static OzAIRoPE_Scaling Create(this OzAIRoPE_ScalingType self)
        {
            switch (self)
            {
                case OzAIRoPE_ScalingType.None:
                    return null;
                case OzAIRoPE_ScalingType.Linear:
                    return new OzAIRoPE_Linear();
                case OzAIRoPE_ScalingType.Yarn:
                    return new OzAIRoPE_Yarn();
            }
            return null;
        }

        public static OzAIRoPE_Scaling.ScalingParams CreateParams(this OzAIRoPE_ScalingType self)
        {
            return self.Create().GetScalingParams();
        }
    }
}
