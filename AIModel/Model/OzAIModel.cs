using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{

    public partial class OzAIModel 
    {
        public string ModelName;
        public string modelPath;
        public OzGGUFFile GGUFFile;
        public OzAIArch Architecture;
        public OzAITokenizer Tokenizer;

        protected string GetDescription()
        {
            if (!File.Exists(modelPath)) return "Model file not found";
            return Path.GetFileName(modelPath); 
        }

        public bool GetNextWord(string inputText, out string outputWord, out bool responseComplete, out string errorMessage)
        {
            responseComplete = false;
            try
            {
                // Tokenization
                if (!Tokenizer.GetTokens(inputText, out var inputTokens, out var times, out var error))
                {
                    outputWord = null;
                    errorMessage = "Could not tokenize. " + error;
                    return false;
                }

                // Inference
                if (!infer(inputTokens, out var outputTokens, out var errorInfer))
                {
                    outputWord = null;
                    errorMessage = "Could not tokenize. " + error;
                    return false;
                }

                outputWord = Tokenizer.GetStringsRaw(outputTokens);

                errorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Inference error. " + ex.Message;
                outputWord = null;
                return false;
            }
        }

        public bool infer(List<int> inputTokens, out List<int> outputTokens, out string errorMessage)
        {
            outputTokens = null;

            if (!OzAIIntVec.Create(Architecture.Mode, out var ints, out errorMessage))
                return false;
            var bytes = new byte[inputTokens.Count * 4];
            var intArray = inputTokens.ToArray();
            Buffer.BlockCopy(intArray, 0, bytes, 0, intArray.Length * 4);
            if (!ints.Init(bytes, 0, (ulong)inputTokens.Count, out errorMessage))
                return false;
            Architecture.IN = ints;

            if (!Architecture.Forward(out errorMessage))
                return false;

            if (!Architecture.OUT.ToBytes(out var bytesRes, out errorMessage))
                return false;
            var resInts = new int[bytesRes.Length / 4];
            Buffer.BlockCopy(bytesRes, 0, resInts, 0, bytesRes.Length);
            outputTokens = new List<int>(resInts);

            errorMessage = null;
            return true;
        }
    }
}
