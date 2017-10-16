using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BL_FileTransfer : IDisposable
    {
        public void Dispose()
        {
            
        }

        public DataContracts.FileTransfer.DC_UploadResponse UploadFileInChunks(DataContracts.FileTransfer.DC_FileData request)
        {
            using (DataLayer.DL_FileTransfer obj = new DataLayer.DL_FileTransfer())
            {
                return obj.UploadFileInChunks(request);
            }
        }

        public DataContracts.FileTransfer.DC_UploadResponse TransferFileInChunks(DataContracts.FileTransfer.DC_FileData request)
        {
            using (DataLayer.DL_FileTransfer obj = new DataLayer.DL_FileTransfer())
            {
                return obj.TransferFileInChunks(request);
            }
        }

        public DataContracts.FileTransfer.DC_FileUploadResponse FileUpload(DataContracts.FileTransfer.DC_RemoteFileInfo request)
        {
            using (DataLayer.DL_FileTransfer obj = new DataLayer.DL_FileTransfer())
            {
                return obj.FileUpload(request);
            }
        }
    }
}
