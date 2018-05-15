﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OperationContracts;
using DataContracts;
using BusinessLayer;
using DataContracts.Masters;

namespace ConsumerSvc
{
    public partial class Consumer : IConsumer
    {
        #region refresh Distribution Data

        public DC_Message SyncCountryMaster(string country_id, string CreatedBy)
        {
            using (BusinessLayer.BL_RefreshDistributionData obj = new BL_RefreshDistributionData())
            {
                return obj.SyncCountryMaster(country_id, CreatedBy);
            }
        }

        public DC_Message SyncCountryMapping(string country_id, string CreatedBy)
        {
            using (BusinessLayer.BL_RefreshDistributionData obj = new BL_RefreshDistributionData())
            {
                return obj.SyncCountryMapping(country_id, CreatedBy);
            }
        }
        #endregion

        #region city
        public DC_Message SyncCityMaster(string city_id, string CreatedBy)
        {
            using (BusinessLayer.BL_RefreshDistributionData obj = new BL_RefreshDistributionData())
            {
                return obj.SyncCityMaster(city_id, CreatedBy);
            }
        }

        public DC_Message SyncCityMapping(string city_id, string CreatedBy)
        {
            using (BusinessLayer.BL_RefreshDistributionData obj = new BL_RefreshDistributionData())
            {
                return obj.SyncCityMapping(city_id, CreatedBy);
            }
        }
        #endregion

        #region Hotel

        public DC_Message SyncHotelMapping(string hotel_id, string CreatedBy)
        {
            using (BusinessLayer.BL_RefreshDistributionData obj = new BL_RefreshDistributionData())
            {
                return obj.SyncHotelMapping(hotel_id, CreatedBy);
            }
        }

        #endregion

        #region Activity

        public DC_Message SyncActivityMapping(string activity_id, string CreatedBy)
        {
            using (BusinessLayer.BL_RefreshDistributionData obj = new BL_RefreshDistributionData())
            {
                return obj.SyncActivityMapping(activity_id, CreatedBy);
            }
        }

        #endregion

        #region Supplier

        public DC_Message SyncSupplierMaster(string supplier_id, string CreatedBy)
        {
            using (BusinessLayer.BL_RefreshDistributionData obj = new BL_RefreshDistributionData())
            {
                return obj.SyncSupplierMaster(supplier_id, CreatedBy);
            }
        }

        #endregion

        #region port

        public DC_Message SyncPortMaster(string port_id, string CreatedBy)
        {
            using (BusinessLayer.BL_RefreshDistributionData obj = new BL_RefreshDistributionData())
            {
                return obj.SyncPortMaster(port_id, CreatedBy);
            }
        }
        #endregion

        #region State

        public DC_Message SyncStateMaster(string state_id, string CreatedBy)
        {
            using (BusinessLayer.BL_RefreshDistributionData obj = new BL_RefreshDistributionData())
            {
                return obj.SyncStateMaster(state_id, CreatedBy);
            }
        }
        #endregion

        #region GetRefreshLog
        public List<DC_RefreshDistributionDataLog> GetRefreshDistributionLog()
        {
            using (BusinessLayer.BL_RefreshDistributionData obj = new BL_RefreshDistributionData())
            {
                return obj.GetRefreshDistributionLog();
            }
        }
        #endregion

        #region GetRefreshStaticHotelLog
        public List<DC_RefreshDistributionDataLog> GetRefreshStaticHotelLog()
        {
            using (BusinessLayer.BL_RefreshDistributionData obj = new BL_RefreshDistributionData())
            {
                return obj.GetRefreshStaticHotelLog();
            }
        }
        #endregion

        #region 
        public List<DC_SupplierEntity> LoadSupplierData()
        {
            using (BusinessLayer.BL_RefreshDistributionData obj = new BL_RefreshDistributionData())
            {
                return obj.LoadSupplierData();
            }
        }
        #endregion

        #region Supplier Entity hotel

        public DC_Message SyncSupplierStaticHotel(string log_id,string supplier_id, string CreatedBy)
        {
            using (BusinessLayer.BL_RefreshDistributionData obj = new BL_RefreshDistributionData())
            {
                return obj.SyncSupplierStaticHotel(log_id ,supplier_id, CreatedBy);
            }
        }
        #endregion
    }
}