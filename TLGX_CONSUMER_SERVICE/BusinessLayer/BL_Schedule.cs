using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BL_Schedule : IDisposable
    {
        public void Dispose()
        {

        }

        public IList<DataContracts.Schedulers.DC_Supplier_Schedule> GetSchedule(string Supplier_Id)
        {
            using (DataLayer.DL_Schedule obj = new DataLayer.DL_Schedule())
            {
                return obj.GetSchedule(Supplier_Id);
            }
        }
        public DataContracts.DC_Message AddUpdateSchedule(DataContracts.Schedulers.DC_Supplier_Schedule obj)
        {
            using (DataLayer.DL_Schedule objDL = new DataLayer.DL_Schedule())
            {
                return objDL.AddUpdateSchedule(obj);
            }
        }
    }
}
