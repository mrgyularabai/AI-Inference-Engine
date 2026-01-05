using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    /// <summary>
    /// This applies scaled dot product attention pooling.
    /// Inputs: Originals, keys, values
    /// </summary>
    public partial class OzAIAttnGroup : OzAIArchComp
    {
        public OzAIMemNode Values;
        public OzAIMemNode Keys;
        public OzAIRoPE_Original RoPE;
        public List<OzAIAttnHead> Heads;

        public override string Name => "OzAIAttnHeadGroup";

        protected override bool InitInner(out string error)
        {
            Values = new OzAIMemNode();
            Keys = new OzAIMemNode();

            if (!createRoPE(out error)) return false;

            Heads = new List<OzAIAttnHead>((int)HParams.HeadCount);
            for (int i = 0; i < HParams.HeadCount; i++)
            {
                if (!createHead(i, out error)) return false;
            }

            error = null;
            return true;
        }

        bool createRoPE(out string error)
        {

            RoPE = new OzAIRoPE_Original();

            var ropeMem = new OzAICompIOMem_Unary()
            {
                Inputs = Keys,
                Outputs = Keys
            };
            var ropeIParams = new OzAICompIParams_ExecOnly()
            {
                ExecManager = IParams.ExecManager
            };
            var ropeParams = new OzAICompParams()
            {
                Mem = ropeMem,
                IParams = ropeIParams,
                HParams = HParams.HeadParams.RoPEParams
            };

            if (!RoPE.Init(ropeParams, out error))
                return false;

            return true;
        }

        bool createHead(int i, out string error)
        {
            var head = new OzAIAttnHead();

            var headMem = new OzAIAttnHead.CompMem()
            {
                Inputs = Mem.Inputs,
                Keys = Keys,
                Values = Values,
                Outputs = Mem.Outputs[i],
            };
            var headParams = new OzAICompParams()
            {
                Mem = headMem,
                IParams = IParams.HeadParams[i],
                HParams = HParams.HeadParams
            };

            if (!head.Init(headParams, out error))
                return false;

            Heads.Add(head);
            return true;
        }

        public override bool Forward(out string error)
        {
            if (!initMem(out error)) return false;
            if (!getKeys(out error)) return false;
            if (!getVals(out error)) return false;

            for (int i = 0; i < Heads.Count; i++)
            {
                var head = Heads[i];
                if (!head.Forward(out error))
                    return false;
            }

            error = null;
            return true;
        }

        bool initMem(out string error)
        {
            var exec = IParams.ExecManager;
            if (!exec.GetProcMode(out var mode, out error))
                return false;

            Values.Clear();
            if (!Values.AddVecs(mode, HParams.HeadParams.ValLen, Mem.Inputs.Count, out error))
                return false;

            Keys.Clear();
            if (!Keys.AddVecs(mode, HParams.HeadParams.KeyLen, Mem.Inputs.Count, out error))
                return false;

            return true;
        }

        bool getVals(out string error)
        {
            var exec = IParams.ExecManager;
            var inps = Mem.Inputs.GetArray();
            var vals = Values.GetArray();

            if (!exec.MatMul(inps, IParams.ValueWeights, vals, out error))
                return false;

            return true;
        }

        bool getKeys(out string error)
        {
            var exec = IParams.ExecManager;
            var inps = Mem.Inputs.GetArray();
            var keys = Keys.GetArray();

            if (!exec.MatMul(inps, IParams.KeyWeights, keys, out error))
                return false;
            if (!RoPE.Forward(out error))
                return false;

            return true;
        }
    }
}
