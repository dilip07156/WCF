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
        public DC_Message Pentaho_SupplierApi_Call(string ApiLocationId, string CalledBy)
        {
            using (BusinessLayer.BL_Pentaho obj = new BL_Pentaho())
            {
                return obj.Pentaho_SupplierApi_Call(ApiLocationId, CalledBy);
            }
        }

        public DataContracts.Pentaho.DC_PentahoTransStatus_TransStatus Pentaho_SupplierApiCall_ViewDetails(string PentahoCall_Id)
        {
            using (BusinessLayer.BL_Pentaho obj = new BL_Pentaho())
            {
                return obj.Pentaho_SupplierApiCall_ViewDetails(PentahoCall_Id);
            }
        }

        public string Pentaho_SupplierApiLocationId_Get(string SupplierId, string EntityId)
        {
            using (BusinessLayer.BL_Pentaho obj = new BL_Pentaho())
            {
                return obj.Pentaho_SupplierApiLocationId_Get(SupplierId, EntityId);
            }
        }

        public DC_Message Pentaho_SupplierApiCall_Remove(string PentahoCallId, string CalledBy)
        {
            using (BusinessLayer.BL_Pentaho obj = new BL_Pentaho())
            {
                return obj.Pentaho_SupplierApiCall_Remove(PentahoCallId, CalledBy);
            }
        }

        public List<DataContracts.Pentaho.DC_PentahoApiCallLogDetails> Pentaho_SupplierApiCall_List()
        {
            using (BusinessLayer.BL_Pentaho obj = new BL_Pentaho())
            {
                return obj.Pentaho_SupplierApiCall_List();
            }
        }
    }
}