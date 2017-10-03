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
using DataContracts.Masters;

namespace ConsumerSvc
{
    public partial class Consumer : IConsumer
    {
        #region Activity Search
        public IList<DC_ActivitySearch_RS> ActivitySearch(DC_Activity_Search_RQ Activity_Request)
        {
            using (BL_Activity objBL = new BL_Activity())
            {
                List<DC_ActivitySearch_RS> searchResults = new List<DC_ActivitySearch_RS>();
                searchResults = objBL.ActivitySearch(Activity_Request);

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }
        #endregion

        public DataContracts.DC_Message AddUpdateActivity(DataContracts.Masters.DC_Activity _objAct)
        {
            using (BusinessLayer.BL_Activity obj = new BL_Activity())
            {
                return obj.AddUpdateActivity(_objAct);
            }
        }

        public DataContracts.DC_Message AddUpdateProductInfo(DataContracts.Masters.DC_Activity _objPro)
        {
            using (BusinessLayer.BL_Activity obj = new BL_Activity())
            {
                return obj.AddUpdateProductInfo(_objPro);
            }
        }

        #region "Activity conatct"
        public IList<DC_Activity_Contact> GetActivityContacts(string Activity_Id, string DataKey_Id)
        {
            using (BL_Activity objBL = new BL_Activity())
            {
                List<DC_Activity_Contact> searchResults = new List<DC_Activity_Contact>();
                searchResults = objBL.GetActivityContacts(Guid.Parse(Activity_Id), Guid.Parse(DataKey_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }

        public bool UpdateActivityContacts(DC_Activity_Contact AC)
        {
            using (BusinessLayer.BL_Activity obj = new BL_Activity())
            {
                return obj.UpdateActivityContacts(AC);
            }
        }

        public bool AddActivityContacts(DC_Activity_Contact AC)
        {
            using (BusinessLayer.BL_Activity obj = new BL_Activity())
            {
                return obj.AddActivityContacts(AC);
            }
        }
        
        public string GetLegacyProductId(string Activity_Id)
        {
            using (BL_Activity obj = new BL_Activity())
            {
                return obj.GetLegacyProductId(Guid.Parse(Activity_Id));
            }
        }
        #endregion

        #region "Activity Status"
        public IList<DC_Activity_Status> GetActivityStatus(string Activity_Id, string DataKey_Id)
        {
            using (BL_Activity objBL = new BL_Activity())
            {
                List<DC_Activity_Status> searchResults = new List<DC_Activity_Status>();
                searchResults = objBL.GetActivityStatus(Guid.Parse(Activity_Id), Guid.Parse(DataKey_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }

        public bool UpdateActivityStatus(DC_Activity_Status AS)
        {
            using (BL_Activity obj = new BL_Activity())
            {
                return obj.UpdateActivityStatus(AS);
            }
        }

        public bool AddActivityStatus(DC_Activity_Status AS)
        {
            using (BL_Activity obj = new BL_Activity())
            {
                return obj.AddActivityStatus(AS);
            }
        }
        #endregion

        #region Activity Media
        public IList<DataContracts.Masters.DC_Activity_Media> GetActivityMedia(DC_Activity_Media_Search_RQ RQ)
        {
            using (BL_Activity objBL = new BL_Activity())
            {
                List<DataContracts.Masters.DC_Activity_Media> searchResults = new List<DC_Activity_Media>();
                searchResults = objBL.GetActivityMedia(RQ);
                if (searchResults.Count == 0)
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                return searchResults;
            }
        }
        public DC_Message AddActivityMedia(DC_Activity_Media RQ)
        {
            using (BusinessLayer.BL_Activity obj = new BL_Activity())
            {
                return obj.AddActivityMedia(RQ);
            }
        }
        #endregion

        #region Activity Inclusions
        public IList<DataContracts.Masters.DC_Activity_Inclusions_RS> GetActivityInclusions(DC_Activity_Inclusions_RQ RQ)
        {
            using (BL_Activity objBL = new BL_Activity())
            {
                List<DC_Activity_Inclusions_RS> searchResults = new List<DC_Activity_Inclusions_RS>();
                searchResults = objBL.GetActivityInclusions(RQ);
                if (searchResults.Count == 0)
                     throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                return searchResults;
            }
        }
        public DataContracts.DC_Message AddActivityInclusions(DataContracts.Masters.DC_Activity_Inclusions RQ)
        {
            using (BusinessLayer.BL_Activity obj = new BL_Activity())
            {
                return obj.AddActivityInclusions(RQ);
            }
        }
        #endregion

        #region Activiy Clasification Attributes
        public IList<DataContracts.Masters.DC_Activity_ClassificationAttributes> GetActivityClasificationAttributes(DC_Activity_ClassificationAttributes_RQ RQ)
        {
            using (BL_Activity objBL = new BL_Activity())
            {
                List<DC_Activity_ClassificationAttributes> searchResults = new List<DC_Activity_ClassificationAttributes>();
                searchResults = objBL.GetActivityClasificationAttributes(RQ);
                if (searchResults.Count == 0)
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                return searchResults;
            }
        }
        public DC_Message AddUpdateActivityClassifiationAttributes(DC_Activity_ClassificationAttributes RQ)
        {
            using (BusinessLayer.BL_Activity obj = new BL_Activity())
            {
                return obj.AddUpdateActivityClassifiationAttributes(RQ);
            }
        }
        #endregion
    }
}