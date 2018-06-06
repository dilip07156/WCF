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
        public string TLGXHotelId { get; set; }
        public string RoomId { get; set; }
        public string RoomView { get; set; }
        public string NoOfRooms { get; set; }
        public string RoomName { get; set; }
        public string NoOfInterconnectingRooms { get; set; }
        public string Description { get; set; }
        public string RoomSize { get; set; }
        public string RoomDecor { get; set; }
        public string Smoking { get; set; }
        public string FloorName { get; set; }
        public string FloorNumber { get; set; }
        public string MysteryRoom { get; set; }
        public string BathRoomType { get; set; }
        public string BedType { get; set; }
        public string CompanyRoomCategory { get; set; }
        public string RoomCategory { get; set; }
        public string Category { get; set; }
        public string CreateUser { get; set; }
        public string CreateDate { get; set; }
        public string EditUser { get; set; }
        public string EditDate { get; set; }
    }
}
