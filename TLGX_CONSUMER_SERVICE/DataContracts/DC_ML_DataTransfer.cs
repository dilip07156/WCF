using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    [DataContract]
    public class DC_ML_DataTransfer
    {
    }
    [DataContract]
    public class DC_ML_Acco_Data
    {
        [DataMember]
        public Guid AccommodationId { get; set; }
        [DataMember]
        public int? TLGXHotelId { get; set; }
        [DataMember]
        public string AccommodationName { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public DateTime? CreateDate { get; set; }
        [DataMember]
        public string CreateUser { get; set; }
        [DataMember]
        public DateTime? EditDate { get; set; }
        [DataMember]
        public string Edituser { get; set; }
    }

    [DataContract]
    public class DC_ML_MasterAccoRoomFacility_Data
    {
        [DataMember]
        public Guid Accommodation_RoomFacility_Id { get; set; }
        [DataMember]
        public Guid Accommodation_Id { get; set; }
        [DataMember]
        public Guid Accommodation_RoomInfo_Id { get; set; }
        [DataMember]
        public string AmenityType { get; set; }
        [DataMember]
        public string AmenityName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string Edit_user { get; set; }
    }

    [DataContract]
    public class DC_ML_MasterAccoRoomInfo_Data
    {
        [DataMember]
        public Guid Accommodation_RoomInfo_Id { get; set; }
        [DataMember]
        public Guid Accommodation_Id { get; set; }
        [DataMember]
        public int Legacy_Htl_Id { get; set; }
        [DataMember]
        public string RoomId { get; set; }
        [DataMember]
        public string RoomView { get; set; }
        [DataMember]
        public int NoOfRooms { get; set; }
        [DataMember]
        public string RoomName { get; set; }
        [DataMember]
        public int NoOfInterconnectingRooms { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string RoomSize { get; set; }
        [DataMember]
        public string RoomDecor { get; set; }
        [DataMember]
        public bool Smoking { get; set; }
        [DataMember]
        public string FloorName { get; set; }
        [DataMember]
        public string FloorNumber { get; set; }
        [DataMember]
        public bool MysteryRoom { get; set; }
        [DataMember]
        public string BathRoomType { get; set; }
        [DataMember]
        public string BedType { get; set; }
        [DataMember]
        public string CompanyRoomCategory { get; set; }
        [DataMember]
        public string RoomCategory { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
    }
}
