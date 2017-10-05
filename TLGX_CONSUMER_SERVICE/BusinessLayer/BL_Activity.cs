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

        #endregion

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
        public List<DC_Activity_Media> GetActivityMedia(DC_Activity_Media_Search_RQ RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.GetActivityMedia(RQ);
            }
        }

        public DC_Message AddUpdateActivityMedia(DC_Activity_Media RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.AddUpdateActivityMedia(RQ);
            }
        }
        #endregion

        #region Activity inclusions
        public List<DC_Activity_Inclusions> GetActivityInclusions(DC_Activity_Inclusions_RQ RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.GetActivityInclusions(RQ);
            }
        }

        public DataContracts.DC_Message AddUpdateActivityInclusions(DataContracts.Masters.DC_Activity_Inclusions RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.AddUpdateActivityInclusions(RQ);
            }
        }
        #endregion

        #region InclusionDetails
        public List<DC_Activity_InclusionsDetails> GetActivityInclusionDetails(DC_Activity_InclusionDetails_RQ RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.GetActivityInclusionDetails(RQ);
            }
        }
        public DataContracts.DC_Message AddUpdateInclusionDetails(DataContracts.Masters.DC_Activity_InclusionsDetails RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.AddUpdateInclusionDetails(RQ);
            }
        }
        #endregion

        #region Activiy Clasification Attributes
        public List<DC_Activity_ClassificationAttributes> GetActivityClasificationAttributes(DC_Activity_ClassificationAttributes_RQ RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.GetActivityClasificationAttributes(RQ);
            }
        }

        public DC_Message AddUpdateActivityClassifiationAttributes(DC_Activity_ClassificationAttributes RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.AddUpdateActivityClassifiationAttributes(RQ);
            }
        }
        #endregion

        #region Activity PickUpDrop
        public List<DataContracts.Masters.DC_Activity_PickUpDrop> GetActivityPickUpDrop(DataContracts.Masters.DC_Activity_PickUpDrop_RQ RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.GetActivityPickUpDrop(RQ);
            }
        }
        public DC_Message AddUpdatePickUpDrop(DC_Activity_PickUpDrop RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.AddUpdatePickUpDrop(RQ);
            }
        }
        #endregion

        #region Activity PickUpDrop Details
        public List<DataContracts.Masters.DC_Activity_PickUpDropDetails> GetPickUpDropDetails(DataContracts.Masters.DC_Activity_PickUpDropDetails_RQ RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.GetPickUpDropDetails(RQ);
            }
        }
        public DataContracts.DC_Message AddUpdatePickUpDropDetails(DC_Activity_PickUpDropDetails RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.AddUpdatePickUpDropDetails(RQ);
            }
        }
        #endregion

        #region Activity Description
        //public List<DataContracts.Masters.DC_Activity_Descriptions> GetActivityDescription(DataContracts.Masters.DC_Activity_Descriptions_RQ RQ)
        //{
        //    using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
        //    {
        //        return obj.GetActivityDescription(RQ);
        //    }
        //}
        //public DataContracts.DC_Message AddUpdateActivityDescription(DC_Activity_Descriptions RQ)
        //{
        //    using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
        //    {
        //        return obj.AddUpdateActivityDescription(RQ);
        //    }
        //}
        #endregion

        #region Activity Flavour
        public List<DataContracts.Masters.DC_Activity_Flavour> GetActivityFlavour(DataContracts.Masters.DC_Activity_Flavour_RQ RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.GetActivityFlavour(RQ);
            }
        }
        public DataContracts.DC_Message AddUpdateActivityFlavour(DC_Activity_Flavour RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.AddUpdateActivityFlavour(RQ);
            }
        }
        #endregion

        #region Activity Deals
        public List<DataContracts.Masters.DC_Activity_Deals> GetActivityDeals(DataContracts.Masters.DC_Activity_Deals_RQ RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.GetActivityDeals(RQ);
            }
        }
        public DataContracts.DC_Message AddUpdateActivityDeals(DC_Activity_Deals RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.AddUpdateActivityDeals(RQ);
            }
        }
        #endregion

        #region Activity Prices
        public List<DataContracts.Masters.DC_Activity_Prices> GetActivityPrices(DataContracts.Masters.DC_Activity_Prices_RQ RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.GetActivityPrices(RQ);
            }
        }
        public DataContracts.DC_Message AddUpdateActivityPrices(DC_Activity_Prices RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.AddUpdateActivityPrices(RQ);
            }
        }
        #endregion

        #region Supplier Product mapping
        public List<DataContracts.Masters.DC_Activity_SupplierProductMapping> GetActSupplierProdMapping(DataContracts.Masters.DC_Activity_SupplierProductMapping_RQ RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.GetActSupplierProdMapping(RQ);
            }
        }
        public DataContracts.DC_Message AddUpdateActSupplierProdMapping(DC_Activity_SupplierProductMapping RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.AddUpdateActSupplierProdMapping(RQ);
            }
        }
        #endregion

    }
}
