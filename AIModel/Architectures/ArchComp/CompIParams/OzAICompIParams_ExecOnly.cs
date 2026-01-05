using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    /// <summary>
    /// These are the 'instance parameters' for a component like the gain weights for RMS, where a different value is needed for each instance of the architecture component.
    /// </summary>
    public class OzAICompIParams_ExecOnly : OzAICompIParams
    {
        public override bool IsPossible(out string error)
        {
            return CheckIfNull(ExecManager, "execution manager", out error);
        }
    }
}
