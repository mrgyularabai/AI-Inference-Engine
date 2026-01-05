using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAIScalar : OzAIVectorRange
    {
        public static bool Create(float val, OzAIProcMode mode, out OzAIScalar res, out string error)
        {
            res = new OzAIScalar();
            res.Offset = 0;
            if (!OzAIVector.Create(mode, out res.Vector, out error))
                return false;
            if (!res.Vector.Init([val], 0, 1, out error))
                return false;
            return true;
        }

        public static bool CreateInt(int val, OzAIProcMode mode, out OzAIScalar res, out string error)
        {
            res = new OzAIScalar();
            res.Offset = 0;
            if (!OzAIIntVec.Create(mode, out var vec, out error))
                return false;
            res.Vector = vec;
            if (!res.Vector.Init(1, out error))
                return false;
            if (!vec.SetNthInt(val, 0, out error))
                return false;
            return true;
        }

        public static bool CreateFloat(float val, OzAIProcMode mode, out OzAIScalar res, out string error)
        {
            res = new OzAIScalar();
            res.Offset = 0;
            if (!OzAIFloatVec.Create(mode, out var vec, out error))
                return false;
            res.Vector = vec;
            if (!res.Vector.Init([val], 0, 1, out error))
                return false;
            return true;
        }

        public static bool CreateHalf(Half val, OzAIProcMode mode, out OzAIScalar res, out string error)
        {
            res = new OzAIScalar();
            res.Offset = 0;
            if (!OzAIHalfVec.Create(mode, out var vec, out error))
                return false;
            res.Vector = vec;
            if (!res.Vector.Init(1, out error))
                return false;
            if (!vec.SetNthHalf(val, 0, out error))
                return false;
            return true;
        }

        public override ulong Length { get => 1; set { } }
    }
}
