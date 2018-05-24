using System;
using System.Runtime.Serialization;


namespace DataContracts.UploadStaticData
{

    [DataContract]
    public class DC_SupplierImportAttributeValues
    {
        [DataMember]
        public System.Guid SupplierImportAttributeValue_Id { get; set; }

        [DataMember]
        public System.Guid SupplierImportAttribute_Id { get; set; }

        [DataMember]
        public string AttributeType { get; set; }

        [DataMember]
        public string AttributeName { get; set; }

        [DataMember]
        public Guid? AttributeValue_ID { get; set; }

        [DataMember]
        public string AttributeValue { get; set; }

        [DataMember]
        public string STATUS { get; set; }

        [DataMember]
        public Nullable<System.DateTime> CREATE_DATE { get; set; }

        [DataMember]
        public string CREATE_USER { get; set; }

        [DataMember]
        public Nullable<System.DateTime> EDIT_DATE { get; set; }

        [DataMember]
        public string EDIT_USER { get; set; }

        [DataMember]
        public int TotalRecords { get; set; }

        [DataMember]
        public int Priority { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string AttributeValueType { get; set; }

        [DataMember]
        public string Comparison { get; set; }
    }

    [DataContract]
    public class DC_SupplierImportAttributeValues_RQ
    {

        [DataMember]
        public Nullable<System.Guid> SupplierImportAttributeValue_Id { get; set; }

        [DataMember]
        public Nullable<System.Guid> SupplierImportAttribute_Id { get; set; }

        [DataMember]
        public Nullable<System.Guid> Supplier_Id { get; set; }

        [DataMember]
        public string Supplier { get; set; }

        [DataMember]
        public string Entity { get; set; }

        [DataMember]
        public string AttributeType { get; set; }

        [DataMember]
        public string AttributeName { get; set; }

        [DataMember]
        public string AttributeValue { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string StatusExcept { get; set; }

        [DataMember]
        public int PageNo { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public int? Priority { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string For { get; set; }
    }
}
