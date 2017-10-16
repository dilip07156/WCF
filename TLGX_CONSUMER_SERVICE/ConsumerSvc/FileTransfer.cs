using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using OperationContracts;
using DataContracts;
using BusinessLayer;
using DataContracts.Mapping;
using DataContracts.FileTransfer;
using System.IO;

namespace ConsumerSvc
{
    public partial class Consumer : IConsumer
    {
        public DC_FileUploadResponse UploadFile(DC_RemoteFileInfo request)
        {
            using (BL_FileTransfer obj = new BL_FileTransfer())
            {
                return obj.FileUpload(request);
            }
        }

        public DC_UploadResponse TransferFileInChunks(DC_FileData request)
        {
            using (BL_FileTransfer obj = new BL_FileTransfer())
            {
                return obj.TransferFileInChunks(request);
            }
        }

        public DC_UploadResponse UploadFileInChunks(DC_FileData request)
        {
            using (BL_FileTransfer obj = new BL_FileTransfer())
            {
                return obj.UploadFileInChunks(request);
            }
        }

        public bool DeleteFile(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            return true;
        }
    }
}