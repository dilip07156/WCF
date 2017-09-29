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

        #region "Activity Status"
        public List<DC_Activity_Status> GetActivityStatus(Guid Activity_Id, Guid DataKey_Id)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.GetActivityStatus(Activity_Id, DataKey_Id);
            }
        }

        public bool UpdateActivityStatus(DC_Activity_Status AS)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.UpdateActivityStatus(AS);
            }
        }

        public bool AddActivityStatus(DC_Activity_Status AS)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.AddActivityStatus(AS);
            }
        }
        #endregion
        #region Activity Media
        public List<DC_Activity_Media_Search> GetActivityMedia(DC_Activity_Media_Search_RQ RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.GetActivityMedia(RQ);
            }
        }
        public DC_Message AddActivityMedia(DC_Activity_Media RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.AddActivityMedia(RQ);
            }
        }
        #endregion
    }
}
