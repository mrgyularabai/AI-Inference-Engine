using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public enum OzAIVocabType
    {
        NONE = 0, // For models without vocab (not supported for at the moment)
        SPM = 1, // LLaMA tokenizer based on byte-level BPE with byte fallback 
        BPE = 2, // GPT-2 tokenizer based on byte-level BPE (not supported for at the moment)
        WPM = 3, // BERT tokenizer based on WordPiece (not supported for at the moment)
        UGM = 4, // T5 tokenizer based on Unigram (not supported for at the moment)
    }
}
