using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public abstract class OzAIFloatMat : OzAIMatrix
    {
        OzAIProcMode _procMode;
        public override bool GetProcMode(out OzAIProcMode mode, out string error)
        {
            if (_procMode == null)
            {
                error = "Could not get processing mode, because OzAIFloatMat not created with 'Create'.";
                mode = null;
                return false;
            }

            mode = _procMode;
            error = null;
            return true;
        }

        public override OzAINumType GetNumType()
        {
            return OzAINumType.Float32;
        }

        public static bool Create(OzAIProcMode mode, out OzAIFloatMat res, out string error)
        {
            res = null;

            if (!checkProcModeSupport(mode, out error))
                return false;

            res = new OzAIFloatMat_CSharp();
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
                error = "Could not create OzAIHalfMat, because GPU support not implemented";
                return false;
            }

            if (!mode.GetCPUSettings(out var settings, out error)) return false;
            if (settings.UseAVX)
            {
                error = "Could not create OzAIHalfMat, because AVX support not implemented";
                return false;
            }

            error = null;
            return true;
        }
    }
}
