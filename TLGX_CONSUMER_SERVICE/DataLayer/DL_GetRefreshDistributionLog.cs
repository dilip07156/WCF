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
                                       group dlr by new { dlr.Element, dlr.Type, dlr.Status } into d
                                       select new DC_RefreshDistributionDataLog
                                       {
                                           Create_User = string.Empty,
                                           Element = d.Key.Element,
                                           Status = d.Key.Status,
                                           Type = d.Key.Type,
                                           Create_Date = d.Max(t => t.Create_Date)
                                       }).ToList();
                    return UpdatedDate;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching date", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

        }
    }
}
