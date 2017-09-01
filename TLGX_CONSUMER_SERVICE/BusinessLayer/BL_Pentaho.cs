using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BL_Pentaho : IDisposable
    {
        public void Dispose()
        { }

        public DataContracts.DC_Message Pentaho_SupplierApi_Call(string ApiLocationId, string CalledBy)
        {
            using (DataLayer.DL_Pentaho obj = new DataLayer.DL_Pentaho())
            {
                return obj.Pentaho_SupplierApi_Call(Guid.Parse(ApiLocationId), CalledBy);
            }
        }

        public DataContracts.Pentaho.DC_PentahoTransStatus_TransStatus Pentaho_SupplierApiCall_ViewDetails(string PentahoCall_Id)
        {
            using (DataLayer.DL_Pentaho obj = new DataLayer.DL_Pentaho())
            {
                return obj.Pentaho_SupplierApiCall_ViewDetails(Guid.Parse(PentahoCall_Id));
            }
        }

        public string Pentaho_SupplierApiLocationId_Get(string SupplierId, string EntityId)
        {
            using (DataLayer.DL_Pentaho obj = new DataLayer.DL_Pentaho())
            {
                return obj.Pentaho_SupplierApiLocationId_Get(Guid.Parse(SupplierId), Guid.Parse(EntityId));
            }
        }

        public DataContracts.DC_Message Pentaho_SupplierApiCall_Remove(string PentahoCallId, string CalledBy)
        {
            using (DataLayer.DL_Pentaho obj = new DataLayer.DL_Pentaho())
            {
                return obj.Pentaho_SupplierApiCall_Remove(Guid.Parse(PentahoCallId), CalledBy);
            }
        }

        public List<DataContracts.Pentaho.DC_PentahoApiCallLogDetails> Pentaho_SupplierApiCall_List(DataContracts.Pentaho.DC_PentahoApiCallLogDetails_RQ RQ)
        {
            using (DataLayer.DL_Pentaho obj = new DataLayer.DL_Pentaho())
            {
                return obj.Pentaho_SupplierApiCall_List(RQ);
            }
        }
    }
}
