using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using DataContracts;

namespace OperationContracts
{
    [ServiceContract]
    public interface IActivity
    {
        #region Activity Search and Get
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/Search", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_ActivitySearch_RS> ActivitySearch(DataContracts.Masters.DC_Activity_Search_RQ Accomodation_Request);
        #endregion
    }
}
