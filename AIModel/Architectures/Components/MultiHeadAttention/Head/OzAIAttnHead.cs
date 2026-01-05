using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using static Ozeki.OzAIArchComp;

namespace Ozeki
{
    /// <summary>
    /// This applies scaled dot product attention pooling.
    /// Inputs: Originals, keys, values
    /// </summary>
    public partial class OzAIAttnHead : OzAIArchComp
    {
        public override string Name => "OzAIAttnHead";

        public OzAIMemNode Queries;
        public OzAIMemNode MyValues;

        OzAIRoPE_Original RoPE;

        protected override bool InitInner(out string error)
        {
            MyValues = new OzAIMemNode();
            Queries = new OzAIMemNode();

            RoPE = new OzAIRoPE_Original();

            var ropeMem = new OzAICompIOMem_Unary()
            {
                Inputs = Queries,
                Outputs = Queries
            };

            var ropeIParams = new OzAICompIParams_ExecOnly() 
            { 
                ExecManager = IParams.ExecManager,
            };

            var ropeParams = new OzAICompParams()
            {
                Mem = ropeMem,
                IParams = ropeIParams,
                HParams = HParams.RoPEParams
            };

            if (!RoPE.Init(ropeParams, out error))
                return false;

            error = null;
            return true;
        }

        public override bool Forward(out string error)
        {
            var exec = IParams.ExecManager;
            if (!exec.GetProcMode(out var mode, out error))
                return false;

            if (!initQKV(mode, out error))
                return false;
            // Make a destination vector for scores
            if (!OzAIVector.Create(mode, out var scoresVec, out error))
                return false;

            Mem.Outputs.Clear();
            var count = (ulong)Queries.GetArray().LongLength;
            for (ulong i = 0; i < count; i++)
            {
                if (!getQKV(mode, i, out var query, out var keys, out var vals, out error))
                    return false;
                if (!processQuery(i, query, keys, vals, scoresVec, out error))
                    return false;
            }

            error = null;
            return true;
        }

        OzAIMatrixRange _keyMatRange;
        bool initQKV(OzAIProcMode mode,  out string error)
        {
            if (!getQueries(mode, out error))
                return false;

            if (!getKeyMatRange(mode, out _keyMatRange, out error))
                return false;

            error = null;
            return true;
        }

        bool getQKV(OzAIProcMode mode, ulong i, out OzAIVector query, out OzAIMatrixRange keys, out OzAIVector[] vals, out string error)
        {
            query = null;
            keys = null;
            vals = null;

            query = Queries.GetArray()[i];
            
            _keyMatRange.Counts = new Tuple<ulong, ulong>(_keyMatRange.Counts.Item1, i + 1);
            keys = _keyMatRange;

            if (!MyValues.Clone(Mem.Values, out error))
                return false;
            vals = MyValues.GetArray();

            error = null;
            return true;
        }

        bool getQueries(OzAIProcMode mode, out string error)
        {
            Queries.Clear();
            if (!Queries.AddVecs(mode, HParams.KeyLen, Mem.Inputs.Count, out error))
                return false;
            var inps = Mem.Inputs.GetArray();
            var outs = Queries.GetArray();
            var exec = IParams.ExecManager;
            if (!exec.MatMul(inps, IParams.QueryMat, outs, out error))
                return false;
            if (!RoPE.Forward(out error))
                return false;
            error = null;
            return true;
        }

        bool getKeyMatRange(OzAIProcMode mode, out OzAIMatrixRange res, out string error)
        {
            res = null;
            var keys = Mem.Keys.GetArray();
            if (!OzAIMatrix.Create(mode, out var keyMat, out error))
                return false;
            if (!keyMat.Init(keys, out error))
                return false;
            if (!OzAIMatrixRange.ToFull(keyMat, out res, out error))
                return false;
            error = null;
            return true;
        }

        bool processQuery(ulong i, OzAIVector query, OzAIMatrixRange keyMatRange, OzAIVector[] values, OzAIVector scores, out string error)
        {
            var exec = IParams.ExecManager;
            ulong keyCount = i + 1;
            if (!scores.Init(keyCount, out error))
                return false;

            if (!OzAIVectorRange.ToFull(scores, out var scoresRange, out error))
                return false;
            if (!OzAIVectorRange.ToFull(query, out var queryRange, out error))
                return false;

            OzAIVectorRange[] src = [queryRange];
            OzAIVectorRange[] dst = [scoresRange];
            if (!exec.MatMul(src, keyMatRange, dst, out error))
                return false;
            if (!exec.Scale(dst, HParams.Scale, dst, out error))
                return false;
            if (!scores.ToDType(OzAINumType.Float32, out var fScores, out error))
                return false;
            if (!OzAIVectorRange.ToFull(fScores, out var fScoresRange, out error))
                return false;
            if (!exec.SoftMax([fScores], [fScoresRange], out error))
                return false;
            if (!fScores.ToDType(scores.GetNumType(), out scores, out error))
                return false;
            scoresRange.Vector = scores;

            var res = values[0];
            var scalar = scoresRange.GetNth(0);
            if (!exec.Scale([res], scalar, [res], out error))
                return false;

            for (ulong j = 1; j < keyCount; j++)
            {
                var val = values[j];
                scalar = scoresRange.GetNth(j);
                if (!exec.Scale([val], scalar, [val], out error))
                    return false;
                if (!exec.Add([val], [res], [res], out error))
                    return false;
            }

            Mem.Outputs.Add(res);

            error = null;
            return true;
        }

    }
}
