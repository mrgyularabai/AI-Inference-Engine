using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAITokenTree
    {
        OzAITokenTree_Node root;

        public OzAITokenTree(int unk)
        {
            root = new OzAITokenTree_Node(unk);
        }

        public void Add(OzAIToken token)
        {
            var val = new OzAITokenTree_Node(token.ID);
            var res = new byte[token.Text.Length];
            Buffer.BlockCopy(token.Text, 0, res, 0, res.Length);
            var current = root;
            while (current != null)
            {
                current = current.AddChild(ref res, val);
            }
        }

        public bool Get(byte[] data, out int res)
        {
            var addition = data;
            var current = root;
            res = root.ID;
            while (current != null)
            {
                res = current.ID;
                current = current.TryToken(ref addition);
            }
            return res != root.ID;
        }
    }
}
