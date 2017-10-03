using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using DataContracts;
using DataContracts.Masters;

namespace OperationContracts
{
    [ServiceContract]
    public interface IActivity
    {
        #region Activity Search and Get
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/Search", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_ActivitySearch_RS> ActivitySearch(DataContracts.Masters.DC_Activity_Search_RQ Activity_Request);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/AddUpdateActivity", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateActivity(DataContracts.Masters.DC_Activity _objAct);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/AddUpdateProductInfo", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateProductInfo(DataContracts.Masters.DC_Activity _objAct);
        #endregion

        #region "Activity Contact"
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "GetActivity/Contacts/{Activity_Id}/{DataKey_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Activity_Contact> GetActivityContacts(string Activity_Id, string DataKey_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "UpdateActivity/Contacts", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateActivityContacts(DataContracts.Masters.DC_Activity_Contact AC);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "AddActivity/Contacts", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool AddActivityContacts(DataContracts.Masters.DC_Activity_Contact AC);
        
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "GetActivity/LegacyProductId/{Activity_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string GetLegacyProductId(string Activity_Id);
        #endregion

        #region "Activity Status"
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "GetActivity/Status/{Activity_Id}/{DataKey_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Activity_Status> GetActivityStatus(string Activity_Id, string DataKey_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "UpdateActivity/Status", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateActivityStatus(DataContracts.Masters.DC_Activity_Status AS);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "AddActivity/Status", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool AddActivityStatus(DataContracts.Masters.DC_Activity_Status AS);
        #endregion
        #region Activity Media
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/Media/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Activity_Media> GetActivityMedia(DataContracts.Masters.DC_Activity_Media_Search_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/Media/AddMedia", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddActivityMedia(DataContracts.Masters.DC_Activity_Media RQ);
        #endregion

        #region Activity Inclusions
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/Inclusions/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DC_Activity_Inclusions_RS> GetActivityInclusions(DC_Activity_Inclusions_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/Inclusions/AddInclusions", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddActivityInclusions(DataContracts.Masters.DC_Activity_Inclusions RQ);
        #endregion

        #region  Activiy Clasification Attributes
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/ClassificationAttributes/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Activity_ClassificationAttributes> GetActivityClasificationAttributes(DataContracts.Masters.DC_Activity_ClassificationAttributes_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/ClassificationAttributes/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateActivityClassifiationAttributes(DC_Activity_ClassificationAttributes RQ);
        #endregion

        #region Activity PickUpDrop
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/PickUpDrop/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
         IList<DataContracts.Masters.DC_Activity_PickUpDrop> GetActivityPickUpDrop(DataContracts.Masters.DC_Activity_PickUpDrop_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/PickUpDrop/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdatePickUpDrop(DC_Activity_PickUpDrop RQ);

        #endregion
    }
}
