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
        public string Icon { get; set; }

        [DataMember]
        public string EntityFor { get; set; }

        [DataMember]
        public string AttributeType { get; set; }
        [DataMember]
        public string AttributeLevel { get; set; }
        [DataMember]
        public string AttributeSubLevel { get; set; }
        [DataMember]
        public string AttributeSubLevelValue { get; set; }

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
        public string EntityFor { get; set; }

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
        [DataMember]
        public int NewHits { get; set; }
    }

    [DataContract]
    public class DC_keywordApply_RQ
    {
        [DataMember]
        public Guid? File_Id { get; set; } //If Running TTFU for a specific file uploaded for getting the static data

        [DataMember]
        public int CurrentBatch { get; set; }//If Running TTFU for a specific file uploaded for getting the static data in batch

        [DataMember]
        public int TotalBatch { get; set; }//If Running TTFU for a specific file uploaded for getting the static data in batch

        [DataMember]
        public string KeywordEntity { get; set; } //e.g. Hotel, RoomType

        [DataMember]
        public string SearchTable { get; set; } //Search Table Name

        [DataMember]
        public string TakeColumn { get; set; } //Field needs to be TTFU

        [DataMember]
        public string UpdateColumn { get; set; } //After TTFU update field value to target column

        [DataMember]
        public string[] TablePrimaryKeys { get; set; } //Primary keys values which should be in where field

        [DataMember]
        public string EditUser { get; set; } //Requesting User
    }

    [DataContract]
    public class DC_keywordApplyToTarget
    {
        [DataMember]
        public string PrimaryKey { get; set; }

        [DataMember]
        public string TableName { get; set; }

        [DataMember]
        public string SourceColumnName { get; set; }

        [DataMember]
        public string SourceColumnValue { get; set; }

        [DataMember]
        public string TargetColumnName { get; set; }

        [DataMember]
        public string TargetColumnValue { get; set; }

        [DataMember]
        public string EditUser { get; set; }
    }

    [DataContract]
    public class DC_KeyWordReRun
    {
        [DataMember]
        public Guid RowId { get; set; }

        [DataMember]
        public string OriginalValue { get; set; }

        [DataMember]
        public string CountryName { get; set; }

        [DataMember]
        public string CityName { get; set; }

        [DataMember]
        public string TxValue { get; set; }

        [DataMember]
        public string SxValue { get; set; }
    }
}
