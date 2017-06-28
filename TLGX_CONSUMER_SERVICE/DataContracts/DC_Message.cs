using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace DataContracts
{
    [DataContract]
    public class DC_Message
    {
        string _statusMessage;
        DataContracts.ReadOnlyMessage.StatusCode _statusCode;
        [DataMember]
        public string StatusMessage
        {
            get
            {
                return _statusMessage;
            }

            set
            {
                _statusMessage = value;
            }
        }
        [DataMember]
        public DataContracts.ReadOnlyMessage.StatusCode StatusCode
        {
            get
            {
                return _statusCode;
            }

            set
            {
                _statusCode = value;
            }
        }
    }
}
