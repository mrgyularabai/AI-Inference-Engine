using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAIMemNode
    {
        public OzAIMemNode()
        {
            _vecs = new List<OzAIVector>();
        }

        List<OzAIVector> _vecs;

        public ulong Count
        {
            get
            {
                return (ulong)_vecs.LongCount();
            }
        }

        public void Add(OzAIVector vec)
        {
            _vecs.Add(vec);
        }
        public void Add(OzAIVector[] vecs)
        {
            _vecs.AddRange(vecs);
        }
        public void Add(List<OzAIVector> vecs)
        {
            _vecs.AddRange(vecs);
        }

        public void Clear()
        {
            _vecs.Clear();
        }

        public void Add(OzAIMemNode node)
        {
            _vecs.AddRange(node._vecs);
        }

        public void Replace(OzAIMemNode node)
        {
            Clear();
            _vecs.AddRange(node._vecs);
        }

        public bool Clone(OzAIMemNode node, out string error)
        {
            Clear();
            for (int i = 0; i < node._vecs.Count; i++)
            {
                var vec = node._vecs[i];
                if (!vec.Clone(out var res, out error))
                {
                    error = "Could not clone. " + error;
                    return false;
                }
                _vecs.Add(res);
            }
            error = null;
            return true;
        }

        public bool CreateDestOf(OzAIMemNode inp, out string error)
        {
            Clear();
            var count = inp.Count;
            if (!inp.GetList()[0].GetNumCount(out var len, out error))
                return false;
            if (!inp.GetList()[0].GetProcMode(out var mode, out error))
                return false;
            return AddVecs(mode, len, count, out error);
        }

        public bool AddVecs(OzAIProcMode mode, ulong len, ulong count, out string error)
        {
            for (ulong i = 0; i < count; i++)
            {
                if (!OzAIVector.Create(mode, out var vec, out error))
                {
                    error = "Could not create vector in memory node: " + error;
                    return false;
                }
                if (!vec.Init(len, out error))
                {
                    error = "Could not initialize vector in memory node: " + error;
                    return false;
                }
                _vecs.Add(vec);
            }
            error = null;
            return true;
        }

        public List<OzAIVector> GetList()
        {
            return _vecs;
        }

        public OzAIVector[] GetArray()
        {
            return _vecs.ToArray();
        }
    }
}
