using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DataContracts;
using DataContracts.Masters;

namespace BusinessLayer
{
    public class BL_Activity : IDisposable
    {
        public void Dispose()
        { }

        #region Activity Search
        public List<DC_ActivitySearch_RS> ActivitySearch(DC_Activity_Search_RQ RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.ActivitySearch(RQ);
            }
        }
        #endregion

        public DataContracts.DC_Message AddUpdateActivity(DataContracts.Masters.DC_Activity _objAct)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.AddUpdateActivity(_objAct);
            }
        }

        public DataContracts.DC_Message AddUpdateProductInfo(DataContracts.Masters.DC_Activity _objPro)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.AddUpdateProductInfo(_objPro);
            }
        }

        #region "Activity Contact"
        public List<DC_Activity_Contact> GetActivityContacts(Guid Activity_Id, Guid DataKey_Id)
        {
            using (DataLayer.DL_Activity _obj = new DataLayer.DL_Activity())
            {
                return _obj.GetActivityContacts(Activity_Id, DataKey_Id);
            }
        }

        public bool UpdateActivityContacts(DC_Activity_Contact AC)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.UpdateActivityContacts(AC);
            }
        }

        public bool AddActivityContacts(DC_Activity_Contact AC)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.AddActivityContacts(AC);
            }
        }
        
        public string GetLegacyProductId(Guid Activity_Id)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.GetLegacyProductId(Activity_Id);
            }
        }
        #endregion
    }
}
