using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.FileTransfer
{
    [MessageContract]
    public class DC_FileDownloadRequest
    {
        [MessageBodyMember]
        public string FileName;
    }
}
