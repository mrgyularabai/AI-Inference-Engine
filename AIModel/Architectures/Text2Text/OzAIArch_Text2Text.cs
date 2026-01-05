using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public abstract class OzAIArch_Text2Text : OzAIArch
    {
        public uint ContextLength;

        public enum OzAIPoolingType
        {
            Unspecified = -1,

            /// <summary>
            /// Do not pool embeddings (per-token embeddings)
            /// </summary>
            None = 0,

            /// <summary>
            /// Take the mean of every token embedding
            /// </summary>
            Mean = 1,

            /// <summary>
            /// Return the embedding for the special "CLS" token
            /// </summary>
            CLS = 2,

            Last = 3,
        }

        // Embedding Architecture
        public uint EmbeddingLength;
        public OzAIEmbedding Embedding;
        public OzAIUnembedding Unembedding;
        public override bool InitFromFile(OzGGUFFile file, out string error)
        {
            if (!file.GetMDUInt32($"{Name}.context_length", out ContextLength, out error))
                return false;

            // Embedding Architecture
            if (!file.GetMDUInt32($"{Name}.embedding_length", out EmbeddingLength, out error))
                return false;

            // Architecture Specific Init
            if (!ArchInitialize(file, out error)) return false;

            error = null;
            return true;
        }
    }
}
