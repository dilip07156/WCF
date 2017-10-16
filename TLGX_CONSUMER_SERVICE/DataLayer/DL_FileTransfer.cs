using DataContracts.FileTransfer;
using System;
using System.Collections.Generic;
using System.IO;
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
                var uploadDirectory = System.Configuration.ConfigurationManager.AppSettings["FileUploadLocation"].ToString();
                if(string.IsNullOrWhiteSpace(uploadDirectory))
                {
                    uploadDirectory = @"D:\UPLOAD\";
                }

                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                var FilePath = Path.Combine(uploadDirectory, request.FileName);

                if (request.FilePostition == 0)
                {
                    File.Create(FilePath).Close();
                }

                using (FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                {
                    fileStream.Seek(request.FilePostition, SeekOrigin.Begin);
                    fileStream.Write(request.BufferData, 0, request.BufferData.Length);
                }

                return new DC_UploadResponse { UploadedPath = FilePath, UploadSucceeded = true };
            }
            catch
            {
                return new DC_UploadResponse { UploadedPath = string.Empty, UploadSucceeded = false };
            }
        }

        public DataContracts.FileTransfer.DC_UploadResponse TransferFileInChunks(DataContracts.FileTransfer.DC_FileData request)
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

        public DataContracts.FileTransfer.DC_FileUploadResponse FileUpload(DataContracts.FileTransfer.DC_RemoteFileInfo request)
        {
            try
            {
                Guid FileUploadUniqueID = Guid.NewGuid();

                var uploadDirectory = @"D:\UPLOAD\";

                // Try to create the upload directory if it does not yet exist
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                // Check if a file with the same filename is already
                // present in the upload directory. If this is the case
                // then delete this file

                var path = Path.Combine(uploadDirectory, System.IO.Path.GetFileNameWithoutExtension(request.FileName) + "_" + FileUploadUniqueID.ToString().Replace("-", "_") + "." + System.IO.Path.GetExtension(request.FileName).Replace(".", ""));

                //if (File.Exists(path))
                //{
                //    File.Delete(path);
                //}

                // Read the incoming stream and save it to file
                const int bufferSize = 4096;

                var buffer = new byte[bufferSize];
                using (var outputStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    var bytesRead = request.FileByteStream.Read(buffer, 0, bufferSize);
                    while (bytesRead > 0)
                    {
                        outputStream.Write(buffer, 0, bytesRead);
                        bytesRead = request.FileByteStream.Read(buffer, 0, bufferSize);
                    }
                    outputStream.Close();
                }
                request.FileByteStream.Close();
                request.FileByteStream.Dispose();

                return new DataContracts.FileTransfer.DC_FileUploadResponse
                {
                    UploadSucceeded = true,
                    UploadedPath = path
                };
            }
            catch (Exception ex)
            {
                // Note down exception some where!
                request.FileByteStream.Close();
                request.FileByteStream.Dispose();
                return new DataContracts.FileTransfer.DC_FileUploadResponse
                {
                    UploadSucceeded = false,
                    UploadedPath = string.Empty
                };
            }
        }
    }
}
