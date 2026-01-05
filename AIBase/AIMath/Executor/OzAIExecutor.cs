using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public abstract class OzAIExecutor
    {
        public abstract void Add(OzAIOperation operation);
        public abstract bool Start(OzAIProcMode mode, out string error);
        public abstract bool AwaitAll(out string error);
        public abstract void Stop();
    }
}
