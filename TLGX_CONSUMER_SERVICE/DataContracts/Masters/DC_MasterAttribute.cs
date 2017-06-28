using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace DataContracts.Masters
{
    [DataContract]
    public class DC_MasterAttribute
    {
        [DataMember]
        public Guid MasterAttributeValue_Id { get; set; }
        [DataMember]
        public Guid? MasterAttribute_Id { get; set; }
        [DataMember]
        public string AttributeValue { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string MasterFor { get; set; }
        [DataMember]
        public Guid? ParentAttributeValue_Id { get; set; }
    }

    [DataContract]
    public class DC_M_masterattribute
    {
        [DataMember]
        public Guid MasterAttribute_Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string MasterFor { get; set; }
        [DataMember]
        public string ParentAttributeName { get; set; }
        [DataMember]
        public Guid? ParentAttribute_Id { get; set; }
        [DataMember]
        public string OTA_CodeTableCode { get; set; }
        [DataMember]
        public string OTA_CodeTableName { get; set; }
        [DataMember]
        public string IsActive { get; set; }
        [DataMember]
        public string Action { get; set; }
        [DataMember]
        public int TotalRecords { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
        [DataMember]
        public int? PageNo { get; set; }


    }

    [DataContract]
    public class DC_M_masterattributevalue
    {
        [DataMember]
        public Guid MasterAttributeValue_Id { get; set; }
        [DataMember]
        public Guid? MasterAttribute_Id { get; set; }
        [DataMember]
        public Guid? ParentAttributeValue_Id { get; set; }
        [DataMember]
        public string ParentAttributeValue { get; set; }
        [DataMember]
        public string AttributeValue { get; set; }
        [DataMember]
        public string MasterAttribute_Name { get; set; }
        [DataMember]
        public string OTA_CodeTableValue { get; set; }
        [DataMember]
        public string IsActive { get; set; }
        [DataMember]
        public string Action { get; set; }
        [DataMember]
        public int TotalCount { get; set; }

    }

    [DataContract]
    public class DC_M_masterparentattributes
    {
        [DataMember]
        public Guid ParentAttribute_Id { get; set; }

        [DataMember]
        public string ParentAttributeName { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
    }


    [DataContract]
    public class DC_M_masterattributelists
    {
        List<DC_M_masterattribute> _masterattribute;
        List<DC_M_masterattributevalue> _masterattributevalue;

        [DataMember]
        public List<DC_M_masterattribute> MasterAttributes
        {
            get
            {
                return _masterattribute;
            }

            set
            {
                _masterattribute = value;
            }
        }

        [DataMember]
        public List<DC_M_masterattributevalue> MasterAttributeValues
        {
            get
            {
                return _masterattributevalue;
            }

            set
            {
                _masterattributevalue = value;
            }
        }
    }
}
