using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.ML
{
    public class DC_ML_DL_SupplierAcco
    {
        public string Mode { get; set; }
        public string BatchId { get; set; }
        public string Transaction { get; set; }
        public List<DC_ML_DL_SupplierAcco_Data> SupplierAccommodationData { get; set; }
    }

    public class DC_ML_DL_SupplierAcco_Data
    {
        public string AccommodationId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierId { get; set; }
        public int TLGXHotelId { get; set; }
        public string AccommodationName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string EditDate { get; set; }
        public string Edituser { get; set; }
    }
}
