//https://hexed.it/
//https://github.com/ggerganov/ggml/blob/master/docs/gguf.md

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public partial class OzGGUFFile
    {

        public OzGGUF_TensorCount TensorCount;
        public List<OzGGUF_Tensor> Tensors;
        public Dictionary<string, int> TensorNamesToID;
        uint DataAlignement; //The Alignment of the Tensor Data
        public byte[] TensorData;
        public ulong DataOffset;

        long _headerEnd;

        bool _tensorsLoaded;

        public bool LoadTensors(out string error)
        {
            error = null;
            if (_tensorsLoaded) return true;
            lock (this)
            {
                if (_tensorsLoaded) return true;
                _tensorsLoaded = true;

                if (_headerEnd == 0 && !LoadHeaders(out error)) return false;

                if (!OpenStreamForReading(out var errorOpen))
                {
                    error = "Could not open GGUF stream. " + errorOpen;
                    return false;
                }

                try
                {
                    StreamToRead.Seek(_headerEnd, SeekOrigin.Begin);
                    var ret = LoadTensorFromStream(StreamToRead, out error);
                    CloseStreamForReading();
                    return ret;
                }
                catch (Exception e)
                {
                    error = "Error while parsing GGUF tensors. " + e.Message;
                    return false;
                }

                
            }
        }

        public FileStream StreamToRead;
        public bool OpenStreamForReading(out string errorMessage)
        {
            try
            {
                StreamToRead = new FileStream(FileName, FileMode.Open);
                errorMessage = null;
                return true;
            }
            catch (Exception e)
            {
                StreamToRead = null;
                errorMessage = e.Message;
                return false;
            }
        }

        public bool LoadBytesFromStream(ulong offset, ulong length, out byte[] bytes, out string error)
        {
            if (StreamToRead == null)
            {
                error = "File not open. Please call OpenStreamForReading first.";
                bytes = null;
                return false;
            }

            try
            {
                bytes = new byte[length];
                StreamToRead.Seek((int)offset, SeekOrigin.Begin);
                var read = StreamToRead.Read(bytes, 0, (int)length);
                if (read != (int)length)
                {
                    bytes = null;
                    error = "Could not read " + length + " bytes.";
                    return false;
                }
            }
            catch (Exception e)
            {
                bytes = null;
                error = e.Message;
                return false;
            }

            error = null;
            return true;
        }

        public void CloseStreamForReading()
        {
            if (StreamToRead == null) return;
            StreamToRead.Close();
            StreamToRead = null;
        }

        public bool LoadTensorFromStream(FileStream s, out string error)
        {
            // Load tensor data
            parseDataAlignment();
            if (!skipPadding(s, out error)) return false;
            _tensDataOffset = s.Position;
            if (!loadTensorDataBlocks(s, out error)) return false;
            return true;
        }

        //**********************************************
        // Parse Tensor Description
        //**********************************************

        bool parseTensors(Stream s, out string errorMessage)
        {
            TensorNamesToID = new Dictionary<string, int>();
            Tensors = new List<OzGGUF_Tensor>();
            for (ulong i = 0; i < TensorCount.Value; i++)
            {
                if (!ReadTo(s, out OzGGUF_Tensor tensor, out errorMessage)) return false;
                Tensors.Add(tensor);
                TensorNamesToID.Add(tensor.Name.Value, Tensors.Count-1);
                
            }
            errorMessage = null;
            return true;
        }

        //**********************************************
        // Skip to Next Aligned Block
        //**********************************************
        bool skipPadding(FileStream s, out string error)
        {
            try
            {
                var padding = DataAlignement - ((uint)s.Position % DataAlignement);
                if (padding == DataAlignement)
                {
                    error = "";
                    return true;
                }

                if (s.Length < s.Position + padding)
                {
                    error = "The file does not contain tensor data.";
                    return false;
                }
                s.Position += padding;
                error = null;
                return true;
            }
            catch (Exception e)
            {
                error = "Could not seek to data. " + e.Message;
                return false;
            }

        }

        bool loadTensorDataBlocks(FileStream s, out string error)
        {
            try
            {
                var size = calcSize((ulong)s.Position);
                for (int i = 0; i < (int)TensorCount.Value; i++)
                {
                    var tensor = Tensors[i];
                    var offset = (ulong)_tensDataOffset + tensor.DataOffset.Value;
                    tensor.Data = loadTensorData(s, offset, tensor.ByteCount);
                }
                error = null;
                return true;
            }
            catch (Exception e)
            {
                error = "Loading Tensor Data into memory failed: " + e.Message;
                return false;
            }
        }

        byte[] loadTensorData(FileStream s, ulong offset, ulong size)
        {
            var ret = new byte[size];
            s.Seek((long)offset, SeekOrigin.Begin);
            s.Read(ret, 0, (int)size);
            return ret;
        }

        bool loadTensorData(FileStream s, out string error)
        {
            try
            {
                var size = calcSize((ulong)s.Position);
                TensorData = new byte[size];
                s.Read(TensorData, 0, (int)size);
                error = null;
                return true;
            }
            catch (Exception e)
            {
                error = "Loading Tensor Data into memory failed: " + e.Message;
                return false;
            }
        }

        ulong calcSize(ulong tensorDataOffset)
        {
            ulong position = 0;
            for (int i = 0; i < (int)TensorCount.Value; i++)
            {
                var tensor = Tensors[i];
                var byteCount = tensor.ByteCount;
                tensor.PositionInFile = tensorDataOffset + position;
                tensor.PositionInMemory = position;
                tensor.PaddingAfter = DataAlignement - (byteCount % DataAlignement); //getObjPadding(byteCount) - byteCount;
                position += byteCount + tensor.PaddingAfter;
            }
            return position;
        }

        ulong getObjPadding(ulong size)
        {
            //block méretre kerekít fel:
            // block méret (DataAlignment): 32
            // size: 28->32
            // size: 29->32
            // size: 57->64

            return (size + DataAlignement - 1) & ~(DataAlignement - 1);
        }

        //**********************************************
        // ToString
        //**********************************************

        public string TensorDescriptions()
        {
            var s = new StringBuilder();
            foreach (var tesnor in Tensors)
            {
                s.AppendLine(tesnor.ToString());
            }
            return s.ToString();
        }
    }
}
