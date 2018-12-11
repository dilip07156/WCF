using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.ML
{
    public class DC_ML_DL_MasterAccoRoomFacility
    {
        public string Mode { get; set; }
        public string BatchId { get; set; }
        public string Transaction { get; set; }
        public List<DC_ML_DL_MasterAccoRoomFacility_Data> MasterAccommodationRoomFacilities { get; set; }
    }

    public class DC_ML_DL_MasterAccoRoomFacility_Data
    {
        public string AccommodationRoomFacilityId { get; set; }
        public string AccommodationId { get; set; }
        public string AccommodationRoomInfoId { get; set; }
        public string AmenityType { get; set; }
        public string AmenityName { get; set; }
        public string Description { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string EditDate { get; set; }
        public string Edituser { get; set; }

        public int? TLGXCommonHotelId { get; set; }
        public string TLGXAccoId { get; set; }
        public string TLGXAccoRoomId { get; set; }
    }
}
