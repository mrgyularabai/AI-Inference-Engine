using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public abstract class OzAIArch
    {

        public OzAIProcMode Mode;
        public OzAIExecManager ExecManager;

        public string Name
        {
            get
            {
                return GetName();
            }
        }
        protected abstract string GetName();

        public OzAIModelSize Size;
        public OzAIVector IN;
        public OzAIVector OUT;

        public abstract bool InitFromFile(OzGGUFFile file, out string error);

        protected abstract bool ArchInitialize(OzGGUFFile file, out string error);

        public static bool CreateFromFile(OzGGUFFile file, out OzAIArch res, out string error)
        {
            switch (file.Architecture)
            {
                case "llama":
                    res = new OzAIArch_LLaMA();
                    break;
                //case "Falcon": 
                //    arch = new OzGGUFArch_Falcon();
                //    break;
                default:
                    error = $"Unknown AI Architecture: {file.Architecture}";
                    res = null;
                    return false;
            }

            if (!res.InitFromFile(file, out error)) return false;

            error = null;
            return true;
        }

        public abstract bool Forward(out string error);
    }
}
