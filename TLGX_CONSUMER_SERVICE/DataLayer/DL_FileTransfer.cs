using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DL_FileTransfer : IDisposable
    {
        public void Dispose()
        {

        }

        public DataContracts.FileTransfer.DC_UploadResponse UploadFileInChunks(DataContracts.FileTransfer.DC_FileData request)
        {
            try
            {
                DHSVC.DC_FileData file = new DHSVC.DC_FileData();
                file.FileName = request.FileName;
                file.FilePostition = request.FilePostition;
                file.BufferData = request.BufferData;

                object result = null;
                DHSVCProxy.PostData(ProxyFor.DataHandler, System.Configuration.ConfigurationManager.AppSettings["Data_Handler_Upload_File_InChunks"], file, file.GetType(), typeof(DataContracts.FileTransfer.DC_UploadResponse), out result);
                file = null;
                return result as DataContracts.FileTransfer.DC_UploadResponse;
            }
            catch (Exception e)
            {
                return new DataContracts.FileTransfer.DC_UploadResponse { UploadedPath = string.Empty, UploadSucceeded = false };
            }
        }
    }
}
