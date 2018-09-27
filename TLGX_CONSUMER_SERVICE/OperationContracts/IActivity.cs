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
        [WebInvoke(Method = "GET", UriTemplate = "GetActivity/Contacts/{Activity_Flavour_Id}/{DataKey_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Activity_Contact> GetActivityContacts(string Activity_Flavour_Id, string DataKey_Id);

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
        [WebInvoke(Method = "GET", UriTemplate = "GetActivity/LegacyProductId/{Activity_Flavour_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        int GetLegacyProductId(string Activity_Flavour_Id);
        #endregion

        #region "Activity Status"
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "GetActivity/Status/{Activity_Flavour_Id}/{DataKey_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Activity_Status> GetActivityStatus(string Activity_Flavour_Id, string DataKey_Id);

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
        [WebInvoke(Method = "POST", UriTemplate = "Activity/Media/AddUpdateMedia", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateActivityMedia(DataContracts.Masters.DC_Activity_Media RQ);
        #endregion

        #region Activity Inclusions
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/Inclusions/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DC_Activity_Inclusions> GetActivityInclusions(DC_Activity_Inclusions_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/Inclusions/AddUpdateInclusions", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateActivityInclusions(DataContracts.Masters.DC_Activity_Inclusions RQ);
        #endregion

        #region InclusionDetails
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/InclusionDetails/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DC_Activity_InclusionsDetails> GetActivityInclusionDetails(DC_Activity_InclusionDetails_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/InclusionDetails/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateInclusionDetails(DataContracts.Masters.DC_Activity_InclusionsDetails RQ);

        #endregion

        #region  Activiy Clasification Attributes
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/ClassificationAttributes/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Activity_ClassificationAttributes> GetActivityClasificationAttributes(DataContracts.Masters.DC_Activity_ClassificationAttributes_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/ClassificationAttributes/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool AddActivityClassifiationAttributes(DataContracts.Masters.DC_Activity_ClassificationAttributes RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/ClassificationAttributes/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateActivityClassifiationAttributes(DataContracts.Masters.DC_Activity_ClassificationAttributes RQ);
        
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

        #region PickUpDrop Details
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/PickUpDropDetails/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Activity_PickUpDropDetails> GetPickUpDropDetails(DataContracts.Masters.DC_Activity_PickUpDropDetails_RQ RQ);
        
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/PickUpDropDetails/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdatePickUpDropDetails(DC_Activity_PickUpDropDetails RQ);
        #endregion

        #region Activity Description
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/Descriptions/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Activity_Descriptions> GetActivityDescription(DataContracts.Masters.DC_Activity_Descriptions_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/Descriptions/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateActivityDescription(DC_Activity_Descriptions RQ);
        #endregion

        #region Activity Flavour
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/Flavours/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Masters.DC_Activity_Flavour> GetActivityFlavour(DataContracts.Masters.DC_Activity_Flavour_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/Flavours/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateActivityFlavour(DC_Activity_Flavour RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/Flavours/AddUpdateCA", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateActivityCA(List<DC_Activity_CA_CRUD> RQ);
        #endregion

        #region Activity Flavour Options
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/FlavourOptions/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Masters.DC_Activity_Flavour_Options> GetActivityFlavourOptions(DataContracts.Masters.DC_Activity_Flavour_Options_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/FlavourOptions/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateActivityFlavourOptions(DC_Activity_Flavour_Options RQ);
        #endregion

        #region Activity Deals
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/Deals/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Activity_Deals> GetActivityDeals(DataContracts.Masters.DC_Activity_Deals_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/Deals/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateActivityDeals(DC_Activity_Deals RQ);
        #endregion

        #region Activity Prices
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/Prices/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Activity_Prices> GetActivityPrices(DataContracts.Masters.DC_Activity_Prices_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/Prices/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateActivityPrices(DC_Activity_Prices RQ);
        #endregion

        #region Supplier Product mapping
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/SupplierProdMapping/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Activity_SupplierProductMapping> GetActSupplierProdMapping(DataContracts.Masters.DC_Activity_SupplierProductMapping_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/SupplierProdMapping/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateActSupplierProdMapping(DC_Activity_SupplierProductMapping RQ);
        #endregion

        #region Reviews And Scores
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/ReviewsAndScores/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Activity_ReviewsAndScores> GetActReviewsAndScores(DataContracts.Masters.DC_Activity_ReviewsAndScores_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/ReviewsAndScores/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateActReviewsNScores(DC_Activity_ReviewsAndScores RQ);
        #endregion
        
        #region Supplier Product mapping_CA
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/SupplierProdMapping_CA/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Activity_SupplierProductMapping_CA> GetActSupplierProdMapping_CA(DataContracts.Masters.DC_Activity_SupplierProductMapping_CA_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/SupplierProdMapping_CA/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateActSupplierProdMapping_CA(DC_Activity_SupplierProductMapping_CA RQ);
        #endregion

        #region Activity Policy
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/ActivityPolicy/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Activity_Policy> GetActivityPolicy(DataContracts.Masters.DC_Activity_Policy_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/ActivityPolicy/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateActivityPolicy(DataContracts.Masters.DC_Activity_Policy RQ);
        #endregion

        #region Activity DaysOfWeek
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Activity/ActivityOperatingDays/Get/{Activity_Flavour_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Masters.DC_Activity_OperatingDays> GetActivityOperatingDays(string Activity_Flavour_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/ActivityOperatingDays/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateActivityOperatingDays(List<DataContracts.Masters.DC_Activity_OperatingDays> RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Activity/ActivityNonOperatingDays/Get/{Activity_Flavour_Id}/{PageSize}/{PageNo}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Masters.DC_Activity_OperatingDays> GetActivityNonOperatingDays(string Activity_Flavour_Id, string PageSize, string PageNo);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/ActivityNonOperatingDays/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateActivityNonOperatingDays(List<DataContracts.Masters.DC_Activity_OperatingDays> RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/DeleteActivityNonOperatingDaysById/DeleteNonOperatingDays", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message DeleteActivityNonOperatingDaysById(string ActivityDaysOfOperationId);
        #endregion

        #region Activity_Flavour_Status
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/Flavours/AddUpdateStatus", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateActivityFlavoursStatus(DC_ActivityFlavoursStatus _obj);
        #endregion

        #region Activity_Supplier_ProductSubCat
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/GetSupplierProductSubType", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Activity_CategoryTypes_DDL> GetSupplierProductSubType(DataContracts.Masters.DC_Supplier_DDL _objAct);
        #endregion

        #region Activity Reports
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/GetActivitiesReport/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DC_Activity_Report_RS> GetActivitiesReport(DC_ActivityCountStats ReportType);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Activity/GetActivitiesProductDetailsReport/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DC_ActivityProductDetailsReport> GetActivitiesProductDetailsReport(DC_ActivityCountStats ReportType);
        #endregion

    }
}
