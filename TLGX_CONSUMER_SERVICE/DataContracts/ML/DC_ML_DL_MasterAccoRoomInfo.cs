using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.ML
{
    public class DC_ML_DL_MasterAccoRoomInfo
    {
        public string Mode { get; set; }
        public string BatchId { get; set; }
        public string Transaction { get; set; }
        public List<DC_ML_DL_MasterAccoRoomInfo_Data> MasterAccommodationRoomInformation { get; set; }
    }

    public class DC_ML_DL_MasterAccoRoomInfo_Data
    {
        public string AccommodationRoomInfoId { get; set; }
        public string AccommodationId { get; set; }
        public int? TLGXCommonHotelId { get; set; }
        public string RoomId { get; set; }
        public string RoomView { get; set; }
        public int? NoOfRooms { get; set; }
        public string RoomName { get; set; }
        public int? NoOfInterconnectingRooms { get; set; }
        public string Description { get; set; }
        public string RoomSize { get; set; }
        public string RoomDecor { get; set; }
        public bool? Smoking { get; set; }
        public string FloorName { get; set; }
        public string FloorNumber { get; set; }
        public bool? MysteryRoom { get; set; }
        public string BathRoomType { get; set; }
        public string BedType { get; set; }
        public string CompanyRoomCategory { get; set; }
        public string RoomCategory { get; set; }
        public string Category { get; set; }
        public string CreateUser { get; set; }
        public string CreateDate { get; set; }
        public string EditUser { get; set; }
        public string EditDate { get; set; }
        public string RoomInfo_TX { get; set; }
        public List<ExtractedAttributes> ExtractedAttributes { get; set; }
        public string TLGXAccoRoomId { get; set; }
        public string TLGXAccoId { get; set; }

    }

    public class DC_ML_DL_AccoRoom_ExtendedAttributes_Data
    {
        //public Guid Accommodation_RoomInfo_Attribute_Id { get; set; }
        public Guid Accommodation_RoomInfo_Id { get; set; }
        public string Accommodation_RoomInfo_Attribute { get; set; }
        public string SystemAttributeKeyword { get; set; }
    }

    public class ExtractedAttributes
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
