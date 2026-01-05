using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzGGUF_Tensor : OzGGUF_Item
    {
        public OzGGUF_String Name;
        public OzGGUF_UInt32 DimCount; // Number of Dimensions
        public List<OzGGUF_UInt64> ElementCounts; // Size of Each Dimension
        public OzGGUF_NumType Type; // Float Precision Level/Quantization Type of the Data
        public OzGGUF_UInt64 DataOffset; // Data Location in File

        ulong _byteCount;
        public ulong ByteCount
        {
            get {
                if (_byteCount>0) return _byteCount;
                _byteCount = GetTensorByteCount();
                return _byteCount;
            }
            set { 
                _byteCount= value;
            }
        }

        public ulong PositionInFile;
        public ulong PositionInMemory;
        public ulong PaddingAfter;

        public byte[] Data;

        public override bool Parse(Stream input, out string error)
        {
            Name = new OzGGUF_String();
            DimCount = new OzGGUF_UInt32();

            if (!Name.Parse(input, out error)) return false;
            if (!DimCount.Parse(input, out error)) return false;

            ElementCounts = new List<OzGGUF_UInt64>();

            for (uint i = 0; i < DimCount.Value; i++)
            {
                ElementCounts.Add(new OzGGUF_UInt64());
                ElementCounts[(int)i].Parse(input, out error);
            }

            Type = new OzGGUF_NumType();
            DataOffset = new OzGGUF_UInt64();

            if (!Type.Parse(input, out error)) return false;
            if (!DataOffset.Parse(input, out error)) return false;
            return true;
        }

        public ulong GetTensorByteCount()
        {
            ulong val = GetNumCount();
            var type = Type.ToAINumType();
            var inst = type.Create();
            val = inst.CalcSize(val);
            return val;
        }

        public ulong GetNumCount()
        {
            ulong val = 1;
            for (int j = 0; j < DimCount.Value; j++)
            {
                val *= ElementCounts[j].Value;
            }
            return val;
        }

        public override string ToString()
        {
            var res = Name.Value + ": "+ Type.ToString()+ "[";
            for (uint i = 0; i < DimCount.Value; i++)
            {
                res += ElementCounts[(int)i].Value + "x";
            }
            return res.Substring(0, res.Length - 1) + "]";
        }
    }
}
