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

namespace ConsumerSvc
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
    public class TransferService : OperationContracts.ITransferService
    {
        public UploadResponse UploadFile(RemoteFileInfo request)
        {
            //FileStream targetStream = null;
            //Stream sourceStream = request.FileByteStream;

            //string uploadFolder = @"D:\UPLOAD\";

            //string filePath = Path.Combine(uploadFolder, request.FileName);

            //using (targetStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            //{
            //    //read from the input stream in 65000 byte chunks
            //    const int bufferLen = 4096;
            //    byte[] buffer = new byte[bufferLen];
            //    int count = 0;
            //    while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
            //    {
            //        // save to output stream
            //        targetStream.Write(buffer, 0, count);
            //    }
            //    targetStream.Close();
            //    sourceStream.Close();
            //}

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
                
                var path = Path.Combine(uploadDirectory, System.IO.Path.GetFileNameWithoutExtension(request.FileName) + "_" + FileUploadUniqueID.ToString().Replace("-","_") + "." +  System.IO.Path.GetExtension(request.FileName).Replace(".",""));

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

                return new UploadResponse
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
                return new UploadResponse
                {
                    UploadSucceeded = false,
                    UploadedPath = string.Empty
                };
            }

        }

        public Response UploadFileInChunks(FileData request)
        {
            try
            {
                var uploadDirectory = @"D:\UPLOAD\";

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

                return new Response { UploadedPath = FilePath, UploadSucceeded = true };
            }
            catch
            {
                return new Response { UploadedPath = string.Empty, UploadSucceeded = false };
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
