using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    /// <summary>
    /// This class contains all the parameters that an architecture component needs to operate
    /// </summary>
    public class OzAICompParams : OzAICheckable
    {
        public OzAICompIOMem Mem;
        public OzAICompIParams IParams;
        public OzAICompHParams HParams;

        public bool SetDefaults(OzAIProcMode mode, out string error)
        {
            if (mode.DoChecks)
            {
                List<object> objs = [IParams, HParams];
                List<string> names = ["instance parameters", "hyperparameters"];
                if (!CheckIfNull(objs, names, out error))
                    return false;
            }

            if (!IParams.SetDefaults(mode, out error)) return false;
            if (!HParams.SetDefaults(mode, out error)) return false;

            error = null;
            return true;
        }

        public bool InitFromFile(OzAIProcMode mode, OzGGUFFile file, out string error)
        {
            if (mode.DoChecks)
            {
                List<object> objs = [IParams, HParams];
                List<string> names = ["instance parameters", "hyperparameters"];
                if (!CheckIfNull(objs, names, out error))
                    return false;
            }

            if (!IParams.InitFromFile(mode, file, out error)) return false;
            if (!HParams.InitFromFile(mode, file, out error)) return false;
            return false;
        }

        public override bool IsPossible(out string error)
        {
            List<object> objs = [Mem, IParams];
            List<string> names = ["IO memory", "instance parameters"];
            if (!CheckIfNull(objs, names, out error))
                return false;
            if (!Mem.IsPossible(out error)) return false;
            if (!IParams.IsPossible(out error)) return false;
            if (HParams != null)
            {
                if (!HParams.IsPossible(out error))
                    return false;
            }

            return true;
        }
    }
}
