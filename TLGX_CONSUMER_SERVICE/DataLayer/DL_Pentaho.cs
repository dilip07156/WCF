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
                        string endpointurl = "runTrans/?trans=" + apilocation;
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

                            context.Supplier_ApiCallLog.Add(new Supplier_ApiCallLog
                            {
                                SupplierApiCallLog_Id = Guid.NewGuid(),
                                SupplierApiLocation_Id = ApiLocationId,
                                PentahoCall_Id = Guid.Parse(callResult.id),
                                CalledDate = DateTime.Now,
                                CalledBy = CalledBy,
                                Message = callResult.message,
                                Status = "CALL"
                            });
                            context.SaveChanges();

                            return dc;
                        }
                        else
                        {
                            context.Supplier_ApiCallLog.Add(new Supplier_ApiCallLog
                            {
                                SupplierApiCallLog_Id = Guid.NewGuid(),
                                SupplierApiLocation_Id = ApiLocationId,
                                PentahoCall_Id = null,
                                CalledDate = DateTime.Now,
                                CalledBy = "System",
                                Message = "Api Call failed.",
                                Status = "CALL"
                            });
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
                            CalledDate = DateTime.Now,
                            CalledBy = "System",
                            Message = "Api Location is not found.",
                            Status = "CALL"
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

        public string Pentaho_SupplierApiLocationId_Get(Guid SupplierId, Guid EntityId)
        {
            try
            {
                if (SupplierId == Guid.Empty || EntityId == Guid.Empty)
                {
                    return string.Empty;
                }
                else
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        var ApiLocationId = context.Supplier_APILocation.Where(w => w.Entity_Id == EntityId && w.Supplier_Id == SupplierId).Select(s => s.Supplier_APILocation_Id).FirstOrDefault();
                        return ApiLocationId.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                return string.Empty;
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
                            p.CalledBy = "System";
                            p.CalledDate = DateTime.Now;
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

        public List<DataContracts.Pentaho.DC_PentahoApiCallLogDetails> Pentaho_SupplierApiCall_List()
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var result = (from log in context.Supplier_ApiCallLog
                                  join loc in context.Supplier_APILocation on log.SupplierApiLocation_Id equals loc.Supplier_APILocation_Id
                                  join sup in context.Suppliers on loc.Supplier_Id equals sup.Supplier_Id
                                  join ent in context.m_masterattributevalue on loc.Entity_Id equals ent.MasterAttributeValue_Id
                                  where log.Status == "CALL"
                                  select new DataContracts.Pentaho.DC_PentahoApiCallLogDetails
                                  {
                                      Entity_Id = loc.Entity_Id,
                                      ApiPath = loc.API_Path,
                                      CalledBy = log.CalledBy,
                                      CalledDate = log.CalledDate,
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
    }

}
