using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    [DataContract]
    public class DC_GooglePlaceNearByWithAccoID
    {
        [DataMember]
        public Guid Accomodation_Id { get; set; }
        [DataMember]
        public string PlaceCategory { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public List<DC_GooglePlaceNearBy> GooglePlaceNearBy { get; set; }
    }
    [DataContract]
    public class DC_GooglePlaceNearBy
    {
        [DataMember]
        public GeometryGooglePlaceNearBy geometry { get; set; }
        [DataMember]
        public string icon { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public OpeningHoursGooglePlaceNearBy opening_hours { get; set; }
        [DataMember]
        public List<PhotoGooglePlaceNearBy> photos { get; set; }
        [DataMember]
        public string place_id { get; set; }
        [DataMember]
        public double rating { get; set; }
        [DataMember]
        public string reference { get; set; }
        [DataMember]
        public string scope { get; set; }
        [DataMember]
        public List<string> types { get; set; }
        [DataMember]
        public string vicinity { get; set; }
        [DataMember]
        public List<object> html_attributions { get; set; }
    }
    [DataContract]
    public class LocationGooglePlaceNearBy
    {
        [DataMember]
        public double lat { get; set; }
        [DataMember]
        public double lng { get; set; }
    }
    [DataContract]
    public class ViewportGooglePlaceNearBy
    {
        [DataMember]
        public double south { get; set; }
        [DataMember]
        public double west { get; set; }
        [DataMember]
        public double north { get; set; }
        [DataMember]
        public double east { get; set; }
    }
    [DataContract]
    public class GeometryGooglePlaceNearBy
    {
        [DataMember]
        public Location location { get; set; }
        [DataMember]
        public Viewport viewport { get; set; }
    }
    [DataContract]
    public class OpeningHoursGooglePlaceNearBy
    {
        [DataMember]
        public bool open_now { get; set; }
        [DataMember]
        public List<object> weekday_text { get; set; }
    }
    [DataContract]
    public class PhotoGooglePlaceNearBy
    {
        [DataMember]
        public int height { get; set; }
        [DataMember]
        public List<string> html_attributions { get; set; }
        [DataMember]
        public int width { get; set; }
    }
}





