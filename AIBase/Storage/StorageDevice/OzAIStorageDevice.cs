using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public abstract class OzAIStorageDevice
    {
        public abstract int Allocate(nuint size);
        public abstract OzAIDataStorage GetStorage(int ID);
        public abstract void Free(int ID);
    }
}
