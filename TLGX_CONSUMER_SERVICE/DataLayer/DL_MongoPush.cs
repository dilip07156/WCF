﻿using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DL_MongoPush : IDisposable
    {
        public void Dispose()
        {

        }

        public void SyncActivityFlavour(Guid Activity_Flavour_id)
        {
            if (Activity_Flavour_id != Guid.Empty)
            {
                DHSVCProxyAsync DHP = new DHSVCProxyAsync();
                string strURI = string.Format(System.Configuration.ConfigurationManager.AppSettings["Sync_Activity_Flavour"], Convert.ToString(Activity_Flavour_id));
                DHP.GetAsync(ProxyFor.SqlToMongo, strURI);
            }

        }

        #region Country

        public DC_Message SyncCountryMaster(Guid Country_Id, string CreatedBy)
        {
            try
            {

                Guid LogId;

                using (ConsumerEntities context = new ConsumerEntities())

                {
                    var iScheduledCount = (from dlr in context.DistributionLayerRefresh_Log
                                           where dlr.Status == "Scheduled" && dlr.Element == "Country" && dlr.Type == "Master"
                                           select true).Count();

                    if (iScheduledCount > 0)
                    {
                        return new DC_Message { StatusMessage = "CountryMaster sync has already been scheduled.", StatusCode = ReadOnlyMessage.StatusCode.Information };
                    }
                    else
                    {

                        LogId = Guid.NewGuid();

                        DataLayer.DistributionLayerRefresh_Log objNew = new DistributionLayerRefresh_Log();
                        objNew.Id = LogId;
                        objNew.Element = "Country";
                        objNew.Type = "Master";
                        objNew.Create_Date = DateTime.Now;
                        objNew.Create_User = CreatedBy;// System.Web.HttpContext.Current.User.Identity.Name;
                        objNew.Status = "Scheduled";
                        context.DistributionLayerRefresh_Log.Add(objNew);
                        context.SaveChanges();
                    }


                    if (Country_Id == Guid.Empty)
                    {
                        using (DHSVCProxyAsync DHP = new DHSVCProxyAsync())
                        {
                            string strURI = string.Format(System.Configuration.ConfigurationManager.AppSettings["Load_CountryMaster"], LogId.ToString());
                            DHP.GetAsync(ProxyFor.SqlToMongo, strURI);
                        }
                    }
                    else
                    {
                        //Code goes here for Indert Update or Delete of a specific country
                    }


                    return new DC_Message { StatusMessage = "CountryMaster sync has been scheduled successfully.", StatusCode = ReadOnlyMessage.StatusCode.Success };
                }
            }
            catch (Exception ex)
            {
                return new DC_Message { StatusMessage = ex.Message, StatusCode = ReadOnlyMessage.StatusCode.Failed };
            }

        }


        public DC_Message SyncCountryMapping(Guid Country_Id, string CreatedBy)
        {
            try
            {
                Guid LogId;

                using (ConsumerEntities context = new ConsumerEntities())

                {
                    var iScheduledCount = (from dlr in context.DistributionLayerRefresh_Log
                                           where dlr.Status == "Scheduled" && dlr.Element == "Country" && dlr.Type == "Mapping"
                                           select true).Count();

                    if (iScheduledCount > 0)
                    {
                        return new DC_Message { StatusMessage = "CountryMapping sync has already been scheduled.", StatusCode = ReadOnlyMessage.StatusCode.Information };
                    }
                    else
                    {
                        LogId = Guid.NewGuid();

                        DataLayer.DistributionLayerRefresh_Log objNew = new DistributionLayerRefresh_Log();
                        objNew.Id = LogId;
                        objNew.Element = "Country";
                        objNew.Type = "Mapping";
                        objNew.Create_Date = DateTime.Now;
                        objNew.Create_User = CreatedBy;// System.Web.HttpContext.Current.User.Identity.Name;
                        objNew.Status = "Scheduled";
                        context.DistributionLayerRefresh_Log.Add(objNew);
                        context.SaveChanges();
                    }
                    if (Country_Id == Guid.Empty)
                    {
                        using (DHSVCProxyAsync DHP = new DHSVCProxyAsync())
                        {
                            string strURI = string.Format(System.Configuration.ConfigurationManager.AppSettings["Load_CountryMapping"], LogId.ToString());
                            DHP.GetAsync(ProxyFor.SqlToMongo, strURI);
                        }
                    }
                    else
                    {
                        //Code goes here for Indert Update or Delete of a specific country
                    }
                    
                    return new DC_Message { StatusMessage = "CountryMapping sync has been scheduled successfully.", StatusCode = ReadOnlyMessage.StatusCode.Success };
                }
            }
            catch (Exception ex)
            {
                return new DC_Message { StatusMessage = ex.Message, StatusCode = ReadOnlyMessage.StatusCode.Failed };
            }


        }
        #endregion

        #region city
        public DC_Message SyncCityMapping(Guid City_Id, string CreatedBy)
        {
            try
            {
                Guid LogId;

                using (ConsumerEntities context = new ConsumerEntities())

                {
                    var iScheduledCount = (from dlr in context.DistributionLayerRefresh_Log
                                           where dlr.Status == "Scheduled" && dlr.Element == "City" && dlr.Type == "Mapping"
                                           select true).Count();

                    if (iScheduledCount > 0)
                    {
                        return new DC_Message { StatusMessage = "CityMapping sync has already been scheduled.", StatusCode = ReadOnlyMessage.StatusCode.Information };
                    }
                    else
                    {
                        LogId = Guid.NewGuid();

                        DataLayer.DistributionLayerRefresh_Log objNew = new DistributionLayerRefresh_Log();
                        objNew.Id = LogId;
                        objNew.Element = "City";
                        objNew.Type = "Mapping";
                        objNew.Create_Date = DateTime.Now;
                        objNew.Create_User = CreatedBy;// System.Web.HttpContext.Current.User.Identity.Name;
                        objNew.Status = "Scheduled";
                        context.DistributionLayerRefresh_Log.Add(objNew);
                        context.SaveChanges();
                    }
                    if (City_Id == Guid.Empty)
                    {
                        using (DHSVCProxyAsync DHP = new DHSVCProxyAsync())
                        {
                            string strURI = string.Format(System.Configuration.ConfigurationManager.AppSettings["Load_CityMapping"], LogId.ToString());
                            DHP.GetAsync(ProxyFor.SqlToMongo, strURI);
                        }
                    }
                    else
                    {
                        //Code goes here for Indert Update or Delete of a specific country
                    }

                    return new DC_Message { StatusMessage = "CityMapping sync has been scheduled successfully.", StatusCode = ReadOnlyMessage.StatusCode.Success };
                }
            }
            catch (Exception ex)
            {
                return new DC_Message { StatusMessage = ex.Message, StatusCode = ReadOnlyMessage.StatusCode.Failed };
            }
       }


        public DC_Message SyncCityMaster(Guid City_Id, string CreatedBy)
        {
            try
            {
                Guid LogId;

                using (ConsumerEntities context = new ConsumerEntities())

                {
                    var iScheduledCount = (from dlr in context.DistributionLayerRefresh_Log
                                           where dlr.Status == "Scheduled" && dlr.Element == "City" && dlr.Type == "Master"
                                           select true).Count();

                    if (iScheduledCount > 0)
                    {
                        return new DC_Message { StatusMessage = "CityMaster sync has already been scheduled.", StatusCode = ReadOnlyMessage.StatusCode.Information };
                    }
                    else
                    {
                        LogId = Guid.NewGuid();

                        DataLayer.DistributionLayerRefresh_Log objNew = new DistributionLayerRefresh_Log();
                        objNew.Id = LogId;
                        objNew.Element = "City";
                        objNew.Type = "Master";
                        objNew.Create_Date = DateTime.Now;
                        objNew.Create_User = CreatedBy;// System.Web.HttpContext.Current.User.Identity.Name;
                        objNew.Status = "Scheduled";
                        context.DistributionLayerRefresh_Log.Add(objNew);
                        context.SaveChanges();
                    }
                    if (City_Id == Guid.Empty)
                    {
                        using (DHSVCProxyAsync DHP = new DHSVCProxyAsync())
                        {
                            string strURI = string.Format(System.Configuration.ConfigurationManager.AppSettings["Load_CityMaster"], LogId.ToString());
                            DHP.GetAsync(ProxyFor.SqlToMongo, strURI);
                        }
                    }
                    else
                    {
                        //Code goes here for Indert Update or Delete of a specific country
                    }

                    return new DC_Message { StatusMessage = "CityMaster sync has been scheduled successfully.", StatusCode = ReadOnlyMessage.StatusCode.Success };
                }
            }
            catch (Exception ex)
            {
                return new DC_Message { StatusMessage = ex.Message, StatusCode = ReadOnlyMessage.StatusCode.Failed };
            }
        }
        #endregion

        #region Hotel

        public DC_Message SyncHotelMapping(Guid Hotel_Id, string CreatedBy)
        {
            DC_Message _msg = new DC_Message();
            try
            {
                Guid LogId= new Guid();

                using (ConsumerEntities context = new ConsumerEntities())

                {
                    var iScheduledCount = context.DistributionLayerRefresh_Log.AsNoTracking()
                                       .Where(w => (w.Status.ToUpper().Trim() == "RUNNING" || w.Status.ToUpper().Trim() == "SCHEDULED") && w.Element.ToUpper().Trim() == "HOTEL" && w.Type.ToUpper().Trim() == "MAPPING").Count();

                    if (iScheduledCount > 0)
                    {
                        _msg.StatusMessage = "Hotel Mapping sync has already been scheduled.";
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Information;
                        return _msg;
                        // return new DC_Message { StatusMessage = "HotelMapping sync has already been scheduled.", StatusCode = ReadOnlyMessage.StatusCode.Information };
                    }
                    else
                    {
                         LogId = Guid.NewGuid();
                        _msg = InsertDistributionLogNewEntry(LogId, "Hotel", "Mapping", "Scheduled", CreatedBy);
                    }
                    if (Hotel_Id == Guid.Empty)
                    {
                        using (DHSVCProxyAsync DHP = new DHSVCProxyAsync())
                        {
                            string strURI = string.Format(System.Configuration.ConfigurationManager.AppSettings["Load_ProductMapping"], LogId.ToString());
                            DHP.GetAsync(ProxyFor.SqlToMongo, strURI);
                        }
                    }
                    else
                    {
                        //Code goes here for Indert Update or Delete of a specific Hotel
                    }
                    return _msg;

                   // return new DC_Message { StatusMessage = "HotelMapping sync has been scheduled successfully.", StatusCode = ReadOnlyMessage.StatusCode.Success };
                }
            }
            catch (Exception ex)
            {
                return new DC_Message { StatusMessage = ex.Message, StatusCode = ReadOnlyMessage.StatusCode.Failed };
            }
        }

        public DC_Message SyncHotelMappingLite(Guid Hotel_Id, string CreatedBy)
        {
            DC_Message _msg = new DC_Message();
            try
            {
                Guid LogId =new Guid();
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var iScheduledCount = context.DistributionLayerRefresh_Log.AsNoTracking()
                                        .Where(w => (w.Status.ToUpper().Trim() == "RUNNING" || w.Status.ToUpper().Trim() == "SCHEDULED") && w.Element.ToUpper().Trim() == "HOTEL" && w.Type.ToUpper().Trim() == "MAPPINGLITE").Count();
                    
                    if (iScheduledCount > 0)
                    {
                        _msg.StatusMessage = "Hotel MappingLite sync has already been scheduled.";
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Information ;
                        return _msg;
                    }
                    else
                    {
                        LogId = Guid.NewGuid();
                        _msg = InsertDistributionLogNewEntry(LogId, "Hotel", "MappingLite", "Scheduled", CreatedBy);
                    }
                   
                    if (Hotel_Id == Guid.Empty)
                    {
                        using (DHSVCProxyAsync DHP = new DHSVCProxyAsync())
                        {
                            string strURI = string.Format(System.Configuration.ConfigurationManager.AppSettings["Load_ProductMappingLite"], LogId.ToString());
                            DHP.GetAsync(ProxyFor.SqlToMongo, strURI);
                        }
                    }
                    else
                    {
                        //Code goes here for Indert Update or Delete of a specific Hotel
                    }
                    return _msg;
                    //return new DC_Message { StatusMessage = "Hotel MappingLite sync has been scheduled successfully.", StatusCode = ReadOnlyMessage.StatusCode.Success };
                }
            }
            catch (Exception ex)
            {
                return new DC_Message { StatusMessage = ex.Message, StatusCode = ReadOnlyMessage.StatusCode.Failed };
            }
        }

        private DC_Message InsertDistributionLogNewEntry(Guid LogId,string elementName, string Type,string status,string CreatedBy)
        {
            DC_Message msg = new DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                   
                        DataLayer.DistributionLayerRefresh_Log objNew = new DistributionLayerRefresh_Log();
                        objNew.Id = LogId;
                        objNew.Element = elementName;
                        objNew.Type = Type;
                        objNew.Create_Date = DateTime.Now;
                        objNew.Create_User = CreatedBy;// System.Web.HttpContext.Current.User.Identity.Name;
                        objNew.Status = status;
                        context.DistributionLayerRefresh_Log.Add(objNew);
                        context.SaveChanges();
                        msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        msg.StatusMessage = "Sync has been scheduled successfully.";
                }
            }
            catch (Exception )
            {

                throw;
            }
            return msg;
        }
        #endregion

        #region Activity

        public DC_Message SyncActivityMapping(Guid Activity_Id, string CreatedBy)
        {
            try
            {
                Guid LogId;

                using (ConsumerEntities context = new ConsumerEntities())

                {
                    var iScheduledCount = (from dlr in context.DistributionLayerRefresh_Log
                                           where dlr.Status == "Scheduled" && dlr.Element == "Activity" && dlr.Type == "Mapping"
                                           select true).Count();

                    if (iScheduledCount > 0)
                    {
                        return new DC_Message { StatusMessage = "ActivityMapping sync has already been scheduled.", StatusCode = ReadOnlyMessage.StatusCode.Information };
                    }
                    else
                    {
                        LogId = Guid.NewGuid();

                        DataLayer.DistributionLayerRefresh_Log objNew = new DistributionLayerRefresh_Log();
                        objNew.Id = LogId;
                        objNew.Element = "Activities";
                        objNew.Type = "Mapping";
                        objNew.Create_Date = DateTime.Now;
                        objNew.Create_User = CreatedBy;// System.Web.HttpContext.Current.User.Identity.Name;
                        objNew.Status = "Scheduled";
                        context.DistributionLayerRefresh_Log.Add(objNew);
                        context.SaveChanges();
                    }
                    if (Activity_Id == Guid.Empty)
                    {
                        using (DHSVCProxyAsync DHP = new DHSVCProxyAsync())
                        {
                            string strURI = string.Format(System.Configuration.ConfigurationManager.AppSettings["Load_ActivityMapping"], LogId.ToString());
                            DHP.GetAsync(ProxyFor.SqlToMongo, strURI);
                        }
                    }
                    else
                    {
                        //Code goes here for Indert Update or Delete of a specific country
                    }

                    return new DC_Message { StatusMessage = "ActivityMapping sync has been scheduled successfully.", StatusCode = ReadOnlyMessage.StatusCode.Success };
                }
            }
            catch (Exception ex)
            {
                return new DC_Message { StatusMessage = ex.Message, StatusCode = ReadOnlyMessage.StatusCode.Failed };
            }
        }
        #endregion

        #region Activity

        public DC_Message SyncSupplierMaster(Guid supplier_id, string CreatedBy)
        {
            try
            {
                Guid LogId;

                using (ConsumerEntities context = new ConsumerEntities())

                {
                    var iScheduledCount = (from dlr in context.DistributionLayerRefresh_Log
                                           where dlr.Status == "Scheduled" && dlr.Element == "Supplier" && dlr.Type == "Master"
                                           select true).Count();

                    if (iScheduledCount > 0)
                    {
                        return new DC_Message { StatusMessage = "SupplierMaster sync has already been scheduled.", StatusCode = ReadOnlyMessage.StatusCode.Information };
                    }
                    else
                    {
                        LogId = Guid.NewGuid();

                        DataLayer.DistributionLayerRefresh_Log objNew = new DistributionLayerRefresh_Log();
                        objNew.Id = LogId;
                        objNew.Element = "Supplier";
                        objNew.Type = "Master";
                        objNew.Create_Date = DateTime.Now;
                        objNew.Create_User = CreatedBy;// System.Web.HttpContext.Current.User.Identity.Name;
                        objNew.Status = "Scheduled";
                        context.DistributionLayerRefresh_Log.Add(objNew);
                        context.SaveChanges();
                    }
                    if (supplier_id == Guid.Empty)
                    {
                        using (DHSVCProxyAsync DHP = new DHSVCProxyAsync())
                        {
                            string strURI = string.Format(System.Configuration.ConfigurationManager.AppSettings["Load_SupplierMaster"], LogId.ToString());
                            DHP.GetAsync(ProxyFor.SqlToMongo, strURI);
                        }
                    }
                    else
                    {
                        //Code goes here for Indert Update or Delete of a specific country
                    }

                    return new DC_Message { StatusMessage = "SupplierMaster sync has been scheduled successfully.", StatusCode = ReadOnlyMessage.StatusCode.Success };
                }
            }
            catch (Exception ex)
            {
                return new DC_Message { StatusMessage = ex.Message, StatusCode = ReadOnlyMessage.StatusCode.Failed };
            }
        }
        #endregion

        #region Port

        public DC_Message SyncPortMaster(Guid port_Id, string CreatedBy)
        {
            try
            {
                Guid LogId;
                using (ConsumerEntities context = new ConsumerEntities())

                {
                    var iScheduledCount = (from dlr in context.DistributionLayerRefresh_Log
                                           where dlr.Status == "Scheduled" && dlr.Element == "Port" && dlr.Type == "Master"
                                           select true).Count();

                    if (iScheduledCount > 0)
                    {
                        return new DC_Message { StatusMessage = "PortMaster sync has already been scheduled.", StatusCode = ReadOnlyMessage.StatusCode.Information };
                    }
                    else
                    {
                        LogId = Guid.NewGuid();

                        DataLayer.DistributionLayerRefresh_Log objNew = new DistributionLayerRefresh_Log();
                        objNew.Id = LogId;
                        objNew.Element = "Port";
                        objNew.Type = "Master";
                        objNew.Create_Date = DateTime.Now;
                        objNew.Create_User = CreatedBy;// System.Web.HttpContext.Current.User.Identity.Name;
                        objNew.Status = "Scheduled";
                        context.DistributionLayerRefresh_Log.Add(objNew);
                        context.SaveChanges();
                    }
                    if (port_Id == Guid.Empty)
                    {
                        using (DHSVCProxyAsync DHP = new DHSVCProxyAsync())
                        {
                            string strURI = string.Format(System.Configuration.ConfigurationManager.AppSettings["Load_PortMaster"], LogId.ToString());
                            DHP.GetAsync(ProxyFor.SqlToMongo, strURI);
                        }
                    }
                    else
                    {
                        //Code goes here for Indert Update or Delete of a specific country
                    }

                    return new DC_Message { StatusMessage = "PortMaster sync has been scheduled successfully.", StatusCode = ReadOnlyMessage.StatusCode.Success };

                }
            }
            catch (Exception ex)
            {
                return new DC_Message { StatusMessage = ex.Message, StatusCode = ReadOnlyMessage.StatusCode.Failed };
            }
        }

        #endregion
        #region state

        public DC_Message SyncStateMaster(Guid state_Id, string CreatedBy)
        {
            try
            {
                Guid LogId;
                using (ConsumerEntities context = new ConsumerEntities())

                {
                    var iScheduledCount = (from dlr in context.DistributionLayerRefresh_Log
                                           where dlr.Status == "Scheduled" && dlr.Element == "State" && dlr.Type == "Master"
                                           select true).Count();

                    if (iScheduledCount > 0)
                    {
                        return new DC_Message { StatusMessage = "StateMaster sync has already been scheduled.", StatusCode = ReadOnlyMessage.StatusCode.Information };
                    }
                    else
                    {
                        LogId = Guid.NewGuid();

                        DataLayer.DistributionLayerRefresh_Log objNew = new DistributionLayerRefresh_Log();
                        objNew.Id = LogId;
                        objNew.Element = "State";
                        objNew.Type = "Master";
                        objNew.Create_Date = DateTime.Now;
                        objNew.Create_User = CreatedBy;// System.Web.HttpContext.Current.User.Identity.Name;
                        objNew.Status = "Scheduled";
                        context.DistributionLayerRefresh_Log.Add(objNew);
                        context.SaveChanges();
                    }
                    if (state_Id == Guid.Empty)
                    {
                        using (DHSVCProxyAsync DHP = new DHSVCProxyAsync())
                        {
                            string strURI = string.Format(System.Configuration.ConfigurationManager.AppSettings["Load_StateMaster"], LogId.ToString());
                            DHP.GetAsync(ProxyFor.SqlToMongo, strURI);
                        }
                    }
                    else
                    {
                        //Code goes here for Indert Update or Delete of a specific country
                    }

                    return new DC_Message { StatusMessage = "StateMaster sync has been scheduled successfully.", StatusCode = ReadOnlyMessage.StatusCode.Success };

                }
            }
            catch (Exception ex)
            {
                return new DC_Message { StatusMessage = ex.Message, StatusCode = ReadOnlyMessage.StatusCode.Failed };
            }
        }
        #endregion

        #region Supplier Static Hotel

        public DC_Message SyncSupplierStaticHotel(Guid log_id, Guid supplier_Id ,string CreatedBy)
        {
            try
            {
                Guid LogId;
                using (ConsumerEntities context = new ConsumerEntities())

                {
                    var iScheduledCount = (from dlr in context.DistributionLayerRefresh_Log
                                           where ( dlr.Status == "Scheduled" || dlr.Status=="Running") && dlr.Element == "Hotels" && dlr.Type == "Static"
                                           select true).Count();

                    if (iScheduledCount > 0)
                    {
                        return new DC_Message { StatusMessage = "Supplier Static Hotel sync has already been scheduled.", StatusCode = ReadOnlyMessage.StatusCode.Information };
                    }
                    else
                    {
                        LogId = Guid.NewGuid();

                        DataLayer.DistributionLayerRefresh_Log objNew = new DistributionLayerRefresh_Log();
                        objNew.Id = LogId;
                        objNew.Element = "Hotels";
                        objNew.Type = "Static";
                        objNew.Create_Date = DateTime.Now;
                        objNew.Create_User = CreatedBy;// System.Web.HttpContext.Current.User.Identity.Name;
                        objNew.Status = "Scheduled";
                        objNew.Supplier_Id = supplier_Id;
                        objNew.Edit_date = DateTime.Now;
                        objNew.Edit_User = CreatedBy;
                        context.DistributionLayerRefresh_Log.Add(objNew);
                        context.SaveChanges();
                    }
                    if (log_id == Guid.Empty)
                    {
                        using (DHSVCProxyAsync DHP = new DHSVCProxyAsync())
                        {
                            string strURI = string.Format(System.Configuration.ConfigurationManager.AppSettings["Load_SupplierStaticHotels"], LogId.ToString(),supplier_Id.ToString());
                            DHP.GetAsync(ProxyFor.SqlToMongo, strURI);
                        }
                    }
                    else
                    {
                        //Code goes here for Indert Update or Delete of a specific country
                    }

                    return new DC_Message { StatusMessage = "Supplier Static Hotel sync has been scheduled successfully.", StatusCode = ReadOnlyMessage.StatusCode.Success };

                }
            }
            catch (Exception ex)
            {
                return new DC_Message { StatusMessage = ex.Message, StatusCode = ReadOnlyMessage.StatusCode.Failed };
            }
        }
        #endregion





    }
}