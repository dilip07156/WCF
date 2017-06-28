using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace DataContracts.Mapping
{
    [DataContract]
    public class DC_Accomodation_SupplierRoomTypeMapping
    {

        System.Guid _Accommodation_SupplierRoomTypeMapping_Id;
        Nullable<System.Guid> _Accommodation_Id;
        Nullable<System.Guid> _Supplier_Id;
        string _SupplierName;
        string _SupplierRoomCategory;
        string _SupplierRoomTypeCode;
        string _BathRoomType;
        string _BedTypeCode;
        string _Description;
        Nullable<int> _MaxAdults;
        Nullable<int> _MaxChild;
        Nullable<int> _MaxInfant;
        Nullable<int> _MaxGuest;
        string _SupplierRoomId;
        string _RoomViewCode;
        string _RoomTypeCode;
        string _SizeMeasurement;
        string _FloorName;
        string _FloorNumber;
        string _SupplierHotelCode;
        string _SupplierHotelName;
        string _SupplierProvider;
        string _Amenities;
        Nullable<int> _Quantity;
        string _RoomLocationCode;
        Nullable<System.Guid> _Accommodation_RoomInfo_Id;

        [DataMember]
        public Guid Accommodation_SupplierRoomTypeMapping_Id
        {
            get
            {
                return _Accommodation_SupplierRoomTypeMapping_Id;
            }

            set
            {
                _Accommodation_SupplierRoomTypeMapping_Id = value;
            }
        }

        [DataMember]
        public Guid? Accommodation_Id
        {
            get
            {
                return _Accommodation_Id;
            }

            set
            {
                _Accommodation_Id = value;
            }
        }

        [DataMember]
        public Guid? Supplier_Id
        {
            get
            {
                return _Supplier_Id;
            }

            set
            {
                _Supplier_Id = value;
            }
        }

        [DataMember]
        public string SupplierName
        {
            get
            {
                return _SupplierName;
            }

            set
            {
                _SupplierName = value;
            }
        }

        [DataMember]
        public string SupplierRoomCategory
        {
            get
            {
                return _SupplierRoomCategory;
            }

            set
            {
                _SupplierRoomCategory = value;
            }
        }

        [DataMember]
        public string SupplierRoomTypeCode
        {
            get
            {
                return _SupplierRoomTypeCode;
            }

            set
            {
                _SupplierRoomTypeCode = value;
            }
        }

        [DataMember]
        public string BathRoomType
        {
            get
            {
                return _BathRoomType;
            }

            set
            {
                _BathRoomType = value;
            }
        }

        [DataMember]
        public string BedTypeCode
        {
            get
            {
                return _BedTypeCode;
            }

            set
            {
                _BedTypeCode = value;
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
            }
        }

        [DataMember]
        public int? MaxAdults
        {
            get
            {
                return _MaxAdults;
            }

            set
            {
                _MaxAdults = value;
            }
        }

        [DataMember]
        public int? MaxChild
        {
            get
            {
                return _MaxChild;
            }

            set
            {
                _MaxChild = value;
            }
        }

        [DataMember]
        public int? MaxInfant
        {
            get
            {
                return _MaxInfant;
            }

            set
            {
                _MaxInfant = value;
            }
        }

        [DataMember]
        public int? MaxGuest
        {
            get
            {
                return _MaxGuest;
            }

            set
            {
                _MaxGuest = value;
            }
        }

        [DataMember]
        public string SupplierRoomId
        {
            get
            {
                return _SupplierRoomId;
            }

            set
            {
                _SupplierRoomId = value;
            }
        }

        [DataMember]
        public string RoomViewCode
        {
            get
            {
                return _RoomViewCode;
            }

            set
            {
                _RoomViewCode = value;
            }
        }

        [DataMember]
        public string RoomTypeCode
        {
            get
            {
                return _RoomTypeCode;
            }

            set
            {
                _RoomTypeCode = value;
            }
        }

        [DataMember]
        public string SizeMeasurement
        {
            get
            {
                return _SizeMeasurement;
            }

            set
            {
                _SizeMeasurement = value;
            }
        }

        [DataMember]
        public string FloorName
        {
            get
            {
                return _FloorName;
            }

            set
            {
                _FloorName = value;
            }
        }

        [DataMember]
        public string FloorNumber
        {
            get
            {
                return _FloorNumber;
            }

            set
            {
                _FloorNumber = value;
            }
        }

        [DataMember]
        public string SupplierHotelCode
        {
            get
            {
                return _SupplierHotelCode;
            }

            set
            {
                _SupplierHotelCode = value;
            }
        }

        [DataMember]
        public string SupplierHotelName
        {
            get
            {
                return _SupplierHotelName;
            }

            set
            {
                _SupplierHotelName = value;
            }
        }

        [DataMember]
        public string SupplierProvider
        {
            get
            {
                return _SupplierProvider;
            }

            set
            {
                _SupplierProvider = value;
            }
        }

        [DataMember]
        public string Amenities
        {
            get
            {
                return _Amenities;
            }

            set
            {
                _Amenities = value;
            }
        }

        [DataMember]
        public int? Quantity
        {
            get
            {
                return _Quantity;
            }

            set
            {
                _Quantity = value;
            }
        }

        [DataMember]
        public string RoomLocationCode
        {
            get
            {
                return _RoomLocationCode;
            }

            set
            {
                _RoomLocationCode = value;
            }
        }

        [DataMember]
        public Guid? Accommodation_RoomInfo_Id
        {
            get
            {
                return _Accommodation_RoomInfo_Id;
            }

            set
            {
                _Accommodation_RoomInfo_Id = value;
            }
        }
    }
}
