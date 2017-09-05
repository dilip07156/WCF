using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DL_Pentaho : IDisposable
    {
        public void Dispose()
        {

        }

        public DC_Message Pentaho_SupplierApi_Call(Guid ApiLocationId, string CalledBy)
        {
            DC_Message dc = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var apilocation = (from a in context.Supplier_APILocation
                                       where a.Supplier_APILocation_Id == ApiLocationId
                                       && a.STATUS.ToUpper() == "ACTIVE"
                                       select a.API_Path).FirstOrDefault();

                    if (!string.IsNullOrWhiteSpace(apilocation))
                    {
                        var ApiCallId = Guid.NewGuid();
                        Guid PentahoCallId = Guid.Empty;

                        context.Supplier_ApiCallLog.Add(new Supplier_ApiCallLog
                        {
                            SupplierApiCallLog_Id = ApiCallId,
                            SupplierApiLocation_Id = ApiLocationId,
                            PentahoCall_Id = PentahoCallId,
                            Create_Date = DateTime.Now,
                            Create_User = CalledBy,
                            Message = "Transformation Called.",
                            Status = "SCHEDULED"
                        });
                        context.SaveChanges();
                        var getInsertedRow = context.Supplier_ApiCallLog.Find(ApiCallId);

                        string endpointurl = "runTrans/?trans=" + apilocation + "&api_call_id=" + ApiCallId.ToString();
                        object result;
                        DHSVCProxy.GetData(ProxyFor.Pentaho, endpointurl, typeof(DataContracts.Pentaho.DC_PentahoApiCallResult), out result);
                        DataContracts.Pentaho.DC_PentahoApiCallResult callResult = result as DataContracts.Pentaho.DC_PentahoApiCallResult;

                        if (callResult != null)
                        {
                            if (callResult.result.ToUpper() == "OK")
                            {
                                dc.StatusMessage = callResult.message;
                                dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                            }
                            else
                            {
                                dc.StatusMessage = callResult.message;
                                dc.StatusCode = ReadOnlyMessage.StatusCode.Danger;
                            }


                            Guid.TryParse(callResult.id, out PentahoCallId);
                            getInsertedRow.PentahoCall_Id = PentahoCallId;
                            getInsertedRow.Message = callResult.message;
                            context.SaveChanges();

                            return dc;
                        }
                        else
                        {
                            getInsertedRow.PentahoCall_Id = Guid.Empty;
                            getInsertedRow.Message = "Api Call failed.";
                            getInsertedRow.Status = "FAILED";
                            context.SaveChanges();

                            return new DC_Message { StatusMessage = "Api Call failed.", StatusCode = ReadOnlyMessage.StatusCode.Failed };
                        }
                    }
                    else
                    {
                        context.Supplier_ApiCallLog.Add(new Supplier_ApiCallLog
                        {
                            SupplierApiCallLog_Id = Guid.NewGuid(),
                            SupplierApiLocation_Id = ApiLocationId,
                            PentahoCall_Id = null,
                            Create_Date = DateTime.Now,
                            Create_User = CalledBy,
                            Message = "Api Location is not found.",
                            Status = "INVALID"
                        });
                        context.SaveChanges();
                        return new DC_Message { StatusMessage = "Api Location is not found.", StatusCode = ReadOnlyMessage.StatusCode.Warning };
                    }
                }


            }
            catch (Exception ex)
            {
                return new DC_Message { StatusMessage = ex.Message, StatusCode = ReadOnlyMessage.StatusCode.Danger };
            }
        }

        public List<DataContracts.Masters.DC_Supplier_ApiLocation> Pentaho_SupplierApiLocation_Get(Guid SupplierId, Guid EntityId)
        {
            try
            {
                if (SupplierId == Guid.Empty || EntityId == Guid.Empty)
                {
                    return new List<DataContracts.Masters.DC_Supplier_ApiLocation>();
                }
                else
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        var search = (from supApi in context.Supplier_APILocation
                                      select supApi).AsQueryable();

                        search = from sup in search
                                 where sup.Supplier_Id == SupplierId
                                 select sup;

                        search = from sup in search
                                 where sup.Entity_Id == EntityId
                                 select sup;

                        var result = from a in search
                                     join mav in context.m_masterattributevalue on a.Entity_Id equals mav.MasterAttributeValue_Id
                                     join sup in context.Suppliers on a.Supplier_Id equals sup.Supplier_Id
                                     select new DataContracts.Masters.DC_Supplier_ApiLocation
                                     {
                                         Supplier_Id = a.Supplier_Id ?? Guid.Empty,
                                         ApiEndPoint = a.API_Path,
                                         ApiLocation_Id = a.Supplier_APILocation_Id,
                                         Create_Date = a.CREATE_DATE,
                                         Create_User = a.CREATE_USER,
                                         Edit_Date = a.EDIT_DATE,
                                         Edit_User = a.EDIT_USER,
                                         Entity = mav.AttributeValue,
                                         Entity_Id = a.Entity_Id,
                                         IsActive = false,
                                         Status = a.STATUS,
                                         Supplier_Name = sup.Name
                                     };

                        return result.ToList();
                    }
                }
            }
            catch (Exception e)
            {
                return new List<DataContracts.Masters.DC_Supplier_ApiLocation>();
            }
        }

        public DataContracts.Pentaho.DC_PentahoTransStatus_TransStatus Pentaho_SupplierApiCall_ViewDetails(Guid PentahoCall_Id)
        {
            try
            {
                if (PentahoCall_Id == Guid.Empty)
                {
                    return null;
                }
                else
                {
                    string endpointurl = "transStatus/?xml=y&id=" + PentahoCall_Id.ToString();
                    object result;
                    DHSVCProxy.GetData(ProxyFor.Pentaho, endpointurl, typeof(DataContracts.Pentaho.DC_PentahoTransStatus_TransStatus), out result);

                    DataContracts.Pentaho.DC_PentahoTransStatus_TransStatus status = result as DataContracts.Pentaho.DC_PentahoTransStatus_TransStatus;

                    return status;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public DC_Message Pentaho_SupplierApiCall_Remove(Guid PentahoCall_Id, string CalledBy)
        {
            try
            {
                DC_Message dc = new DataContracts.DC_Message();
                if (PentahoCall_Id != Guid.Empty)
                {
                    string endpointurl = "removeTrans?id=" + PentahoCall_Id.ToString();
                    object result;
                    DHSVCProxy.GetData(ProxyFor.Pentaho, endpointurl, typeof(void), out result);

                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        List<Supplier_ApiCallLog> results = (from p in context.Supplier_ApiCallLog
                                                             where p.PentahoCall_Id == PentahoCall_Id
                                                             select p).ToList();
                        foreach (Supplier_ApiCallLog p in results)
                        {
                            p.Status = "REMOVED";
                            p.Edit_User = CalledBy;
                            p.Edit_Date = DateTime.Now;
                            p.Message = "Transformation Queue has been removed from Server.";
                        }
                        context.SaveChanges();
                    }

                    dc.StatusMessage = "The transformation was removed successfully.";
                    dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                }
                else
                {
                    dc.StatusMessage = "Invalid Transformation Call Id !";
                    dc.StatusCode = ReadOnlyMessage.StatusCode.Danger;
                }
                return dc;
            }
            catch (Exception e)
            {
                return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Danger, StatusMessage = e.Message };
            }
        }

        public List<DataContracts.Pentaho.DC_PentahoApiCallLogDetails> Pentaho_SupplierApiCall_List(DataContracts.Pentaho.DC_PentahoApiCallLogDetails_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var Supplier_APILocation = (from a in context.Supplier_APILocation select a).AsQueryable();


                    if (RQ.Supplier_Id != null)
                    {
                        if (RQ.Supplier_Id != Guid.Empty)
                        {
                            Supplier_APILocation = Supplier_APILocation.Where(w => w.Supplier_Id == RQ.Supplier_Id);
                        }
                    }

                    if (RQ.Entity_Id != null)
                    {
                        if (RQ.Entity_Id != Guid.Empty)
                        {
                            Supplier_APILocation = Supplier_APILocation.Where(w => w.Entity_Id == RQ.Entity_Id);
                        }
                    }

                    var Supplier_ApiCallLog = (from a in context.Supplier_ApiCallLog select a).AsQueryable();
                    if (!string.IsNullOrWhiteSpace(RQ.Status))
                    {
                        Supplier_ApiCallLog = Supplier_ApiCallLog.Where(w => w.Status.ToUpper().Trim() == RQ.Status.Trim().ToUpper());
                    }

                    var result = (from log in Supplier_ApiCallLog
                                  join loc in Supplier_APILocation on log.SupplierApiLocation_Id equals loc.Supplier_APILocation_Id
                                  join sup in context.Suppliers.AsNoTracking() on loc.Supplier_Id equals sup.Supplier_Id
                                  join ent in context.m_masterattributevalue.AsNoTracking() on loc.Entity_Id equals ent.MasterAttributeValue_Id
                                  select new DataContracts.Pentaho.DC_PentahoApiCallLogDetails
                                  {
                                      Entity_Id = loc.Entity_Id,
                                      ApiPath = loc.API_Path,//.Split('/').Last().Split('.').First().Trim(),
                                      Create_User = log.Create_User,
                                      Create_Date = log.Create_Date,
                                      Edit_User = log.Edit_User,
                                      Edit_Date = log.Edit_Date,
                                      Entity = ent.AttributeValue,
                                      Message = log.Message,
                                      PentahoCall_Id = log.PentahoCall_Id,
                                      Status = log.Status,
                                      Supplier = sup.Code,
                                      SupplierApiCallLog_Id = log.SupplierApiCallLog_Id,
                                      SupplierApiLocation_Id = log.SupplierApiLocation_Id,
                                      Supplier_Id = loc.Supplier_Id
                                  }).ToList();

                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<string> Pentaho_SupplierApiCall_Status()
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var result = (from log in context.Supplier_ApiCallLog select log.Status).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                return new List<string>();
            }
        }
    }

}
