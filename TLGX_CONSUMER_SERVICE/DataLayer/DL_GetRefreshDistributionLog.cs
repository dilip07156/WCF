using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DL_GetRefreshDistributionLog : IDisposable
    {
        public void Dispose()
        {

        }

        public List<DC_RefreshDistributionDataLog> GetRefreshDistributionLog()
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var UpdatedDate = (from dlr in context.DistributionLayerRefresh_Log
                                           //where dlr.Status == "Completed"
                                       group dlr by new { dlr.Element, dlr.Type } into d
                                       select new DC_RefreshDistributionDataLog
                                       {
                                           Create_User = string.Empty,
                                           Element = d.Key.Element,
                                           Status = string.Empty,
                                           Type = d.Key.Type,
                                           Create_Date = d.Max(t => t.Create_Date),
                                           //SupplierId= d.Key.Supplier_Id.ToString()

                                       }).ToList();
                    return UpdatedDate;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching date", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

        }

        //comment it
        public List<DC_RefreshDistributionDataLog> GetRefreshStaticHotelLog()
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var UpdatedDate = (from dlr in context.DistributionLayerRefresh_Log
                                           // join b in context.SupplierEntity on dlr.Status_Id equals b.Supplier_Id.ToString()

                                       group dlr by new { dlr.Element, dlr.Type, dlr.Supplier_Id } into d
                                       select new DC_RefreshDistributionDataLog
                                       {
                                           Create_User = string.Empty,
                                           Element = d.Key.Element,
                                           Status = string.Empty,
                                           Type = d.Key.Type,
                                           Create_Date = d.Max(t => t.Create_Date),
                                           SupplierId = d.Key.Supplier_Id.ToString()
                                       }).ToList();
                    return UpdatedDate;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching date", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

        }

        public string GetMaxDate(Guid? Supplier_Id)
        {
            string maxdt = "";

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var result = (from dlr in context.DistributionLayerRefresh_Log
                                  where dlr.Supplier_Id == Supplier_Id
                                  select new DataContracts.DC_RefreshDistributionDataLog
                                  {
                                      Create_Date = dlr.Create_Date,
                                      Status = dlr.Status
                                  }).ToList();
                    if (result != null && result.Count > 0)
                    {
                        maxdt = result.Max(a => a.Create_Date).Value.ToString();

                    }
                    maxdt = maxdt == null ? "" : maxdt;


                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching date", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
            return maxdt;
        }

        public List<DC_SupplierEntity> LoadSupplierData()
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;
                    context.Configuration.AutoDetectChangesEnabled = false;

                    using (var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        var distribution = context.DistributionLayerRefresh_Log.AsNoTracking().Where(x => x.Element == "Hotels" && x.Type == "Static").GroupBy(x => x.Supplier_Id).Select(g => g.OrderByDescending(x => x.Create_Date).FirstOrDefault()).ToList();
                        var supplier = (from s in context.Supplier select s).AsEnumerable();
                        var SupplierData = (from s in supplier
                                            join b in distribution on s.Supplier_Id equals b.Supplier_Id into c
                                            from subset in c.DefaultIfEmpty()
                                            where s.StatusCode == "ACTIVE"
                                            orderby s.Name ascending
                                            select new DC_SupplierEntity
                                            {
                                                Supplier_Id = s.Supplier_Id,
                                                Supplier_Name = s.Name,
                                                Element = (subset == null) ? "Hotels" : subset.Element,
                                                Type = (subset == null) ? "Static" : subset.Type,
                                                Status = (subset == null) ? string.Empty : subset.Status ?? string.Empty,
                                                LastUpdated = subset == null ? string.Empty : subset.Create_Date.ToString(),
                                                TotalCount = subset == null ? 0 : subset.TotalCount,
                                                MongoPushCount = subset == null ? 0 : subset.MongoPushCount,
                                            }).Distinct().ToList();

                        return SupplierData;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching date", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

        }

        #region == ML Data Integration
        public DC_Message SyncMLAPIData(DC_Distribution_MLDataRQ _obj)
        {
            DC_Message _msg = new DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var iScheduledCount = context.DistributionLayerRefresh_Log.AsNoTracking()
                                       .Where(w => (w.Status.ToUpper().Trim() == "RUNNING" || w.Status.ToUpper().Trim() == "SCHEDULED") && w.Element.ToUpper().Trim() == _obj.Element.Trim().ToUpper() && w.Type.ToUpper().Trim() == _obj.Type.Trim().ToUpper()).Count();

                    if (iScheduledCount > 0)
                    {
                        _msg.StatusMessage = GetMessageFromObjElemet(_obj.Element) + "sync has already been scheduled.";
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Information;
                        return _msg;
                    }
                    else
                    {
                        Guid logid = Guid.NewGuid();
                        _obj.Logid = logid;
                        _msg = InsertDistributionLogNewEntryForMLDataAPI(_obj);
                        _msg.StatusMessage = GetMessageFromObjElemet(_obj.Element) + _msg.StatusMessage;
                        string strURI = string.Empty;
                        string baseAddress = Convert.ToString(OperationContext.Current.Host.BaseAddresses[0]);

                        //Get URI for Element and Entity Type
                        if (_obj.Element.Trim().ToUpper() == "MLDATAMASTERACCO" && _obj.Type.Trim().ToUpper() == "MASTER")
                        {
                            strURI = string.Format(baseAddress + System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_MasterAccommodationRecord"] + logid.ToString());
                        }
                        else if (_obj.Element.Trim().ToUpper() == "MLDATAMASTERACCORMFACILITY" && _obj.Type.Trim().ToUpper() == "MASTER")
                        {
                            strURI = string.Format(baseAddress + System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_MasterAccommodationRoomFacilities"] + logid.ToString());
                        }
                        else if (_obj.Element.Trim().ToUpper() == "MLDATAMASTERACCORMINFO" && _obj.Type.Trim().ToUpper() == "MASTER")
                        {
                            strURI = string.Format(baseAddress + System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_MasterAccommodationRoomInfo"] + logid.ToString());
                        }
                        else if (_obj.Element.Trim().ToUpper() == "MLDATAROOMTYPEMATCHING" && _obj.Type.Trim().ToUpper() == "MAPPING")
                        {
                            strURI = string.Format(baseAddress + System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_RoomTypeMatching"] + logid.ToString());
                        }
                        else if (_obj.Element.Trim().ToUpper() == "MLDATASUPPLIERACCO" && _obj.Type.Trim().ToUpper() == "MAPPING")
                        {
                            strURI = string.Format(baseAddress + System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_SupplierAccommodationData"] + logid.ToString());
                        }
                        else if (_obj.Element.Trim().ToUpper() == "MLDATASUPPLIERACCORM" && _obj.Type.Trim().ToUpper() == "MAPPING")
                        {
                            strURI = string.Format(baseAddress + System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_SupplierAccommodationRoomData"] + logid.ToString());
                        }
                        else if (_obj.Element.Trim().ToUpper() == "MLDATASUPPLIERACCORMEXTATTR" && _obj.Type.Trim().ToUpper() == "MAPPING")
                        {
                            strURI = string.Format(baseAddress + System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_SupplierAccommodationRoomExtendedAttributes"] + logid.ToString());
                        }
                        using (DHSVCProxyAsync DHP = new DHSVCProxyAsync())
                        {

                            DHP.GetAsync(ProxyFor.MachingLearningDataTransfer, strURI);
                        }
                    }

                }

            }
            catch (Exception)
            {

                throw;
            }
            return _msg;
        }
        private string GetMessageFromObjElemet(string Element)
        {
            string Entity = string.Empty;
            string strElement = Element.Trim().ToUpper();
            if (strElement == "MLDATAMASTERACCO")
                Entity = "Master Accommodation Data";
            else if (strElement == "MLDATAMASTERACCORMFACILITY")
                Entity = "Master Accommodation Room Facility Data";
            else if (strElement == "MLDATAMASTERACCORMINFO")
                Entity = "Master Accommodation Room Info Data";
            else if (strElement == "MLDATAROOMTYPEMATCHING")
                Entity = "Room Type Matching Data";
            else if (strElement == "MLDATASUPPLIERACCO")
                Entity = "Supplier Accommodation Data";
            else if (strElement == "MLDATASUPPLIERACCORM")
                Entity = "Supplier Accommodation Room Data";
            else if (strElement == "MLDATASUPPLIERACCORMEXTATTR")
                Entity = "Supplier Accommodation Room Ext attribute Data";
            return Entity;
        }

        private DC_Message InsertDistributionLogNewEntryForMLDataAPI(DC_Distribution_MLDataRQ obj)
        {
            DC_Message msg = new DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;
                    string CallingAgent = woc.Headers["CallingAgent"];
                    string CallingUser = woc.Headers["CallingUser"];
                    DataLayer.DistributionLayerRefresh_Log objNew = new DistributionLayerRefresh_Log();
                    objNew.Id = (obj.Logid ?? Guid.NewGuid());
                    objNew.Element = obj.Element.Trim();
                    objNew.Type = obj.Type.Trim();
                    objNew.Create_Date = DateTime.Now;
                    objNew.Create_User = CallingUser;
                    objNew.Status = obj.Status;
                    context.DistributionLayerRefresh_Log.Add(objNew);
                    context.SaveChanges();
                    msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                    msg.StatusMessage = "Sync has been scheduled successfully.";
                }
            }
            catch (Exception)
            {

                throw;
            }
            return msg;
        }
        #endregion

    }


}

