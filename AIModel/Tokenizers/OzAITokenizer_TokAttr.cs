using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ozeki
{
    public partial class OzAITokenizer
    {
        // Handle per token attributes
        //NOTE: Each model customizes per token attributes.
        //NOTE: Per token attributes are missing from the GGUF file.
        //TODO: Extract attributes from GGUF file.

        //private static readonly string[] jinaPreTokenizers = ["jina-v2-de", "jina-v2-es", "jina-v2-code"];
        //private static readonly string[] phiModels = ["phi-3", "phi3"];

        //bool handleTokenAttrs(OzGGUFFile file, out string error)
        //{

        //    string model_name;
        //    string tokenizer_pre;

        //    if (!file.GetMDString("general.name", out model_name, out error, false) && error != null)
        //        return false;
        //    if (!file.GetMDString("tokenizer.ggml.pre", out tokenizer_pre, out error, false) && error != null)
        //        return false;

        //    model_name = model_name.ToLower();

        //    // set attributes by model/tokenizer name
        //    if (tokenizer_pre != null && containsAny(tokenizer_pre, jinaPreTokenizers))
        //    {
        //        Tokens["<mask>"].SetType(OzAIToken.LSTRIP, true);
        //    }
        //    else if (model_name != null && containsAny(model_name, phiModels))
        //    {
        //        configPhiModels();
        //    }

        //    error = null;
        //    return true;
        //}

        //bool containsAny(string name, string[] elements)
        //{
        //    foreach (var substr in elements)
        //    {
        //        if (name.Contains(substr))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //private void configPhiModels()
        //{
        //    foreach (var id in SpecialTokenCache)
        //    {
        //        TokenList[id].SetType(OzAIToken.RSTRIP, true);
        //    }

        //    Tokens["</s>"].SetType(OzAIToken.RSTRIP, true);

        //    foreach (var token in new string[3] { "<unk>", "<s>", "<|endoftext|>" })
        //    {
        //        Tokens[token].SetType(OzAIToken.RSTRIP, false);
        //    }
        //}
    }
}
