using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DataContracts.Pentaho
{
    [DataContract]
    public class DC_PentahoApiCallReturnResult
    {
        public Guid ApiCall_Id { get; set; }
        public Guid ApiLocation_Id { get; set; }
        public Guid PentahoTxCall_Id { get; set; }
        public string Message { get; set; }
        public DataContracts.ReadOnlyMessage.StatusCode StatusCode { get; set; }

        public string PentahoTx_ViewDetails_Url { get; set; }
        public string PentahoTx_Start_Url { get; set; }
        public string PentahoTx_Cleanup_Url { get; set; }
        public string PentahoTx_Remove_Url { get; set; }
    }
}
