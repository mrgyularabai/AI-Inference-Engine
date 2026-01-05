using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{

    public abstract class OzAIArchComp : OzAICheckable
    {
        public OzAICompParams Params;
        public abstract string Name { get; }

        public bool Init(OzAICompParams args, out string error)
        {

            Params = args;
            if (!getProcModeSafe(args, out var mode, out error)) return false;
            if (!mode.DoChecks)
                return InitInner(out error);

            if (!args.IsPossible(out error))
            {
                error = $"Failed to init {Name}, becuase invalid component parameters provided: " + error;
                return false;
            }
            if (!IsPossible(out error))
            {
                error = $"Failed to init {Name}, becuase the execution of this component would not be possible: " + error;
                return false;
            }

            return InitInner(out error);
        }

        bool getProcModeSafe(OzAICompParams args, out OzAIProcMode mode, out string error)
        {
            mode = null;
            if (args == null)
            {
                error = $"Could not init {Name}, becuase no OzAICompParams provided.";
                return false;
            }
            if (args.IParams == null)
            {
                error = $"Could not init {Name}, becuase no instance params provided in OzAICompParams.";
                return false;
            }
            if (args.IParams.ExecManager == null)
            {
                error = $"Could not init {Name}, becuase no exec manager provided in instance params provided in OzAICompParams.";
                return false;
            }
            if (!args.IParams.ExecManager.GetProcMode(out mode, out error))
            {
                error = $"Could not init {Name}: " + error;
                return false;
            }
            error = null;
            return true;
        }

        protected virtual bool InitInner(out string error)
        {
            error = null;
            return true;
        }

        public abstract bool Forward(out string error);

    }
}
