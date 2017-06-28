using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace OperationContracts
{
    [ServiceContract]
    public interface IAdmin
    {
        #region Site Map

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Admin/SiteMap/Get/{ID}/{ApplicationID}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Admin.DC_SiteMap> GetSiteMapMaster(string ID, string applicationid);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Admin/SiteMap/GetByUserRole/{UserName}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Admin.DC_SiteMap> GetSiteMapMasterByUserRole(string UserName);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Admin/SiteMap/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateSiteMapMaster(DataContracts.Admin.DC_SiteMap SM);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Admin/SiteMap/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool AddSiteMapMaster(DataContracts.Admin.DC_SiteMap SM);

        #endregion

        #region Roles

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Admin/Roles/Get/{applicationID}/{PageNo}/{PageSize}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Admin.DC_Roles> GetAllRole(string applicationID, string PageNo, string PageSize);

        //[OperationContract]
        //[FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        //[WebInvoke(Method = "POST", UriTemplate = "Admin/Roles/ByEntityType", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        //IList<DataContracts.Admin.DC_Roles>GetAllRoleByEntityType(DataContracts.Admin.DC_Roles RlE);

        //[OperationContract]
        //[FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        //[WebInvoke(Method = "POST", UriTemplate = "Admin/Roles/ByApplication/{PageNo}/{PageSize}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        //IList<DataContracts.Admin.DC_Roles> GetAllRoleByApplication(DataContracts.Admin.DC_Roles RlE, string PageNo, string PageSize);


        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Admin/Roles/IsRoleExist", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool IsRoleExist(DataContracts.Admin.DC_Roles RlE);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Admin/Roles/AddEntityWithRole", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool AddUpdateRoleEntityType(DataContracts.Admin.DC_Roles rl);



        #endregion

        #region Url Authrization
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Admin/RoleAuthorizedForUrl", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool IsRoleAuthorizedForUrl(DataContracts.Admin.DC_RoleAuthorizedForUrl RAForUrl);
        #endregion

        #region User EntityTagging
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Admin/EntityType/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Admin.DC_EntityType> GetEntityType();

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Admin/Entity/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Admin.DC_EntityDetails> GetEntity(DataContracts.Admin.DC_EntityDetails ED);


        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Admin/UserEntity/AddUpdate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateUserEntity(DataContracts.Admin.DC_UserEntity UE);


        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Admin/UserEntity/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.Admin.DC_UserEntity GetUserEntityDetails(DataContracts.Admin.DC_UserEntity ED);

        #endregion

        #region UserManagement
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Admin/Users/Get/{PageNo}/{PageSize}/{ApplicationId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Admin.DC_UserDetails> GetAllUsers(string PageNo, string PageSize, string ApplicationId);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Admin/Users/SoftDelete", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message UsersSoftDelete(DataContracts.Admin.DC_UserDetails _objUserDetails);

        #endregion

        #region Application Management
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Admin/Application/Get/{PageNo}/{PageSize}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Admin.DC_ApplicationMgmt> GetAllApplication(string PageNo, string PageSize);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Admin/Application/Get/ByUser/{username}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string GetApplicationName(string username);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Admin/Application/AddUpdateApplicationMgmt", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddUpdateApplication(DataContracts.Admin.DC_ApplicationMgmt apmgmt);


        #endregion

    }
}
