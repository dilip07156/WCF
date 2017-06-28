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
    public class DC_GenericMasterDetails_ByIDOrName
    {
        [DataMember]
        public Guid? ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public EntityType ObjName { get; set; }
        [DataMember]
        public string Remark { get; set; }
        [DataMember]
        public string Optional1 { get; set; }
        [DataMember]
        public string Optional2 { get; set; }
        [DataMember]
        public DetailsWhatFor WhatFor { get; set; }

    }
    [DataContract]
    public enum EntityType
    {
        [EnumMember]
        supplier,
        [EnumMember]
        country,
        [EnumMember]
        city,
        [EnumMember]
        state,
        [EnumMember]
        product
    }
    [DataContract]
    public enum DetailsWhatFor
    {
        [EnumMember]
        CodeById,
        [EnumMember]
        RemarksForMapping,
        [EnumMember]
        IDByName,

    }
}
