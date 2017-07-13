using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Masters
{
    [DataContract]
    public class DC_Keyword
    {
        [DataMember]
        public System.Guid Keyword_Id { get; set; }

        [DataMember]
        public string Keyword { get; set; }

        [DataMember]
        public Nullable<bool> Missing { get; set; }

        [DataMember]
        public Nullable<bool> Extra { get; set; }

        [DataMember]
        public Nullable<System.DateTime> Create_Date { get; set; }

        [DataMember]
        public string Create_User { get; set; }

        [DataMember]
        public Nullable<System.DateTime> Edit_Date { get; set; }

        [DataMember]
        public string Edit_User { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public Nullable<bool> Attribute { get; set; }

        [DataMember]
        public int Sequence { get; set; }

        [DataMember]
        public int TotalRecords { get; set; }

        [DataMember]
        public List<DC_keyword_alias> Alias { get; set; }

    }

    [DataContract]
    public class DC_Keyword_RQ
    {
        [DataMember]
        public System.Guid? Keyword_Id { get; set; }
        [DataMember]
        public string systemWord { get; set; }
        [DataMember]
        public string Alias { get; set; }
        [DataMember]
        public bool? Attribute { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int PageNo { get; set; }
        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public string AliasStatus { get; set; }
        //[DataMember]
        //public int AliasPageNo { get; set; }
        //[DataMember]
        //public int AliasPageSize { get; set; }

    }

    [DataContract]
    public class DC_keyword_alias
    {
        [DataMember]
        public Guid KeywordAlias_Id { get; set; }
        [DataMember]
        public Guid Keyword_Id { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int Sequence { get; set; }
        [DataMember]
        public int TotalRecords { get; set; }
        [DataMember]
        public int NoOfHits { get; set; }
    }

}
