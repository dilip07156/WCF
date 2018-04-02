using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace DataContracts.STG
{
    [DataContract]
    public class List_DC_stg_SupplierHotelRoomMapping
    {
        [DataMember]
        public List<DC_stg_SupplierHotelRoomMapping> l_DC_stg_SupplierHotelRoomMapping;
    }

    [DataContract]
    public class DC_stg_SupplierHotelRoomMapping
    {
        [DataMember]
        public System.Guid stg_SupplierHotelRoomMapping_Id { get; set; }
        [DataMember]
        public string SupplierID { get; set; }
        [DataMember]
        public string SupplierName { get; set; }
        [DataMember]
        public string SupplierProductId { get; set; }
        [DataMember]
        public string SupplierProductName { get; set; }
        [DataMember]
        public string BathRoomType { get; set; }
        [DataMember]
        public string RoomDescription { get; set; }
        [DataMember]
        public int? MaxAdults { get; set; }
        [DataMember]
        public int? MaxChild { get; set; }
        [DataMember]
        public int? MaxInfant { get; set; }
        [DataMember]
        public int? MaxGuestOccupancy { get; set; }
        [DataMember]
        public string SupplierRoomId { get; set; }
        [DataMember]
        public string RoomViewCode { get; set; }
        [DataMember]
        public string SupplierRoomTypeCode { get; set; }
        [DataMember]
        public string RoomSize { get; set; }
        [DataMember]
        public string FloorName { get; set; }
        [DataMember]
        public int? FloorNumber { get; set; }
        [DataMember]
        public string SupplierProvider { get; set; }
        [DataMember]
        public string Amenities { get; set; }
        [DataMember]
        public string Quantity { get; set; }
        [DataMember]
        public string RoomLocationCode { get; set; }
        [DataMember]
        public int? ChildAge { get; set; }
        [DataMember]
        public string ExtraBed { get; set; }
        [DataMember]
        public string Bedrooms { get; set; }
        [DataMember]
        public string Smoking { get; set; }
        [DataMember]
        public string PromotionalVendorCode { get; set; }
        [DataMember]
        public string RoomName { get; set; }
        [DataMember]
        public string SupplierRoomCategory { get; set; }
        [DataMember]
        public string SupplierRoomCategoryId { get; set; }
        [DataMember]
        public string BeddingConfig { get; set; }
        [DataMember]
        public string BedTypeCode { get; set; }
        [DataMember]
        public string RatePlan { get; set; }
        [DataMember]
        public string RatePlanCode { get; set; }
        [DataMember]
        public string TX_RoomName { get; set; }        

        [DataMember]
        public Nullable<System.Guid> Supplier_Id { get; set; }
        [DataMember]
        public Guid? SupplierImportFile_Id { get; set; }

    }

    [DataContract]
    public class DC_stg_SupplierHotelRoomMapping_RQ
    {
        [DataMember]
        public Nullable<System.Guid> stg_SupplierHotelRoomMapping_Id { get; set; }

        [DataMember]
        public Nullable<System.Guid> Supplier_Id { get; set; }

        [DataMember]
        public Guid? SupplierImportFile_Id { get; set; }
        [DataMember]
        public string SupplierName { get; set; }
        [DataMember]
        public string SupplierID { get; set; }
        [DataMember]
        public string SupplierProductId { get; set; }
        [DataMember]
        public string SupplierProductName { get; set; }
        [DataMember]
        public int PageNo { get; set; }

        [DataMember]
        public int PageSize { get; set; }
        [DataMember]
        public string SupplierRoomId { get; set; }
        [DataMember]
        public string SupplierRoomTypeCode { get; set; }
        [DataMember]
        public string RoomName { get; set; }
        [DataMember]
        public string SupplierRoomCategory { get; set; }
        [DataMember]
        public string SupplierRoomCategoryId { get; set; }
    }
}
