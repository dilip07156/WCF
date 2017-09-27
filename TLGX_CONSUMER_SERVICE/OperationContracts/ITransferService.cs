using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace OperationContracts
{
    [ServiceContract]
    public interface ITransferService
    {
        [OperationContract]
        UploadResponse UploadFile(RemoteFileInfo request);

        [OperationContract]
        bool UploadFileInChunks(FileData request);
    }

    [MessageContract]
    public class DownloadRequest
    {
        [MessageBodyMember]
        public string FileName;
    }

    [MessageContract]
    public class RemoteFileInfo : IDisposable
    {
        [MessageHeader(MustUnderstand = true)]
        public string FileName;

        [MessageHeader(MustUnderstand = true)]
        public long Length;

        [MessageBodyMember(Order = 1)]
        public System.IO.Stream FileByteStream;

        public void Dispose()
        {
            if (FileByteStream != null)
            {
                FileByteStream.Close();
                FileByteStream = null;
            }
        }
    }

    [MessageContract]
    public class UploadResponse
    {
        [MessageBodyMember(Order = 1)]
        public bool UploadSucceeded { get; set; }

        [MessageBodyMember(Order = 2)]
        public string UploadedPath { get; set; }
    }

    [DataContract]
    public class FileData
    {
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public byte[] BufferData { get; set; }
        [DataMember]
        public long FilePostition { get; set; }
    }

}
