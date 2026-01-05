using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public abstract class OzAIDataStorage
    {
        public nint Addr { get; protected set; }
        public nuint Size { get; protected set; }

        public void Allocate()
        {
            if (Addr != 0) return;
            Addr = InnerAllocate();
        }
        protected abstract nint InnerAllocate();

        public void Free()
        {
            if (Addr == 0) return;
            InnerFree();
            Addr = 0;
        }
        protected abstract void InnerFree();
    }
}
