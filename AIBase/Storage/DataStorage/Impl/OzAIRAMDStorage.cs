using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAIRAMDStorage : OzAIDataStorage
    {
        public OzAIRAMDStorage(nuint size)
        {
            Size = size;
        }

        protected override nint InnerAllocate()
        {
            unsafe
            {
                return (nint)NativeMemory.AlignedAlloc(Size, OzAIMemManager.AlignmentBytes);
            }
        }

        protected override void InnerFree()
        {
            unsafe
            {
                NativeMemory.AlignedFree((void*)Addr);
            }
        }
    }
}
