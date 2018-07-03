using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BL_RefreshDistributionData : IDisposable
    {
        public void Dispose()
        {

        }

        #region Refresh distributation data Country

        public DC_Message SyncCountryMaster(string country_id, string CreatedBy)
        {
            Guid countryid = new Guid();

            if (Guid.TryParse(country_id, out countryid))
            {
                using (DataLayer.DL_MongoPush obj = new DataLayer.DL_MongoPush())
                {
                    return obj.SyncCountryMaster(countryid, CreatedBy);
                }
            }
            else
            {
                return new DC_Message { StatusMessage = "Invalid CountryID", StatusCode = ReadOnlyMessage.StatusCode.Danger };
            }
        }

        public DC_Message SyncCountryMapping(string country_id, string CreatedBy)
        {
            Guid countryid = new Guid();

            if (Guid.TryParse(country_id, out countryid))
            {
                using (DataLayer.DL_MongoPush obj = new DataLayer.DL_MongoPush())
                {
                    return obj.SyncCountryMapping(countryid, CreatedBy);
                }
            }
            else
            {
                return new DC_Message { StatusMessage = "Invalid CountryID", StatusCode = ReadOnlyMessage.StatusCode.Danger };
            }
        }
        #endregion
        #region City 
        public DC_Message SyncCityMapping(string city_id, string CreatedBy)
        {
            Guid cityid = new Guid();

            if (Guid.TryParse(city_id, out cityid))
            {
                using (DataLayer.DL_MongoPush obj = new DataLayer.DL_MongoPush())
                {
                    return obj.SyncCityMapping(cityid, CreatedBy);
                }
            }
            else
            {
                return new DC_Message { StatusMessage = "Invalid CityID", StatusCode = ReadOnlyMessage.StatusCode.Danger };
            }
        }

        public DC_Message SyncCityMaster(string city_id, string CreatedBy)
        {
            Guid cityid = new Guid();

            if (Guid.TryParse(city_id, out cityid))
            {
                using (DataLayer.DL_MongoPush obj = new DataLayer.DL_MongoPush())
                {
                    return obj.SyncCityMaster(cityid, CreatedBy);
                }
            }
            else
            {
                return new DC_Message { StatusMessage = "Invalid CityID", StatusCode = ReadOnlyMessage.StatusCode.Danger };
            }
        }
        #endregion

        #region Hotel

        public DC_Message SyncHotelMapping(string hotel_id, string CreatedBy)
        {
            Guid hotelid;

            if (Guid.TryParse(hotel_id, out hotelid))
            {
                using (DataLayer.DL_MongoPush obj = new DataLayer.DL_MongoPush())
                {
                    return obj.SyncHotelMapping(hotelid, CreatedBy);
                }
            }
            else
            {
                return new DC_Message { StatusMessage = "Invalid HotelID", StatusCode = ReadOnlyMessage.StatusCode.Danger };
            }
        }

        public DC_Message SyncHotelMappingLite(string hotel_id, string CreatedBy)
        {
            Guid hotelid;

            if (Guid.TryParse(hotel_id, out hotelid))
            {
                using (DataLayer.DL_MongoPush obj = new DataLayer.DL_MongoPush())
                {
                    return obj.SyncHotelMappingLite(hotelid, CreatedBy);
                }
            }
            else
            {
                return new DC_Message { StatusMessage = "Invalid HotelID", StatusCode = ReadOnlyMessage.StatusCode.Danger };
            }
        }
        #endregion
        #region Activity
        public DC_Message SyncActivityMapping(string activity_id, string CreatedBy)
        {
            Guid activityid = new Guid();

            if (Guid.TryParse(activity_id, out activityid))
            {
                using (DataLayer.DL_MongoPush obj = new DataLayer.DL_MongoPush())
                {
                    return obj.SyncActivityMapping(activityid, CreatedBy);
                }
            }
            else
            {
                return new DC_Message { StatusMessage = "Invalid ActivityID", StatusCode = ReadOnlyMessage.StatusCode.Danger };
            }
        }
        #endregion

        #region Supplier
        public DC_Message SyncSupplierMaster(string supplier_id, string CreatedBy)
        {
            Guid supplierid = new Guid();

            if (Guid.TryParse(supplier_id, out supplierid))
            {
                using (DataLayer.DL_MongoPush obj = new DataLayer.DL_MongoPush())
                {
                    return obj.SyncSupplierMaster(supplierid, CreatedBy);
                }
            }
            else
            {
                return new DC_Message { StatusMessage = "Invalid SupplierID", StatusCode = ReadOnlyMessage.StatusCode.Danger };
            }
        }
        #endregion

        #region port

        public DC_Message SyncPortMaster(string port_id, string CreatedBy)
        {
            Guid portid = new Guid();

            if (Guid.TryParse(port_id, out portid))
            {
                using (DataLayer.DL_MongoPush obj = new DataLayer.DL_MongoPush())
                {
                    return obj.SyncPortMaster(portid, CreatedBy);
                }
            }
            else
            {
                return new DC_Message { StatusMessage = "Invalid PortID", StatusCode = ReadOnlyMessage.StatusCode.Danger };
            }
        }
        #endregion

        #region state

        public DC_Message SyncStateMaster(string state_id, string CreatedBy)
        {
            Guid stateid = new Guid();

            if (Guid.TryParse(state_id, out stateid))
            {
                using (DataLayer.DL_MongoPush obj = new DataLayer.DL_MongoPush())
                {
                    return obj.SyncStateMaster(stateid, CreatedBy);
                }
            }
            else
            {
                return new DC_Message { StatusMessage = "Invalid StateID", StatusCode = ReadOnlyMessage.StatusCode.Danger };
            }
        }
        #endregion

        #region GetRefreshLog
        public List<DC_RefreshDistributionDataLog> GetRefreshDistributionLog()
        {
            using (DataLayer.DL_GetRefreshDistributionLog obj = new DataLayer.DL_GetRefreshDistributionLog())
            {
                return obj.GetRefreshDistributionLog();
            }
        }
        #endregion

        #region GetRefresh StaticHotelLog
        public List<DC_RefreshDistributionDataLog> GetRefreshStaticHotelLog()
        {
            using (DataLayer.DL_GetRefreshDistributionLog obj = new DataLayer.DL_GetRefreshDistributionLog())
            {
                return obj.GetRefreshStaticHotelLog();
            }
        }
        #endregion

        #region LoadSupplierData
        public List<DC_SupplierEntity> LoadSupplierData()
        {
            using (DataLayer.DL_GetRefreshDistributionLog obj = new DataLayer.DL_GetRefreshDistributionLog())
            {
                return obj.LoadSupplierData();
            }
        }
        #endregion

        #region state

        public DC_Message SyncSupplierStaticHotel(string log_id, string supplier_id, string CreatedBy)
        {
            Guid logid = new Guid();
            Guid gSupplier_Id;
            if (Guid.TryParse(log_id, out logid) && Guid.TryParse(supplier_id, out gSupplier_Id))
            {
                using (DataLayer.DL_MongoPush obj = new DataLayer.DL_MongoPush())
                {
                    return obj.SyncSupplierStaticHotel(logid, gSupplier_Id, CreatedBy);
                }
            }
            else
            {
                return new DC_Message { StatusMessage = "Invalid Supplier_id", StatusCode = ReadOnlyMessage.StatusCode.Danger };
            }
        }
        #endregion

        #region == ML Data Integration
        public DC_Message SyncMLAPIData(DC_Distribution_MLDataRQ _obj)
        {
            using (DataLayer.DL_GetRefreshDistributionLog obj = new DataLayer.DL_GetRefreshDistributionLog())
            {
                return obj.SyncMLAPIData(_obj);
            }
        }
        #endregion
    }
}
