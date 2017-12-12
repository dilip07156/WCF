using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.ServiceModel.Web;
using System.ServiceModel;
//using ASP.NET_Identity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;
using System.Security.Claims;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using DataContracts;
using log4net;
using DataContracts.Admin;

namespace DataLayer
{
    public class DL_Admin : IDisposable
    {
        public void Dispose()
        {
        }

        #region Site Map
        public IList<DataContracts.Admin.DC_SiteMap> GetSiteMapMaster(int ID, string applicationID)
        {
            try
            {
                Guid _guidApplicationID = Guid.Empty;
                if (applicationID != "0")  //Default Zero
                    _guidApplicationID = Guid.Parse(applicationID);

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from sm in context.SiteMaps
                                 join smpt in context.SiteMaps on sm.Parent equals smpt.ID into smpar
                                 from smparval in smpar.DefaultIfEmpty()
                                 where sm.ID == (ID == 0 ? sm.ID : ID) && sm.applicationId == (_guidApplicationID == Guid.Empty ? sm.applicationId : _guidApplicationID)
                                 orderby sm.ID
                                 select new DataContracts.Admin.DC_SiteMap
                                 {
                                     SiteMap_ID = sm.SiteMap_ID,
                                     ID = sm.ID,
                                     Create_Date = sm.Create_Date,
                                     Create_User = sm.Create_User,
                                     Description = sm.Description,
                                     Edit_Date = sm.Edit_Date,
                                     Edit_User = sm.Edit_User,
                                     IsSiteMapNode = (sm.IsSiteMapNode ?? false),
                                     IsActive = (sm.IsActive ?? true),
                                     ParentID = sm.Parent,
                                     ParentTitle = smparval.Title,
                                     Roles = sm.Roles,
                                     Title = sm.Title,
                                     Url = sm.Url,
                                     ApplicationID = sm.applicationId
                                 };
                    return search.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching sitemap master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Admin.DC_SiteMap> GetSiteMapMasterByUserRole(string UserName)
        {
            try
            {
                using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
                {
                    string userid = userManager.FindByName(UserName).Id;
                    IList<string> roleNames = userManager.GetRoles(userid);


                    List<DataContracts.Admin.DC_SiteMap> objSiteMap = new List<DataContracts.Admin.DC_SiteMap>();
                    Guid applicationserarch = Guid.Empty;
                    using (ConsumerEntities context = new ConsumerEntities())
                    {

                        var appsearch = (from user in context.AspNetUsers where user.UserName == UserName select user.applicationid).FirstOrDefault();
                        applicationserarch = ((appsearch.HasValue) ? appsearch.Value : Guid.Empty);

                        foreach (string role in roleNames)
                        {
                            //Get application ID for the role


                            var search = from sm in context.SiteMaps
                                         join smpt in context.SiteMaps on sm.Parent equals smpt.ID into smpar
                                         from smparval in smpar.DefaultIfEmpty()
                                         where (((sm.Roles.StartsWith(role) || sm.Roles.EndsWith(role) || sm.Roles.Contains(role))
                                         && (sm.IsActive ?? true)) || sm.Parent == null) && (sm.IsSiteMapNode ?? false) == true && sm.applicationId == applicationserarch
                                         orderby sm.ID
                                         select new DataContracts.Admin.DC_SiteMap
                                         {
                                             SiteMap_ID = sm.SiteMap_ID,
                                             ID = sm.ID,
                                             Create_Date = sm.Create_Date,
                                             Create_User = sm.Create_User,
                                             Description = sm.Description,
                                             Edit_Date = sm.Edit_Date,
                                             Edit_User = sm.Edit_User,
                                             IsSiteMapNode = (sm.IsSiteMapNode ?? false),
                                             IsActive = (sm.IsActive ?? true),
                                             ParentID = sm.Parent,
                                             ParentTitle = smparval.Title,
                                             Roles = sm.Roles,
                                             Title = sm.Title,
                                             ApplicationID = sm.applicationId,
                                             Url = sm.Url
                                         };
                            objSiteMap.AddRange(search);

                        }
                        var resultDistinct = objSiteMap.Distinct(new Comparer()).ToList();
                        var resultOrdered = (from r in resultDistinct orderby r.ID select r).ToList();
                        return resultOrdered;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching sitemap master by user role", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        //Calls to get distinct sitemap 
        public class Comparer : IEqualityComparer<DataContracts.Admin.DC_SiteMap>
        {
            public bool Equals(DataContracts.Admin.DC_SiteMap x, DataContracts.Admin.DC_SiteMap y)
            {
                return x.SiteMap_ID == y.SiteMap_ID;
            }

            public int GetHashCode(DataContracts.Admin.DC_SiteMap obj)
            {
                return (int)obj.ID;
            }
        }

        public bool UpdateSiteMapMaster(DataContracts.Admin.DC_SiteMap SM)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from sm in context.SiteMaps
                                  where sm.SiteMap_ID == SM.SiteMap_ID
                                  select sm).FirstOrDefault();
                    if (search != null)
                    {

                        //Check removed or add role
                        List<string> existingRole = new List<string>();
                        List<string> newRole = new List<string>(); ;

                        if (search.Roles != null)
                        {
                            foreach (var searchitem in Convert.ToString(search.Roles).Split(','))
                            {
                                existingRole.Add(searchitem.ToString());
                            }
                        }

                        if (SM.Roles != null)
                        {
                            foreach (var itemRole in Convert.ToString(SM.Roles).Split(','))
                            {
                                newRole.Add(itemRole.ToString());
                            }
                        }

                        bool blnRemoved = false;
                        List<string> removedRole = new List<string>();
                        bool blnAdded = false;
                        List<string> AddedRole = new List<string>();

                        #region if Removed

                        foreach (var itemsearch in existingRole)
                        {
                            if (newRole.Contains(itemsearch))
                            {
                                if (!blnRemoved)
                                    blnRemoved = false;
                            }
                            else
                            {
                                blnRemoved = true;
                                removedRole.Add(itemsearch);
                            }
                        }
                        if (blnRemoved)
                        {
                            //If role removed
                            //Checking childs
                            foreach (var item in removedRole)
                            {
                                var child = (from ch in context.SiteMaps
                                             where ch.Parent == SM.ID
                                             select ch).ToList();
                                if (child.Count > 0) //have child so remove roles
                                {
                                    foreach (var ch in child)
                                    {
                                        List<string> _lstRole = ch.Roles.ToString().Split(',').ToList();
                                        _lstRole.Remove(item.ToString());
                                        ch.Roles = String.Join(",", _lstRole);
                                    }
                                }
                            }
                        }

                        #endregion

                        #region if Added
                        foreach (var itemSm in newRole)
                        {
                            if (existingRole.Contains(itemSm))
                            {
                                if (!blnAdded)
                                    blnAdded = false;
                            }
                            else
                            {
                                blnAdded = true;
                                AddedRole.Add(itemSm);
                            }
                        }

                        if (blnAdded)
                        {
                            //Role add
                            //File parents for the 
                            foreach (var adrl in AddedRole)
                            {
                                var parent = (from ch in context.SiteMaps
                                              join pt in context.SiteMaps on ch.Parent equals pt.ID
                                              where ch.ID == SM.ID
                                              select pt).ToList();

                                if (parent.Count > 0)
                                {
                                    foreach (var pt in parent)
                                    {
                                        List<string> _lstRole = new List<string>();
                                        if (Convert.ToString(pt.Roles) != null)
                                        {
                                            _lstRole = pt.Roles.ToString().Split(',').ToList();
                                        }
                                        _lstRole.Add(adrl.ToString());
                                        pt.Roles = String.Join(",", _lstRole);
                                    }
                                }

                            }
                        }
                        #endregion

                        if (SM.IsActive != (search.IsActive ?? true))
                        {
                            search.IsActive = SM.IsActive;
                            search.IsSiteMapNode = SM.IsSiteMapNode;
                            search.Edit_Date = SM.Edit_Date;
                            search.Edit_User = SM.Edit_User;
                            search.applicationId = SM.ApplicationID;
                        }
                        else
                        {
                            search.Description = SM.Description;
                            search.Edit_Date = SM.Edit_Date;
                            search.Edit_User = SM.Edit_User;
                            //SiteMapNode active Or Inactive Check
                            if (!SM.IsSiteMapNode)
                                SetSiteMapNodeActiveInactiveForParent(SM);
                            else
                                SetSiteMapNodeActiveInactiveForChild(SM);
                            search.IsSiteMapNode = SM.IsSiteMapNode;
                            search.IsActive = SM.IsActive;
                            search.Parent = SM.ParentID;
                            search.Roles = SM.Roles;
                            search.Title = SM.Title;
                            search.Url = SM.Url;
                            search.applicationId = SM.ApplicationID;
                        }
                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating sitemap master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        private void SetSiteMapNodeActiveInactiveForChild(DC_SiteMap sM)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from sm in context.SiteMaps
                                  where sm.ID == sM.ParentID
                                  select sm).FirstOrDefault();
                    if(search != null)
                    {
                        //Check parent node
                        int _intparent = Convert.ToInt32(search.ID);
                        var parent = (from sm in context.SiteMaps
                                      where sm.ID == _intparent
                                      select sm).FirstOrDefault();
                        if (!Convert.ToBoolean(parent.IsSiteMapNode))
                        {
                            parent.IsSiteMapNode = sM.IsSiteMapNode;
                            context.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void SetSiteMapNodeActiveInactiveForParent(DC_SiteMap sM)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from sm in context.SiteMaps
                                  where sm.Parent == sM.ID
                                  select sm).ToList();
                    if (search != null && search.Count > 0)
                    {
                        foreach(var item in search)
                        {
                            item.IsSiteMapNode = sM.IsSiteMapNode;
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating sitemap master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool AddSiteMapMaster(DataContracts.Admin.DC_SiteMap SM)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    int? ID = context.SiteMaps.Max(u => (int?)u.ID);

                    SiteMap newObj = new SiteMap();
                    newObj.SiteMap_ID = SM.SiteMap_ID;
                    newObj.ID = (ID ?? 0) + 1;
                    newObj.Description = SM.Description;
                    newObj.Create_Date = SM.Create_Date;
                    newObj.Create_User = SM.Create_User;
                    newObj.IsSiteMapNode = SM.IsSiteMapNode;
                    newObj.IsActive = SM.IsActive;
                    newObj.Parent = SM.ParentID;
                    newObj.Roles = SM.Roles;
                    newObj.Title = SM.Title;
                    newObj.Url = SM.Url;
                    newObj.applicationId = SM.ApplicationID;

                    if (Convert.ToString(SM.ParentID) != null)
                    {
                        //Have parents
                        var parent = (from prnt in context.SiteMaps
                                      where prnt.ID == SM.ParentID
                                      select prnt).ToList();
                        if (parent.Count > 0)
                        {
                            foreach (var pt in parent)
                            {
                                List<string> _lstRole = pt.Roles.ToString().Split(',').ToList();
                                foreach (var item in SM.Roles.ToString().Split(','))
                                {
                                    if (!_lstRole.Contains(item.ToString()))
                                    {
                                        _lstRole.Add(item.ToString());
                                    }
                                }
                                pt.Roles = String.Join(",", _lstRole);
                            }
                        }
                    }
                    context.SiteMaps.Add(newObj);
                    context.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding sitemap node", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        #endregion

        #region Roles
        public List<DataContracts.Admin.DC_Roles> GetAllRole(string applicationID, int PageNo, int PageSize)
        {
            try
            {
                Guid _guidApplicationID = Guid.Empty;
                if (applicationID != "0")  //Default Zero
                    _guidApplicationID = Guid.Parse(applicationID);
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.AspNetRoles where a.ApplicationID == (_guidApplicationID == Guid.Empty ? a.ApplicationID : _guidApplicationID) select a;

                    int total;

                    total = search.Count();
                    var skip = PageSize * PageNo;
                    if (PageSize == 0)
                        PageSize = total;



                    var listRoles = (from lst in search
                                     join ent in context.m_EntityType on lst.EntityTypeID equals ent.EnityTypeID.ToString() into roles
                                     from lstrole in roles.DefaultIfEmpty()
                                     orderby lst.Name
                                     select new DataContracts.Admin.DC_Roles
                                     {
                                         RoleID = lst.Id,
                                         RoleName = lst.Name,
                                         //EntityTypeID = lst.EntityTypeID,
                                         //EntityType = lstrole.EntityTypeName,
                                         ApplicationID = (lst.ApplicationID.HasValue) ? lst.ApplicationID.Value : Guid.Empty,
                                         TotalRecords = total

                                     }).Skip(skip).Take(PageSize == 0 ? total : PageSize);

                    return listRoles.ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching all roles", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public bool AddUpdateRoleEntityType(DataContracts.Admin.DC_Roles RlE)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var role = (from rl in context.AspNetRoles
                                where rl.Id == RlE.RoleID /*&& rl.EntityTypeID == RlE.EntityTypeID*/
                                select rl).FirstOrDefault();

                    if (role != null)
                    {
                        //role.EntityTypeID = Convert.ToString(RlE.EntityTypeID);
                        role.ApplicationID = RlE.ApplicationID;
                    }

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while add update role entity type", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        //public List<DataContracts.Admin.DC_Roles> GetAllRoleByEntityType(DataContracts.Admin.DC_Roles RlE)
        //{
        //    try
        //    {
        //        using (ConsumerEntities context = new ConsumerEntities())
        //        {
        //            int entityTypeID = Convert.ToInt32(RlE.EntityTypeID);
        //            var role = (from lst in context.AspNetRoles
        //                        join ent in context.m_EntityType on lst.EntityTypeID equals ent.EnityTypeID.ToString()
        //                        where ent.EnityTypeID == entityTypeID
        //                        orderby lst.Name
        //                        select new DataContracts.Admin.DC_Roles
        //                        {
        //                            RoleID = lst.Id,
        //                            RoleName = lst.Name,
        //                            EntityTypeID = lst.EntityTypeID,
        //                            EntityType = ent.EntityTypeName,
        //                        });

        //            return role.ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while checking role existing or not ", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
        //    }
        //}
        //public List<DataContracts.Admin.DC_Roles> GetAllRoleByApplication(DataContracts.Admin.DC_Roles RlE, int PageNo, int PageSize)
        //{
        //    try
        //    {
        //        using (ConsumerEntities context = new ConsumerEntities())
        //        {
        //            var search = from a in context.AspNetRoles where a.ApplicationID == RlE.ApplicationID select a;

        //            int total;

        //            total = search.Count();
        //            var skip = PageSize * PageNo;
        //            if (PageSize == 0)
        //                PageSize = total;

        //            var listRoles = (from lst in search
        //                             join ent in context.m_EntityType on lst.EntityTypeID equals ent.EnityTypeID.ToString()
        //                             orderby lst.Name
        //                             select new DataContracts.Admin.DC_Roles
        //                             {
        //                                 RoleID = lst.Id,
        //                                 RoleName = lst.Name,
        //                                 EntityTypeID = lst.EntityTypeID,
        //                                 EntityType = ent.EntityTypeName,
        //                                 ApplicationID = lst.ApplicationID,
        //                                 TotalRecords = total

        //                             }).Skip(skip).Take(PageSize);

        //            return listRoles.ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while checking role existing or not ", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
        //    }
        //}
        public bool IsRoleExist(DataContracts.Admin.DC_Roles RlE)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var role = (from rl in context.AspNetRoles
                                where rl.Id == RlE.RoleID && rl.ApplicationID == RlE.ApplicationID /*&& rl.EntityTypeID == RlE.EntityTypeID*/
                                select rl).FirstOrDefault();

                    if (role != null)
                        return true;

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while checking role existing or not ", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        #endregion

        #region Url Authrization
        public bool IsRoleAuthorizedForUrl(DataContracts.Admin.DC_RoleAuthorizedForUrl RAForUrl)
        {
            try
            {
                bool IsRoleAuthorizedForUrl = false;
                List<string> UrlRole = new List<string>();
                using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
                {



                    string userid = userManager.FindByName(RAForUrl.user).Id;
                    IList<string> roleNames = userManager.GetRoles(userid);
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        //authenticate user 
                        string userapplication = Convert.ToString((from x in context.AspNetUsers where x.UserName == RAForUrl.user select x.applicationid).FirstOrDefault());
                        string urlapplication = Convert.ToString((from url in context.SiteMaps where url.Url == RAForUrl.url select url.applicationId).FirstOrDefault());

                        if (urlapplication.ToLower() != userapplication.ToLower())
                            return IsRoleAuthorizedForUrl;


                        var search = (from sm in context.SiteMaps
                                      where sm.Url == RAForUrl.Url
                                      select sm).FirstOrDefault();

                        if (search != null)
                        {
                            UrlRole = Convert.ToString(search.Roles).Split(',').ToList();
                        }

                        //authenticate roles
                        foreach (var itemforapp in UrlRole)
                        {
                            if (Convert.ToString((from rl in context.AspNetRoles where rl.Name == itemforapp select rl.ApplicationID).FirstOrDefault()) != userapplication)
                                return IsRoleAuthorizedForUrl;
                        }
                        if (UrlRole.Count > 0 && roleNames.Count > 0)
                        {
                            foreach (var item in UrlRole)
                            {
                                foreach (var itemAccessrole in roleNames)
                                {
                                    if (item.ToString() == itemAccessrole)
                                    {
                                        IsRoleAuthorizedForUrl = true;
                                        return IsRoleAuthorizedForUrl;
                                    }
                                }
                            }
                        }
                    }
                }
                return IsRoleAuthorizedForUrl;
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching all roles", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        #endregion

        #region User Entity Tagging
        public IList<DataContracts.Admin.DC_EntityType> GetEntityType()
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from mE in context.m_EntityType
                                 select new DataContracts.Admin.DC_EntityType
                                 {
                                     EnityTypeID = mE.EnityTypeID,
                                     EntityTypeName = mE.EntityTypeName
                                 };
                    return search.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching sitemap master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public DC_Message AddUpdateUserEntity(DataContracts.Admin.DC_UserEntity UE)
        {
            DC_Message _msg = new DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //var search = context.UserEntities.Find();
                    var search = (from x in context.UserEntities where x.UserID == UE.UserID select x).FirstOrDefault();
                    if (search != null)
                    {
                        search.EntityTypeID = UE.EntityTypeID;
                        search.EntityID = UE.EntityID;
                    }
                    else
                    {
                        UserEntity _UE = new UserEntity();
                        _UE.ID = Convert.ToString(Guid.NewGuid());
                        _UE.UserID = UE.UserID;
                        _UE.EntityID = UE.EntityID;
                        _UE.EntityTypeID = UE.EntityTypeID;
                        context.UserEntities.Add(_UE);

                    }

                    //Manager Id Update
                    string userid = Convert.ToString(UE.UserID);
                    var searchuser = (from user in context.AspNetUsers
                                      where user.Id == userid
                                      select user).FirstOrDefault();
                    if (searchuser != null)
                    {
                        searchuser.ManagerId = UE.ManagerID;
                        if (searchuser.applicationid != null)
                        {
                            searchuser.Edit_User = UE.Edit_User;
                            searchuser.Edit_Date = UE.Edit_Date;
                            _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                            searchuser.Create_User = UE.Create_User;
                            searchuser.Create_Date = UE.Create_Date;
                        }
                        searchuser.applicationid = UE.ApplicationId;
                        searchuser.IsActive = true;
                    }
                    if (context.SaveChanges() == 1)
                    {
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                    }
                    else
                    {
                        _msg.StatusMessage = ReadOnlyMessage.strFailed;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                    }
                    return _msg;

                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating user entity mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        enum EntityType { Blank, MDM, Clients, Org, Suppliers };
        public IList<DataContracts.Admin.DC_EntityDetails> GetEntity(DataContracts.Admin.DC_EntityDetails ED)
        {
            try
            {
                List<DataContracts.Admin.DC_EntityDetails> _lst = new List<DataContracts.Admin.DC_EntityDetails>();
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (ED.EntityTypeID == Convert.ToInt32(EntityType.Clients)) //Client Table
                    {
                        var search = from client in context.Clients
                                     select new DataContracts.Admin.DC_EntityDetails
                                     {
                                         EntityID = client.Client_Id,
                                         EntityName = client.ClientName,
                                         EntityTypeID = ED.EntityTypeID

                                     };
                        return search.ToList();
                    }
                    else if (ED.EntityTypeID == Convert.ToInt32(EntityType.Org)) //Org Table
                    {
                        var search = from Org in context.Organisation_Company
                                     select new DataContracts.Admin.DC_EntityDetails
                                     {
                                         EntityID = Org.Organisation_Company_Id,
                                         EntityName = Org.Name,
                                         EntityTypeID = ED.EntityTypeID

                                     };
                        return search.ToList();

                    }
                    else if (ED.EntityTypeID == Convert.ToInt32(EntityType.Suppliers)) //Suppliers Table
                    {
                        var search = from sup in context.Supplier
                                     select new DataContracts.Admin.DC_EntityDetails
                                     {
                                         EntityID = sup.Supplier_Id,
                                         EntityName = sup.Name,
                                         EntityTypeID = ED.EntityTypeID

                                     };
                        return search.ToList();
                    }
                    else
                        return _lst;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataContracts.Admin.DC_UserEntity GetUserEntityDetails(DataContracts.Admin.DC_UserEntity ED)
        {
            try
            {
                DataContracts.Admin.DC_UserEntity _obj = new DataContracts.Admin.DC_UserEntity();
                var userid = Convert.ToString(ED.UserID);
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var result1 = (from ue in context.AspNetUsers where ue.Id == userid select ue).FirstOrDefault();
                    if (result1 != null)
                    {
                        _obj.UserID = ED.UserID;
                        _obj.ManagerID = result1.ManagerId;
                    }
                    var result = (from ed in context.UserEntities
                                  where ed.UserID == ED.UserID
                                  select ed).FirstOrDefault();
                    if (result != null)
                    {
                        _obj.EntityID = result.EntityID;
                        _obj.EntityTypeID = result.EntityTypeID;
                    }

                    return _obj;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while getting user entity mapping data", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError }); throw;
            }
        }

        #endregion

        #region User Management
        public List<DataContracts.Admin.DC_UserDetails> GetAllUsers(int PageNo, int PageSize, string ApplicationId)
        {
            try
            {
                Guid _guidApplicationID = Guid.Empty;
                if (ApplicationId != "0")  //Default Zero
                    _guidApplicationID = Guid.Parse(ApplicationId);
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.AspNetUsers
                                 where a.applicationid == (_guidApplicationID == Guid.Empty ? a.applicationid : _guidApplicationID)
                                 select a;

                    int total;

                    total = search.Count();
                    var skip = PageSize * PageNo;
                    if (PageSize == 0)
                        PageSize = total;



                    var listUsers = (from lstusers in search
                                     join ue in context.UserEntities on lstusers.Id equals ue.UserID.ToString()
                                     join ent in context.m_EntityType on ue.EntityTypeID equals ent.EnityTypeID
                                     orderby lstusers.UserName
                                     select new DataContracts.Admin.DC_UserDetails
                                     {
                                         Userid = lstusers.Id,
                                         UserName = lstusers.UserName,
                                         EntityTypeID = ue.EntityTypeID,
                                         EntityType = ent.EntityTypeName,
                                         IsActive = lstusers.IsActive,
                                         Create_User = lstusers.Create_User,
                                         Create_Date = lstusers.Create_Date,
                                         Edit_Date = lstusers.Edit_Date,
                                         Edit_User = lstusers.Edit_User,
                                         ApplicationId = lstusers.applicationid.HasValue ? lstusers.applicationid.Value : Guid.Empty,
                                         TotalRecords = total

                                     }).Skip(skip).Take(PageSize);

                    return listUsers.ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching all users", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public DataContracts.DC_Message UsersSoftDelete(DataContracts.Admin.DC_UserDetails _objUserDetails)
        {
            DC_Message _msg = new DC_Message();
            using (ConsumerEntities context = new ConsumerEntities())
            {
                var result = context.AspNetUsers.Find(_objUserDetails.Userid);

                result.IsActive = _objUserDetails.IsActive;
                result.Edit_Date = DateTime.Now;
                result.Edit_User = _objUserDetails.Edit_User;
                if (context.SaveChanges() == 1)
                {
                    _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                }
                else
                {
                    _msg.StatusMessage = ReadOnlyMessage.strFailed;
                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                }
            }
            return _msg;

        }
        #endregion

        #region Application Management
        public List<DataContracts.Admin.DC_ApplicationMgmt> GetAllApplication(int PageNo, int PageSize)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Applications select a;

                    int total;

                    total = search.Count();
                    var skip = PageSize * PageNo;
                    if (PageSize == 0)
                        PageSize = total;



                    var listApplication = (from lstapplication in search
                                           orderby lstapplication.ApplicationName
                                           select new DataContracts.Admin.DC_ApplicationMgmt
                                           {
                                               ApplicationId = lstapplication.ApplicationId,
                                               ApplicationName = lstapplication.ApplicationName,
                                               Description = lstapplication.Description,
                                               TotalRecords = total
                                           }).Skip(skip).Take(PageSize);
                    return listApplication.ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching all users", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public string GetApplicationName(string username)
        {
            try
            {
                string strapplicationname = string.Empty;
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from a in context.AspNetUsers
                                  join b in context.Applications on a.applicationid equals b.ApplicationId
                                  where a.UserName == username
                                  select b.ApplicationName).FirstOrDefault();
                    if (search != null)
                    {
                        strapplicationname = search.ToString();
                    }
                    return strapplicationname;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching all users", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DataContracts.DC_Message AddUpdateApplication(DataContracts.Admin.DC_ApplicationMgmt apmgmt)
        {
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //Duplicate Check
                    var applicationduplicate = (from app in context.Applications
                                                where (app.ApplicationName.ToLower() == apmgmt.ApplicationName.ToLower() && app.ApplicationId != apmgmt.ApplicationId)
                                                select app).FirstOrDefault();

                    if (applicationduplicate != null)
                    {
                        _msg.StatusMessage = ReadOnlyMessage.strAlreadyExist;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Warning;
                    }
                    else
                    {
                        var application = (from app in context.Applications
                                           where app.ApplicationId == apmgmt.ApplicationId
                                           select app).FirstOrDefault();

                        if (application != null)
                        {
                            application.ApplicationName = Convert.ToString(apmgmt.ApplicationName);
                            application.Description = Convert.ToString(apmgmt.Description);
                            _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            Application _ap = new Application();
                            _ap.ApplicationId = apmgmt.ApplicationId;
                            _ap.ApplicationName = apmgmt.ApplicationName;
                            _ap.Description = apmgmt.Description;
                            context.Applications.Add(_ap);
                            _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }

                        context.SaveChanges();
                    }
                    return _msg;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while add update role entity type", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }


        #endregion

    }

    #region Identity Classes
    public class ApplicationUser : IdentityUser
    {
        public ClaimsIdentity GenerateUserIdentity(ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = manager.CreateIdentity(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            return Task.FromResult(GenerateUserIdentity(manager));
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
    #endregion

}
