using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using OperationContracts;
using System.IO;
using BusinessLayer;
using DataContracts.FileTransfer;

namespace ConsumerSvc
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
    public class TransferService : OperationContracts.IFileTransfer
    {
        public bool DeleteFile(string FilePath)
        {
            throw new NotImplementedException();
        }

        public DC_UploadResponse TransferFileInChunks(DC_FileData request)
        {
            throw new NotImplementedException();
        }

        public DC_FileUploadResponse UploadFile(DC_RemoteFileInfo request)
        {
            throw new NotImplementedException();
        }

        public DC_UploadResponse UploadFileInChunks(DC_FileData request)
        {
            throw new NotImplementedException();
        }
    }
}
