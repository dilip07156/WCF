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
        public DataContracts.DC_GeoLocation GetGeoLocationByAddress(DataContracts.DC_Address.DC_Address_Physical PA)
        {
            using (BusinessLayer.BL_GeoLocation obj = new BL_GeoLocation())
            {
                return obj.GetGeoLocationByAddress(PA);
            }
        }

        public DataContracts.DC_GeoLocation GetGeoLocationByLatLng(DataContracts.DC_Address.DC_Address_GeoCode AG)
        {
            using (BusinessLayer.BL_GeoLocation obj = new BL_GeoLocation())
            {
                return obj.GetGeoLocationByLatLng(AG);
            }
        }
        public List<DataContracts.DC_Accommodation_NearbyPlaces> GetNearByPlacesByLatLng(DataContracts.DC_Address.DC_Address_GeoCodeForNearBy AG)
        {
            using (BusinessLayer.BL_GeoLocation obj = new BL_GeoLocation())
            {
                return obj.GetNearByPlacesByLatLng(AG);
            }
        }
    }
}