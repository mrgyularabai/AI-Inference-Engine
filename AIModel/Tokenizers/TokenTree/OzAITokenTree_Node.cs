using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ozeki
{
    public class OzAITokenTree_Node
    {
        public int ID { get; set; }
        public Dictionary<byte[], OzAITokenTree_Node> Children { get; set; }
        public List<int> Lengths;

        public OzAITokenTree_Node(int id)
        {
            ID = id;
            Lengths = new List<int>();
            Children = new Dictionary<byte[], OzAITokenTree_Node>(new OzByteArrayComparer());
        }

        public OzAITokenTree_Node AddChild(ref byte[] addition, OzAITokenTree_Node child)
        {
            if (addition.Length == 0)
                return null;
            var res = TryToken(ref addition);
            if (res != null)
                return res;

            Children.Add(addition, child);
            if (Lengths.Count == 0 || Lengths[Lengths.Count - 1] != addition.Length)
            {
                Lengths.Add(addition.Length);
            }
            return null;
        }

        static OzByteArrayComparer _byteArrayComparer = new OzByteArrayComparer();
        public OzAITokenTree_Node TryToken(ref byte[] addition)
        {
            if (addition.Length == 0)
                return null;
            if (Lengths.Count < Children.Count)
            {
                for (int i = 0; i < Lengths.Count; i++)
                {
                    if (Lengths[i] > addition.Length)
                        return null;
                    var res = new byte[Lengths[i]];
                    Buffer.BlockCopy(addition, 0, res, 0, Lengths[i]);
                    if (!Children.ContainsKey(res))
                        continue;
                    var child = Children[res];
                    if (addition.Length < 2)
                    {
                        addition = Array.Empty<byte>();
                        return child;
                    }
                    var len = addition.Length - Lengths[i];
                    res = new byte[len];
                    Buffer.BlockCopy(addition, Lengths[i], res, 0, len);
                    addition = res;
                    return child;
                }
            }
            else
            {
                foreach (var item in Children.Keys)
                {
                    if (!ContainsRange(addition, item))
                        continue;
                    var newLen = addition.Length - item.Length;
                    var res = new byte[newLen];
                    Buffer.BlockCopy(addition, item.Length, res, 0, newLen);
                    addition = res;
                    return Children[item];
                }
            }
            return null;
        }

        static bool ContainsRange(byte[] data, byte[] item)
        {
            if (data.Length < item.Length)
                return false;
            for (int j = 0; j < item.Length; j++)
            {
                if (data[j] != item[j])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
