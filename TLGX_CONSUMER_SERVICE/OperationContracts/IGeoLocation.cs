using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace OperationContracts
{
    [ServiceContract]
    public interface IGeoLocation
    {
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "GetGeoLocation/ByAddress", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_GeoLocation GetGeoLocationByAddress(DataContracts.DC_Address.DC_Address_Physical PA);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "GetGeoLocation/ByLatLng", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_GeoLocation GetGeoLocationByLatLng(DataContracts.DC_Address.DC_Address_GeoCode AG);
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "GetNearByPlace/ByLatLng", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.DC_Accommodation_NearbyPlaces> GetNearByPlacesByLatLng(DataContracts.DC_Address.DC_Address_GeoCodeForNearBy AG);

    }
}
