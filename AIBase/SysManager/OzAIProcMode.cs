using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public partial class OzAIProcMode
    {
        public bool DoChecks = true;
        public static bool GetDefaults(out OzAIProcMode res, out string error)
        {
            res = new OzAIProcMode();
            if (!OzAICPUSettings.GetDefault(out var cpu, out error))
                return false;
            if (!res.Initialize(cpu, out error))
                return false;
            return true;
        }

        public bool Initialize(OzAICPUSettings cpuSettings, out string error)
        {
            if (cpuSettings == null)
            {
                error = "Could not initialize OzAIProcMode, because no cpuSettings Provided";
                return false;
            }
            _cpuSetting = cpuSettings;
            if (!_cpuSetting.IsInitialized(out error))
            {
                error = "Could not initialize OzAIProcMode: " + error;
                return false;
            }
            Initialized = true;
            error = null;
            return true;
        }

        public bool Initialized {  get; set; }
        public bool IsCPUOnly(out bool res, out string error)
        {
            if (!Initialized)
            {
                res = false;
                error = "Processing mode not initialized yet.";
            }
            error = null;
            res = true;
            return res;
        }

        OzAICPUSettings _cpuSetting;
        public bool GetCPUSettings(out OzAICPUSettings res, out string error)
        {
            if (!Initialized)
            {
                res = null;
                error = "Could not get CPU settings, because processing mode not initialized yet.";
            }
            res = _cpuSetting;
            error = null;
            return true;
        }
    }
}
