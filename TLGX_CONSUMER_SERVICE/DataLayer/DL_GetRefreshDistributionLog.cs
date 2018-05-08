using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
                        var distinctFullPullSuppliers = context.SupplierEntity.AsNoTracking().Where(x => x.Parent_Id == null && x.Entity == "HotelInfo").Select(x => new { x.Supplier_Id, x.SupplierName }).Distinct().ToList();

                        var SupplierData = (from a in distinctFullPullSuppliers
                                            join b in distribution on a.Supplier_Id equals b.Supplier_Id into c
                                            from subset in c.DefaultIfEmpty()
                                            orderby a.SupplierName ascending
                                            select new DC_SupplierEntity
                                            {
                                                Supplier_Id = a.Supplier_Id,
                                                Supplier_Name = a.SupplierName,
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
    }
}
