using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using DataContracts.Masters;
using DataContracts;

namespace OperationContracts
{
    [ServiceContract]
    public interface IMasters
    {
        #region Country Master

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Country/Search/Names", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<string> GetCountryNameList(DataContracts.Masters.DC_Country_Search_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "GetAllCountries", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.DC_Master_Country> GetAllCountries();

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Country/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Country> GetCountryMaster(DataContracts.Masters.DC_Country_Search_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Country/AddUpdateCountryMaster", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message AddUpdateCountryMaster(DataContracts.Masters.DC_Country param);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Country/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool AddCountryMaster(DataContracts.Masters.DC_Country obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Country/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateCountryMaster(DataContracts.Masters.DC_Country obj);
        #endregion

        #region StateMaster
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "GetAllStates", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DC_Master_State> GetAllStates();

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "GetStatesByCountry/{Country_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DC_Master_State> GetStatesByCountry(string Country_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/States/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DC_Master_State> GetStatesMaster(DC_State_Search_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/States/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddStatesMaster(DC_Master_State obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/States/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message UpdateStatesMaster(DC_Master_State obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/States/Get/AlphaPaging", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<State_AlphaPage> GetStatesAlphaPaging(DC_State_Search_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/GetStateNameAndCode", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_State_Master_DDL GetStateNameAndCode(DC_State_Master_DDL_RQ obj);
        #endregion

        #region City Master

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/City/Search/Names", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<string> GetCityNameList(DataContracts.Masters.DC_City_Search_RQ RQ);


        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "GetAllCities", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.DC_Master_City> GetAllCities();

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "GetCitiesByCountry/{Country_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.DC_Master_City> GetCitiesByCountry(string Country_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/City/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_City> GetCityMaster(DataContracts.Masters.DC_City_Search_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/City/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddCityMaster(DataContracts.Masters.DC_City obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/City/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message UpdateCityMaster(DataContracts.Masters.DC_City obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/City/Get/AlphaPaging", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.City_AlphaPage> GetCityAlphaPaging(DataContracts.Masters.DC_City_Search_RQ RQ);


        #endregion

        #region DynamicAttributes
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DynamicAttributes/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_DynamicAttributes> GetDynamicAttributes(DataContracts.Masters.DC_DynamicAttributes RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DynamicAttributes/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateDynamicAttributes(DataContracts.Masters.DC_DynamicAttributes RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DynamicAttributes/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool AddDynamicAttributes(DataContracts.Masters.DC_DynamicAttributes RQ);
        #endregion

        #region Team Masters
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/Teams/Get/{Team_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.DC_Teams> GetTeamMasterData(string Team_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Teams/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateTeamMasterData(DataContracts.DC_Teams RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Teams/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool AddTeamMasterData(DataContracts.DC_Teams RQ);
        #endregion

        #region Update Country State City Area Location Hierarchy
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "UpdateAddressMasterHierarchy", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateAddressMasterHierarchy(DataContracts.DC_Address.DC_Country_State_City_Area_Location RQ);
        #endregion

        #region Port Master
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/Port/Get/{PageNo}/{PageSize}/{PortId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_PortMaster> GetPort(string PageSize, string PageNo, string PortId);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Port/Search", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_PortMaster> PortMasterSeach(DataContracts.Masters.DC_PortMaster_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Port/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message UpdatePortMaster(DataContracts.Masters.DC_PortMaster obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Port/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddPortMaster(DataContracts.Masters.DC_PortMaster obj);
        #endregion

        #region Master Attribute
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/MasterAttribute/GetFor", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_MasterAttribute> GetAllAttributeAndValuesByFOR(DataContracts.Masters.DC_MasterAttribute _obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/MasterAttribute/ByParentAttributeValue", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_MasterAttribute> GetAllAttributeAndValuesByParentAttributeValue(DataContracts.Masters.DC_MasterAttribute _obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/MasterAttribute/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_MasterAttribute> GetAllAttributeAndValues(DataContracts.Masters.DC_MasterAttribute _obj);





        //Shifted methods From Here
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/MasterAttributes/GetAll", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DC_M_masterattribute> GetMasterAttributes(DC_M_masterattribute _obj);


        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/MasterAttributes/GetParentAttributes/{MasterFor}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DC_M_masterparentattributes> GetParentAttributes(string MasterFor);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/MasterAttributes/GetAttributesMasterForValues", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DC_M_masterparentattributes> GetAttributesMasterForValues();

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/MasterAttributes/GetAttributesForValues/{MasterAttribute_id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DC_M_masterparentattributes> GetAttributesForValues(string MasterAttribute_id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/MasterAttributes/SaveMasterAttribute", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message SaveMasterAttribute(DC_M_masterattribute obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/MasterAttributes/GetAttributeValuesById/{MasterAttribute_Id}/{PageSize}/{PageNo}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DC_M_masterattributevalue> GetAttributeValues(string MasterAttribute_Id, string PageSize, string PageNo);
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/MasterAttributes/GetAttributeDetails/{MasterAttribute_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_M_masterattribute GetAttributeDetails(string MasterAttribute_Id);
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/MasterAttributes/SaveAttributeValue", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message SaveAttributeValue(DC_M_masterattributevalue obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/MasterAttributes/GetListAttributeAndValuesByFOR", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_M_masterattributelists GetListAttributeAndValuesByFOR(DC_MasterAttribute _obj);


        //Shifted methods till Here




        #endregion

        #region Supplier
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Supplier/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Supplier> GetSupplier(DataContracts.Masters.DC_Supplier_Search_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/SupplierByEntity/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Supplier_DDL> GetSupplierByEntity(DataContracts.Masters.DC_Supplier_Search_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/SupplierMarket/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_SupplierMarket> GetSupplierMarket(DataContracts.Masters.DC_SupplierMarket RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/ProductCategoryBySupplier/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Supplier_ProductCategory> GetProductCategoryBySupplier(DataContracts.Masters.DC_Supplier_ProductCategory RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Supplier/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateSupplier(DataContracts.Masters.DC_Supplier _objSup);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/SupplierMarket/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateSupplierMarket(DataContracts.Masters.DC_SupplierMarket _objSupMkt);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/SupplierMarket/SoftDelete", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message SupplierMarketSoftDelete(DataContracts.Masters.DC_SupplierMarket _objSupMkt);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Supplier_ProductCategory/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateSupplier_ProductCategory(DataContracts.Masters.DC_Supplier_ProductCategory _objSupCat);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Supplier_ProductCategory/SoftDelete", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message Supplier_ProductCategorySoftDelete(DataContracts.Masters.DC_Supplier_ProductCategory _objSupCat);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Supplier_ApiLocation/Search", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Masters.DC_Supplier_ApiLocation> SupplierApiLocation_Search(DataContracts.Masters.DC_Supplier_ApiLocation RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Supplier_ApiLocation/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message SupplierApiLocation_Update(DataContracts.Masters.DC_Supplier_ApiLocation objApiLoc);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Supplier_ApiLocation/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message SupplierApiLocation_Add(DataContracts.Masters.DC_Supplier_ApiLocation objApiLoc);

        #endregion

        #region Status
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "GetAllStatuses", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DC_Statuses> GetAllStatuses();
        #endregion

        #region "Activity Master"
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Activity/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Activity> GetActivityMaster(DataContracts.Masters.DC_Activity_Search_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/ActivityBySupplier/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Activity> GetActivityMasterBySupplier(DataContracts.Masters.DC_Activity_Search_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/ActivityContent/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Activity_Content> GetActivityContentMaster(DataContracts.Masters.DC_Activity_Content RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/ActivityNames/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<string> GetActivityNames(DataContracts.Masters.DC_Activity_Search_RQ RQ);
        #endregion

        #region Common Funciton to Get Codes by Entity Type

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "GetCodeById/{objName}/{obj_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string GetCodeById(string objName, string obj_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "GetRemarksForMapping/{from}/{Mapping_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string GetRemarksForMapping(string from, string Mapping_Id);


        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/GetDetailsByIdOrName", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.Masters.DC_GenericMasterDetails_ByIDOrName GetDetailsByIdOrName(DataContracts.Masters.DC_GenericMasterDetails_ByIDOrName RQ);

        #endregion

        #region To Fill DropDown
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/ForDropDown/GetProductByCity", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_Accomodation_DDL> GetProductByCity(DC_Accomodation_DDL _obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/ForDropDown/GetMasterStateData/{Country_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_State_Master_DDL> GetMasterStateData(string Country_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/ForDropDown/GetStateByCity/{City_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_State_Master_DDL> GetStateByCity(string City_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/ForDropDown/GetMasterCityData/{Country_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_City_Master_DDL> GetMasterCityData(string Country_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/ForDropDown/GetMasterCityAreaLocationDetail/{CityAreaLocation_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_CityAreaLocation> GetMasterCityAreaLocationDetail(string CityAreaLocation_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/ForDropDown/GetMasterCityAreaDetail/{CityArea_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_CityArea> GetMasterCityAreaDetail(string CityArea_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/ForDropDown/GetSupplierMasterData", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_Supplier_DDL> GetSupplierMasterData();

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/GetSupplierDataByMapping_Id/{objName}/{Mapping_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Supplier_DDL GetSupplierDataByMapping_Id(string objName, string Mapping_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/ForDropDown/GetCountryCodes/{Country_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_CountryMaster> GetCountryCodes(string Country_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/ForDropDown/GetMasterCountryDataList", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_CountryMaster> GetMasterCountryDataList();

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/GetActivityForDDL/{CountryName}/{CityName}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_Activity_DDL> GetActivityByCountryCity(string CountryName, string CityName);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/ForDropDown/GetSuppliersByProductCategory/{ProductCategory}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_Supplier_DDL> GetSuppliersByProductCategory(string ProductCategory);

        #endregion

        #region City Area and Location
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/SaveCityAreaLocation", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool SaveCityAreaLocation(DC_CityAreaLocation _obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/GetMasterCityAreaLocationData/{CityArea_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_CityAreaLocation> GetMasterCityAreaLocationData(string CityArea_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/SaveCityArea", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool SaveCityArea(DC_CityArea _obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/GetMasterCityAreaData/{City_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_CityArea> GetMasterCityAreaData(string City_Id);
        #endregion

        #region Keyword
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Keyword/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message AddUpdateKeyword(DC_Keyword param);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Keyword/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_Keyword> SearchKeyword(DC_Keyword_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Keyword/Alias/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_keyword_alias> SearchKeywordAlias(DC_Keyword_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Keyword/Apply", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message ApplyKeyword(DC_keywordApply_RQ RQ);
        #endregion

        //[OperationContract]
        //[FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        //[WebInvoke(Method = "GET", UriTemplate = "Master/ColumnNames/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        //string[] GetColumnNames(string TableName);


        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Master/GetListOfColumnNamesByTable/{TableName}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<string> GetListOfColumnNamesByTable(string TableName);

        #region Zone Master

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Zone/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddzoneMaster(DataContracts.Masters.DC_ZoneRQ param);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Zone/AddZoneCity", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddZoneCityMapping(DataContracts.Masters.DC_ZoneRQ param);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Zone/Search", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DC_ZoneSearch>SearchZone(DataContracts.Masters.DC_ZoneRQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Zone/SearchZoneCities", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DC_ZoneCitiesSearch>SearchZoneCities(DataContracts.Masters.DC_ZoneRQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Zone/DeleteZoneCities", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message DeleteZoneCities(DataContracts.Masters.DC_ZoneRQ RQ);


        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Zone/DeactivateOrActivateZones", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message DeactivateOrActivateZones(DataContracts.Masters.DC_ZoneRQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Zone/SearchZoneHotels", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DC_ZoneHotelList> SearchZoneHotels(DataContracts.Masters.DC_ZoneRQ param);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Zone/InsertInZoneProductTable", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message InsertZoneHotelsInTable(DataContracts.Masters.DC_ZoneRQ param);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Master/Zone/DeleteFromZoneProductTable", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message DeleteZoneHotelsInTable(DataContracts.Masters.DC_ZoneRQ param);
        #endregion
    }
}
