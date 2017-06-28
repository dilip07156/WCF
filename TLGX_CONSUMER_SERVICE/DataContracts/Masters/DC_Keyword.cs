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

        System.Guid _Keyword_Id;
        string _Keyword;
        Nullable<bool> _Missing;
        Nullable<bool> _Extra;
        Nullable<System.DateTime> _Create_Date;
        string _Create_User;
        Nullable<System.DateTime> _Edit_Date;
        string _Edit_User;
        string _Status;

        System.Guid _AliasKeywordAlias_Id;
        string _AliasValue;
        Nullable<System.DateTime> _AliasCreate_Date;
        string _AliasCreate_User;
        Nullable<System.DateTime> _AliasEdit_Date;
        string _AliasEdit_User;
        string _AliasStatus;


        [DataMember]
        public Guid Keyword_Id
        {
            get
            {
                return _Keyword_Id;
            }

            set
            {
                _Keyword_Id = value;
            }
        }
        [DataMember]
        public string Keyword
        {
            get
            {
                return _Keyword;
            }

            set
            {
                _Keyword = value;
            }
        }
        [DataMember]
        public bool? Missing
        {
            get
            {
                return _Missing;
            }

            set
            {
                _Missing = value;
            }
        }
        [DataMember]
        public bool? Extra
        {
            get
            {
                return _Extra;
            }

            set
            {
                _Extra = value;
            }
        }
        [DataMember]
        public DateTime? Create_Date
        {
            get
            {
                return _Create_Date;
            }

            set
            {
                _Create_Date = value;
            }
        }
        [DataMember]
        public string Create_User
        {
            get
            {
                return _Create_User;
            }

            set
            {
                _Create_User = value;
            }
        }
        [DataMember]
        public DateTime? Edit_Date
        {
            get
            {
                return _Edit_Date;
            }

            set
            {
                _Edit_Date = value;
            }
        }
        [DataMember]
        public string Edit_User
        {
            get
            {
                return _Edit_User;
            }

            set
            {
                _Edit_User = value;
            }
        }
        [DataMember]
        public string Status
        {
            get
            {
                return _Status;
            }

            set
            {
                _Status = value;
            }
        }

        [DataMember]
        public Guid AliasKeywordAlias_Id
        {
            get
            {
                return _AliasKeywordAlias_Id;
            }

            set
            {
                _AliasKeywordAlias_Id = value;
            }
        }
        [DataMember]
        public string AliasValue
        {
            get
            {
                return _AliasValue;
            }

            set
            {
                _AliasValue = value;
            }
        }
        [DataMember]
        public DateTime? AliasCreate_Date
        {
            get
            {
                return _AliasCreate_Date;
            }

            set
            {
                _AliasCreate_Date = value;
            }
        }
        [DataMember]
        public string AliasCreate_User
        {
            get
            {
                return _AliasCreate_User;
            }

            set
            {
                _AliasCreate_User = value;
            }
        }
        [DataMember]
        public DateTime? AliasEdit_Date
        {
            get
            {
                return _AliasEdit_Date;
            }

            set
            {
                _AliasEdit_Date = value;
            }
        }
        [DataMember]
        public string AliasEdit_User
        {
            get
            {
                return _AliasEdit_User;
            }

            set
            {
                _AliasEdit_User = value;
            }
        }
        [DataMember]
        public string AliasStatus
        {
            get
            {
                return _AliasStatus;
            }

            set
            {
                _AliasStatus = value;
            }
        }
    }

    [DataContract]
    public class DC_Keyword_RQ
    {
        System.Guid? _Keyword_Id;
        System.Guid? _AliasKeywordAlias_Id;
        string _systemWord;
        string _Alias;
        string _Status;

        [DataMember]
        public string SystemWord
        {
            get
            {
                return _systemWord;
            }

            set
            {
                _systemWord = value;
            }
        }

        [DataMember]
        public string Alias
        {
            get
            {
                return _Alias;
            }

            set
            {
                _Alias = value;
            }
        }

        [DataMember]
        public string Status
        {
            get
            {
                return _Status;
            }

            set
            {
                _Status = value;
            }
        }
        [DataMember]
        public Guid? Keyword_Id
        {
            get
            {
                return _Keyword_Id;
            }

            set
            {
                _Keyword_Id = value;
            }
        }
        [DataMember]
        public Guid? AliasKeywordAlias_Id
        {
            get
            {
                return _AliasKeywordAlias_Id;
            }

            set
            {
                _AliasKeywordAlias_Id = value;
            }
        }
    }

    [DataContract]
    public class DC_keyword_alias
    {
        [DataMember]
        public Guid KeywordAlias_Id { get; set; }
        [DataMember]
        public Guid Keyword_Id { get; set; }
        [DataMember]
        public string Keyword { get; set; }
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
    }

}
