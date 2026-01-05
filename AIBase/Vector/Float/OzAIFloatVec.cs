using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public abstract class OzAIFloatVec : OzAIVector
    {
        public override OzAINumType GetNumType()
        {
            return OzAINumType.Float32;
        }

        OzAIProcMode _procMode;
        public override bool GetProcMode(out OzAIProcMode mode, out string error)
        {
            if (_procMode == null)
            {
                error = "Could not get processing mode, because OzAIFloatVec not created with 'Create'.";
                mode = null;
                return false;
            }

            mode = _procMode;
            error = null;
            return true;
        }

        public static bool Create(OzAIProcMode mode, out OzAIFloatVec res, out string error)
        {
            res = null;

            if (!checkProcModeSupport(mode, out error))
                return false;

            res = new OzAIFloatVec_CSharp();
            res._procMode = mode;
            return true;
        }

        static bool checkProcModeSupport(OzAIProcMode mode, out string error)
        {
            if (mode == null)
            {
                error = "Could not create OzAIHalfMat, because no processing mode provided.";
                return false;
            }

            if (!mode.IsCPUOnly(out var cpuOnly, out error)) return false;
            if (!cpuOnly)
            {
                error = "GPU support not implemented";
                return false;
            }

            if (!mode.GetCPUSettings(out var settings, out error)) return false;
            if (settings.UseAVX)
            {
                error = "AVX support not implemented";
                return false;
            }

            error = null;
            return true;
        }
    }
}
