using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    /// <summary>
    /// These are the 'hyperparameters' for a component like the epsilon value for RMS, where there is only one value for it in a model, but it applies to all the instances of the architecture component.
    /// </summary>
    public abstract class OzAICompHParams : OzAICheckable
    {
        public virtual bool SetDefaults(OzAIProcMode mode, out string error)
        {
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
