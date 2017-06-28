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
        public IList<DataContracts.Schedulers.DC_Supplier_Schedule> GetSchedule(string Supplier_Id)
        {
            using (BusinessLayer.BL_Schedule obj = new BL_Schedule())
            {
                return obj.GetSchedule(Supplier_Id);
            }
        }

        public DataContracts.DC_Message AddUpdateSchedule(DataContracts.Schedulers.DC_Supplier_Schedule obj)
        {
            using (BusinessLayer.BL_Schedule objBL = new BL_Schedule())
            {
                return objBL.AddUpdateSchedule(obj);
            }
        }

    }
}