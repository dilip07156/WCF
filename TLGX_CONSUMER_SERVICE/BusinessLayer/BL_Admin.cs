using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace BusinessLayer
{
    public class BL_Admin : IDisposable
    {
        public void Dispose()
        {
        }

        #region Site Map
        public IList<DataContracts.Admin.DC_SiteMap> GetSiteMapMaster(string ID,string applicationID)
        {
            int iID;

            if (!int.TryParse(ID, out iID))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Admin obj = new DataLayer.DL_Admin())
                {
                    return obj.GetSiteMapMaster(iID,applicationID);
                }
            }

        }

        public List<DataContracts.Admin.DC_SiteMap> GetSiteMapMasterByUserRole(string UserName)
        {
            using (DataLayer.DL_Admin obj = new DataLayer.DL_Admin())
            {
                return obj.GetSiteMapMasterByUserRole(UserName);
            }
        }

        public bool UpdateSiteMapMaster(DataContracts.Admin.DC_SiteMap SM)
        {
            using (DataLayer.DL_Admin obj = new DataLayer.DL_Admin())
            {
                return obj.UpdateSiteMapMaster(SM);
            }
        }

        public bool AddSiteMapMaster(DataContracts.Admin.DC_SiteMap SM)
        {
            using (DataLayer.DL_Admin obj = new DataLayer.DL_Admin())
            {
                return obj.AddSiteMapMaster(SM);
            }
        }
        #endregion

        #region Roles
        public List<DataContracts.Admin.DC_Roles> GetAllRole(string applicationID, string PageNo, string PageSize)
        {
            int iPageNo;
            int iPageSize;
            if (!int.TryParse(PageNo, out iPageNo) || !int.TryParse(PageSize, out iPageSize))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Admin obj = new DataLayer.DL_Admin())
                {
                    return obj.GetAllRole(applicationID, iPageNo, iPageSize);
                }
            }

        }
        public bool AddUpdateRoleEntityType(DataContracts.Admin.DC_Roles RlE)
        {
            using (DataLayer.DL_Admin obj = new DataLayer.DL_Admin())
            {
                return obj.AddUpdateRoleEntityType(RlE);
            }
        }
        public bool IsRoleExist(DataContracts.Admin.DC_Roles RlE)
        {
            using (DataLayer.DL_Admin obj = new DataLayer.DL_Admin())
            {
                return obj.IsRoleExist(RlE);
            }
        }
        //public List<DataContracts.Admin.DC_Roles> GetAllRoleByEntityType(DataContracts.Admin.DC_Roles RlE)
        //{
        //    using (DataLayer.DL_Admin obj = new DataLayer.DL_Admin())
        //    {
        //        return obj.GetAllRoleByEntityType(RlE);
        //    }
        //}
        //public List<DataContracts.Admin.DC_Roles> GetAllRoleByApplication(DataContracts.Admin.DC_Roles RlE, string PageNo, string PageSize)
        //{
        //    int iPageNo;
        //    int iPageSize;
        //    if (!int.TryParse(PageNo, out iPageNo) || !int.TryParse(PageSize, out iPageSize))
        //    {
        //        throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
        //    }
        //    else
        //    {
        //        using (DataLayer.DL_Admin obj = new DataLayer.DL_Admin())
        //        {
        //            return obj.GetAllRoleByApplication(RlE,iPageNo, iPageSize);
        //        }
        //    }          
        //}
        #endregion

        #region User EntityTagging
        public IList<DataContracts.Admin.DC_EntityType> GetEntityType()
        {
            using (DataLayer.DL_Admin obj = new DataLayer.DL_Admin())
            {
                return obj.GetEntityType();
            }
        }
        public IList<DataContracts.Admin.DC_EntityDetails> GetEntity(DataContracts.Admin.DC_EntityDetails ED)
        {
            using (DataLayer.DL_Admin obj = new DataLayer.DL_Admin())
            {
                return obj.GetEntity(ED);
            }
        }
        public DataContracts.DC_Message AddUpdateUserEntity(DataContracts.Admin.DC_UserEntity UE)
        {
            using (DataLayer.DL_Admin obj = new DataLayer.DL_Admin())
            {
                return obj.AddUpdateUserEntity(UE);
            }
        }

        public DataContracts.Admin.DC_UserEntity GetUserEntityDetails(DataContracts.Admin.DC_UserEntity UE)
        {
            using (DataLayer.DL_Admin obj = new DataLayer.DL_Admin())
            {
                return obj.GetUserEntityDetails(UE);
            }
        }

        #endregion


        #region Url Authrization
        public bool IsRoleAuthorizedForUrl(DataContracts.Admin.DC_RoleAuthorizedForUrl RAForUrl)
        {
            using (DataLayer.DL_Admin obj = new DataLayer.DL_Admin())
            {
                return obj.IsRoleAuthorizedForUrl(RAForUrl);
            }
        }

        #endregion

        #region User Management
        public List<DataContracts.Admin.DC_UserDetails> GetAllUsers(string PageNo, string PageSize, string ApplicationId)
        {
            int iPageNo;
            int iPageSize;
            if (!int.TryParse(PageNo, out iPageNo) || !int.TryParse(PageSize, out iPageSize))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Admin obj = new DataLayer.DL_Admin())
                {
                    return obj.GetAllUsers(iPageNo, iPageSize,ApplicationId);
                }
            }

        }
        public DataContracts.DC_Message UsersSoftDelete(DataContracts.Admin.DC_UserDetails _objUserDetails)
        {
            using (DataLayer.DL_Admin obj = new DataLayer.DL_Admin())
            {
                return obj.UsersSoftDelete(_objUserDetails);
            }
        }
        #endregion


        #region Application Management
        public List<DataContracts.Admin.DC_ApplicationMgmt> GetAllApplication(string PageNo, string PageSize)
        {
            int iPageNo;
            int iPageSize;
            if (!int.TryParse(PageNo, out iPageNo) || !int.TryParse(PageSize, out iPageSize))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Admin obj = new DataLayer.DL_Admin())
                {
                    return obj.GetAllApplication(iPageNo, iPageSize);
                }
            }

        }
        public string GetApplicationName(string username)
        {
            using (DataLayer.DL_Admin obj = new DataLayer.DL_Admin())
            {
                return obj.GetApplicationName(username);
            }
        }
        public DataContracts.DC_Message AddUpdateApplication(DataContracts.Admin.DC_ApplicationMgmt apmgmt)
        {
            using (DataLayer.DL_Admin obj = new DataLayer.DL_Admin())
            {
                return obj.AddUpdateApplication(apmgmt);
            }
        }
        #endregion

    }
}
