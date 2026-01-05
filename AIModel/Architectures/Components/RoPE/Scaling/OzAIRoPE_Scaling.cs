using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public abstract class OzAIRoPE_Scaling : OzAIArchComp
    {
        public abstract class ScalingParams : OzAICompParams
        {
            public bool Finetuned; // Finetuned for the Scaling of RoPE for different ctx lengths
            public uint OriginalCtxLen; // Trained with this orignal ctx length before applying RoPE scaling
            public float FreqScaleFactor; //Inv of Scaling Factor
            public float AttnScaleFactor;

            public static bool CreateFromFile(OzGGUFFile file, out ScalingParams res, out OzAIRoPE_ScalingType type, out string error)
            {
                res = null;
                if (!getScalingType(file, out type, out error))
                    return false;

                res = type.CreateParams();

                if (!file.GetMDBool($"{file.Architecture}.rope.scaling.finetuned", out res.Finetuned, out error, false, false) && error != null)
                    return false;

                if (!file.GetMDUInt32($"{file.Architecture}.context_length", out var ctxLen, out error))
                    return false;
                if (!file.GetMDUInt32($"{file.Architecture}.rope.scaling.original_context_length", out res.OriginalCtxLen, out error, false, ctxLen) && error != null)
                    return false;

                if (!getScaleFactor(file, out res.FreqScaleFactor, out error))
                    return false;

                if (!file.GetMDFloat32($"{file.Architecture}.rope.scaling.attn_factor", out res.AttnScaleFactor, out error, false, 0.0f) && error != null)
                    return false;

                if(!res.ScalingTypeInit(file, out error))
                    return false;

                return true;
            }

            static bool getScaleFactor(OzGGUFFile file, out float res, out string error)
            {
                res = float.NaN;

                float scaleFactor;
                if (file.GetMDFloat32($"{file.Architecture}.rope.scaling.factor", out scaleFactor, out error, false))
                {
                    if (error != null) return false;
                    // try the old key name
                    if (!file.GetMDFloat32($"{file.Architecture}.rope.scale_linear", out scaleFactor, out error, false, 0.0f) && error != null)
                        return false;
                }

                res = scaleFactor == 0.0f ? 1.0f : 1.0f / scaleFactor;

                return true;
            }

            static bool getScalingType(OzGGUFFile file, out OzAIRoPE_ScalingType res, out string error)
            {
                res = OzAIRoPE_ScalingType.Linear;

                string scaling;
                if (!file.GetMDString($"{file.Architecture}.rope.scaling.type", out scaling, out error, false, "linear") && error != null)
                    return false;

                switch (scaling)
                {
                    case "none":
                        res = OzAIRoPE_ScalingType.None;
                        error = "RoPE scaling type of 'none' not implemented. (need to set a fixed context length for that case)";
                        return false;
                    case "linear":
                        res = OzAIRoPE_ScalingType.Linear;
                        break;
                    case "yarn":
                        res = OzAIRoPE_ScalingType.Yarn;
                        break;
                }

                return true;
            }

            protected abstract bool ScalingTypeInit(OzGGUFFile file, out string error);
        }

        public abstract ScalingParams GetScalingParams();
    }
}
