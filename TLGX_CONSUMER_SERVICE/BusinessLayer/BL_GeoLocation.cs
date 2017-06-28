using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
   public class BL_GeoLocation : IDisposable
    {
        public void Dispose()
        { }

        public DataContracts.DC_GeoLocation GetGeoLocationByAddress(DataContracts.DC_Address.DC_Address_Physical PA)
        {
            using (DataLayer.DL_GeoLocation obj = new DataLayer.DL_GeoLocation())
            {
                return obj.GetGeoLocationByAddress(PA);
            }
        }

        public DataContracts.DC_GeoLocation GetGeoLocationByLatLng(DataContracts.DC_Address.DC_Address_GeoCode AG)
        {
            using (DataLayer.DL_GeoLocation obj = new DataLayer.DL_GeoLocation())
            {
                return obj.GetGeoLocationByLatLng(AG);
            }
        }

        public List<DataContracts.DC_Accommodation_NearbyPlaces> GetNearByPlacesByLatLng(DataContracts.DC_Address.DC_Address_GeoCodeForNearBy AG)
        {
            using (DataLayer.DL_GeoLocation obj = new DataLayer.DL_GeoLocation())
            {
                return obj.GetNearByPlacesByLatLng(AG);
            }
        }
    }
}
