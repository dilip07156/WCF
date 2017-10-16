using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.FileTransfer
{
   [DataContract]
    public class DC_RemoteFileInfo
    {
        [DataMember]
        public string FileName;

        [DataMember]
        public long Length;

        [DataMember]
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
}
