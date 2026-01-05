//https://hexed.it/
//https://github.com/ggerganov/ggml/blob/master/docs/gguf.md

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static Ozeki.OzGPC_UL;

namespace Ozeki
{
    public partial class OzGGUFFile
    {
        public bool GetMD(string name, out OzGGUF_Item res, out string error, bool required = true)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(name);
                if (!MDs.ContainsKey(bytes))
                {
                    res = null;
                    error = required ? $"No metadata entry called {name} found." : null;
                    return false;
                }
                res = MDs[bytes].MDValue;
            }
            catch (Exception e)
            {
                res = null;
                error = $"There was an error whilst fetching the metadata: {name}; " + e.Message;
                return false;
            }

            error = null;
            return true;
        }

        // String
        public bool GetMDString(string name, out string res, out string error, bool required = true, string defaultVal = null)
        {
            OzGGUF_Item mdValue;
            if (!GetMD(name, out mdValue, out error, required))
            {
                res = defaultVal;
                return false;
            }
            try
            {
                var item = mdValue as OzGGUF_String;
                res = item.Value;
            }
            catch (Exception ex)
            {
                res = defaultVal;
                error = $"Error occured whilst getting the metadata entry called {name}: " + ex.Message;
                return false;
            }
            return true;
        }

        // Ints
        public bool GetMDUInt8(string name, out byte res, out string error, bool required = true, byte defaultVal = byte.MaxValue)
        {
            OzGGUF_Item mdValue;
            if (!GetMD(name, out mdValue, out error, required))
            {
                res = defaultVal;
                return false;
            }
            try
            {
                var item = mdValue as OzGGUF_UInt8;
                res = item.Value;
            }
            catch (Exception ex)
            {
                res = defaultVal;
                error = $"Error occured whilst getting the metadata entry called {name}: " + ex.Message;
                return false;
            }
            return true;
        }

        public bool GetMDUInt16(string name, out ushort res, out string error, bool required = true, ushort defaultVal = ushort.MaxValue)
        {
            OzGGUF_Item mdValue;
            if (!GetMD(name, out mdValue, out error, required))
            {
                res = defaultVal;
                return false;
            }
            try
            {
                var item = mdValue as OzGGUF_UInt16;
                res = item.Value;
            }
            catch (Exception ex)
            {
                res = defaultVal;
                error = $"Error occured whilst getting the metadata entry called {name}: " + ex.Message;
                return false;
            }
            return true;
        }

        public bool GetMDUInt32(string name, out uint res, out string error, bool required = true, uint defaultVal = uint.MaxValue)
        {
            OzGGUF_Item mdValue;
            if (!GetMD(name, out mdValue, out error, required))
            {
                res = defaultVal;
                return false;
            }
            try
            {
                var item = mdValue as OzGGUF_UInt32;
                res = item.Value;
            }
            catch (Exception ex)
            {
                res = defaultVal;
                error = $"Error occured whilst getting the metadata entry called {name}: " + ex.Message;
                return false;
            }
            return true;
        }

        public bool GetMDUInt64(string name, out ulong res, out string error, bool required = true, ulong defaultVal = ulong.MaxValue)
        {
            OzGGUF_Item mdValue;
            if (!GetMD(name, out mdValue, out error, required))
            {
                res = defaultVal;
                return false;
            }
            try
            {
                var item = mdValue as OzGGUF_UInt64;
                res = item.Value;
            }
            catch (Exception ex)
            {
                res = defaultVal;
                error = $"Error occured whilst getting the metadata entry called {name}: " + ex.Message;
                return false;
            }
            return true;
        }

        public bool GetMDInt8(string name, out sbyte res, out string error, bool required = true, sbyte defaultVal = -1)
        {
            OzGGUF_Item mdValue;
            if (!GetMD(name, out mdValue, out error, required))
            {
                res = defaultVal;
                return false;
            }
            try
            {
                var item = mdValue as OzGGUF_Int8;
                res = item.Value;
            }
            catch (Exception ex)
            {
                res = defaultVal;
                error = $"Error occured whilst getting the metadata entry called {name}: " + ex.Message;
                return false;
            }
            return true;
        }

        public bool GetMDInt16(string name, out short res, out string error, bool required = true, short defaultVal = -1)
        {
            OzGGUF_Item mdValue;
            if (!GetMD(name, out mdValue, out error, required))
            {
                res = defaultVal;
                return false;
            }
            try
            {
                var item = mdValue as OzGGUF_Int16;
                res = item.Value;
            }
            catch (Exception ex)
            {
                res = defaultVal;
                error = $"Error occured whilst getting the metadata entry called {name}: " + ex.Message;
                return false;
            }
            return true;
        }

        public bool GetMDInt32(string name, out int res, out string error, bool required = true, int defaultVal = -1)
        {
            OzGGUF_Item mdValue;
            if (!GetMD(name, out mdValue, out error, required))
            {
                res = defaultVal;
                return false;
            }
            try
            {
                var item = mdValue as OzGGUF_Int32;
                res = item.Value;
            }
            catch (Exception ex)
            {
                res = defaultVal;
                error = $"Error occured whilst getting the metadata entry called {name}: " + ex.Message;
                return false;
            }
            return true;
        }

        public bool GetMDInt64(string name, out long res, out string error, bool required = true, long defaultVal = -1)
        {
            OzGGUF_Item mdValue;
            if (!GetMD(name, out mdValue, out error, required))
            {
                res = defaultVal;
                return false;
            }
            try
            {
                var item = mdValue as OzGGUF_Int64;
                res = item.Value;
            }
            catch (Exception ex)
            {
                res = defaultVal;
                error = $"Error occured whilst getting the metadata entry called {name}: " + ex.Message;
                return false;
            }
            return true;
        }

        // Floats
        public bool GetMDFloat32(string name, out float res, out string error, bool required = true, float defaultVal = float.NaN)
        {
            OzGGUF_Item mdValue;
            if (!GetMD(name, out mdValue, out error, required))
            {
                res = defaultVal;
                return false;
            }
            try
            {
                var item = mdValue as OzGGUF_Float32;
                res = item.Value;
            }
            catch (Exception ex)
            {
                res = defaultVal;
                error = $"Error occured whilst getting the metadata entry called {name}: " + ex.Message;
                return false;
            }
            return true;
        }

        public bool GetMDFloat64(string name, out double res, out string error, bool required = true, double defaultVal = double.NaN)
        {
            OzGGUF_Item mdValue;
            if (!GetMD(name, out mdValue, out error, required))
            {
                res = defaultVal;
                return false;
            }
            try
            {
                var item = mdValue as OzGGUF_Float64;
                res = item.Value;
            }
            catch (Exception ex)
            {
                res = defaultVal;
                error = $"Error occured whilst getting the metadata entry called {name}: " + ex.Message;
                return false;
            }
            return true;
        }

        // Bool
        public bool GetMDBool(string name, out bool res, out string error, bool required = true, bool defaultVal = false)
        {
            OzGGUF_Item mdValue;
            if (!GetMD(name, out mdValue, out error, required))
            {
                res = defaultVal;
                return false;
            }
            try
            {
                var item = mdValue as OzGGUF_Bool;
                res = item.Value;
            }
            catch (Exception ex)
            {
                res = defaultVal;
                error = $"Error occured whilst getting the metadata entry called {name}: " + ex.Message;
                return false;
            }
            return true;
        }

        public bool GetTensor(string name, out OzGGUF_Tensor res, out string error)
        {
            if (!TensorNamesToID.ContainsKey(name))
            {
                error = $"Tensor with the specified name, '{name}', not found.";
                res = null;
                return false;
            }
            var idx = TensorNamesToID[name];
            res = Tensors[idx];
            error = null;
            return true;
        }

        public bool GetVector(string name, OzAIProcMode mode, out OzAIVector res, out string error, bool cast2Default = true)
        {
            if (!GetTensor(name, out var tens, out error))
            {
                error = $"Vector with the specified name, '{name}', not found.";
                res = null;
                return false;
            }
            var numType = tens.Type.ToAINumType();
            var inst = numType.Create();
            if (!numType.CreateVec(mode, out res, out error))
                return false;
            var len = tens.GetNumCount();
            if (!res.Init(tens.Data, 0, len, out error))
                return false;
            if (!mode.GetCPUSettings(out var cpu, out error))
                return false;
            if (cast2Default && numType != cpu.DefaultProcType)
            {
                if (!res.ToDType(cpu.DefaultProcType, out res, out error))
                    return false;
            }
            error = null;
            return true;
        }

        public bool GetVectors(string name, OzAIProcMode mode, out List<OzAIVector> res, out string error)
        {
            if (!GetTensor(name, out var tens, out error))
            {
                error = $"Vector with the specified name, '{name}', not found.";
                res = null;
                return false;
            }

            res = new List<OzAIVector>();
            var numType = tens.Type.ToAINumType();
            var inst = numType.Create();
            var vecSize = inst.CalcSize(tens.ElementCounts[0].Value);
            
            for (ulong i = 0; i < tens.ElementCounts[1].Value; i++)
            {
                if (!numType.CreateVec(mode, out var vec, out error))
                    return false;

                if (!vec.Init(tens.Data, i * vecSize, vecSize, out error))
                    return false;

                if (!mode.GetCPUSettings(out var cpu, out error))
                    return false;
                if (numType != cpu.DefaultProcType)
                {
                    if (!vec.ToDType(cpu.DefaultProcType, out vec, out error))
                        return false;
                }
                res.Add(vec);
            }

            error = null;
            return true;
        }

        public bool GetMatrix(string name, OzAIProcMode mode, out OzAIMatrix res, out string error)
        {
            if (!GetTensor(name, out var tens, out error))
            {
                error = $"Matrix with the specified name, '{name}', not found.";
                res = null;
                return false;
            }
            res = null;

            var numType = tens.Type.ToAINumType();
            var vecSize = tens.ElementCounts[0].Value;
            var height = tens.ElementCounts[1].Value;

            if (!numType.CreateMat(mode, out res, out error))
                return false;
            if (!res.Init(tens.Data, 0ul, vecSize * height, vecSize, out error))
                return false;

            error = null;
            return true;
        }

        public bool GetMatricies(string name, OzAIProcMode mode, out List<OzAIMatrix> res, int count, out string error)
        {
            if (!GetTensor(name, out var tens, out error))
            {
                error = $"Matrix with the specified name, '{name}', not found.";
                res = null;
                return false;
            }
            res = new List<OzAIMatrix>();

            var numType = tens.Type.ToAINumType();
            var width = tens.ElementCounts[0].Value;
            var height = tens.ElementCounts[1].Value / (ulong)count;
            var matCount = height * width;
            for (int i = 0; i < count; i++)
            {
                if (!numType.CreateMat(mode, out var mat, out error))
                    return false;
                if (!mat.Init(tens.Data, (ulong)i * matCount, matCount, width, out error))
                    return false;
                res.Add(mat);
            }

            error = null;
            return true;
        }
    }
}
