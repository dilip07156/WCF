using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using OperationContracts;
using DataContracts;
using BusinessLayer;

namespace ConsumerSvc
{
    public partial class Consumer : IConsumer
    {
        #region Site Map
        public IList<DataContracts.Admin.DC_SiteMap> GetSiteMapMaster(string ID,string applicationID)
        {
            using (BusinessLayer.BL_Admin obj = new BL_Admin())
            {
                return obj.GetSiteMapMaster(ID,applicationID);
            }
        }

        public IList<DataContracts.Admin.DC_SiteMap> GetSiteMapMasterByUserRole(string UserName)
        {
            using (BusinessLayer.BL_Admin obj = new BL_Admin())
            {
                return obj.GetSiteMapMasterByUserRole(UserName);
            }
        }

        public bool UpdateSiteMapMaster(DataContracts.Admin.DC_SiteMap SM)
        {
            using (BusinessLayer.BL_Admin obj = new BL_Admin())
            {
                return obj.UpdateSiteMapMaster(SM);
            }
        }

        public bool AddSiteMapMaster(DataContracts.Admin.DC_SiteMap SM)
        {
            using (BusinessLayer.BL_Admin obj = new BL_Admin())
            {
                return obj.AddSiteMapMaster(SM);
            }
        }
        #endregion

        #region Roles
        public IList<DataContracts.Admin.DC_Roles> GetAllRole(string ApplicationID, string PageNo, string PageSize)
        {
            using (BusinessLayer.BL_Admin obj = new BL_Admin())
            {
                return obj.GetAllRole(ApplicationID,PageNo, PageSize);
            }
        }
        public bool AddUpdateRoleEntityType(DataContracts.Admin.DC_Roles RlE)
        {
            using (BusinessLayer.BL_Admin obj = new BL_Admin())
            {
                return obj.AddUpdateRoleEntityType(RlE);
            }
        }
        public bool IsRoleExist(DataContracts.Admin.DC_Roles RlE)
        {
            using (BusinessLayer.BL_Admin obj = new BL_Admin())
            {
                return obj.IsRoleExist(RlE);
            }
        }
        //public IList<DataContracts.Admin.DC_Roles> GetAllRoleByEntityType(DataContracts.Admin.DC_Roles RlE)
        //{
        //    using (BusinessLayer.BL_Admin obj = new BL_Admin())
        //    {
        //        return obj.GetAllRoleByEntityType(RlE);
        //    }
        //}
        #endregion
        #region Url Authrization
        public bool IsRoleAuthorizedForUrl(DataContracts.Admin.DC_RoleAuthorizedForUrl RAForUrl)
        {
            using (BusinessLayer.BL_Admin obj = new BL_Admin())
            {
                return obj.IsRoleAuthorizedForUrl(RAForUrl);
            }
        }

        #endregion
        #region Entity User Tagging
        public DC_Message AddUpdateUserEntity(DataContracts.Admin.DC_UserEntity UE)
        {
            using (BusinessLayer.BL_Admin obj = new BL_Admin())
            {
                return obj.AddUpdateUserEntity(UE);
            }
        }
        public IList<DataContracts.Admin.DC_EntityType> GetEntityType()
        {
            using (BusinessLayer.BL_Admin obj = new BL_Admin())
            {
                return obj.GetEntityType();
            }
        }
        public IList<DataContracts.Admin.DC_EntityDetails> GetEntity(DataContracts.Admin.DC_EntityDetails ED)
        {
            using (BusinessLayer.BL_Admin obj = new BL_Admin())
            {
                return obj.GetEntity(ED);
            }
        }
        public DataContracts.Admin.DC_UserEntity GetUserEntityDetails(DataContracts.Admin.DC_UserEntity ED)
        {
            using (BusinessLayer.BL_Admin obj = new BL_Admin())
            {
                return obj.GetUserEntityDetails(ED);
            }
        }
        #endregion

        #region UserManagement
        public IList<DataContracts.Admin.DC_UserDetails> GetAllUsers(string PageNo, string PageSize, string ApplicationId)
        {
            using (BusinessLayer.BL_Admin obj = new BL_Admin())
            {
                return obj.GetAllUsers(PageNo, PageSize, ApplicationId);
            }
        }

        public DataContracts.DC_Message UsersSoftDelete(DataContracts.Admin.DC_UserDetails _objUserDetails)
        {
            using (BusinessLayer.BL_Admin obj = new BL_Admin())
            {
                return obj.UsersSoftDelete(_objUserDetails);
            }
        }
        #endregion

        #region Application Management
        public IList<DataContracts.Admin.DC_ApplicationMgmt> GetAllApplication(string PageNo, string PageSize)
        {
            using (BusinessLayer.BL_Admin obj = new BL_Admin())
            {
                return obj.GetAllApplication(PageNo, PageSize);
            }
        }

        public string GetApplicationName(string username)
        {
            using (BusinessLayer.BL_Admin obj = new BL_Admin())
            {
                return obj.GetApplicationName(username);
            }
        }


        public DataContracts.DC_Message AddUpdateApplication(DataContracts.Admin.DC_ApplicationMgmt apmgmt)
        {
            using (BusinessLayer.BL_Admin obj = new BL_Admin())
            {
                return obj.AddUpdateApplication(apmgmt);
            }
        }
        #endregion

    }
}