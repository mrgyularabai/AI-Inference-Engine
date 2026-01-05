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
    public abstract class OzAICompIParams : OzAICheckable
    {

        public OzAIExecManager ExecManager;
        public virtual bool SetDefaults(OzAIProcMode mode, out string error)
        {
            error = null;
            return true;
        }

        public bool CheckForExec(out string error)
        {
            if (ExecManager == null)
            {
                error = "No exec manager provided";
                return false;
            }
            error = null;
            return true;
        }

        public virtual bool InitFromFile(OzAIProcMode mode, OzGGUFFile file, out string error)
        {
            error = null;
            return false;
        }
    }
}
