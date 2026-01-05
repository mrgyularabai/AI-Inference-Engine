using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAIRAM : OzAIStorageDevice
    {
        Dictionary<int, OzAIRAMDStorage> storages = new Dictionary<int, OzAIRAMDStorage>();

        public override int Allocate(nuint size)
        {
            var storage = new OzAIRAMDStorage(size);
            var res = storages.Count;
            storages.Add(res, storage);
            return res;
        }

        public override void Free(int ID)
        {
            storages[ID].Free();
            storages.Remove(ID);
        }

        public override OzAIDataStorage GetStorage(int ID)
        {
            return storages[ID];
        }
    }
}
