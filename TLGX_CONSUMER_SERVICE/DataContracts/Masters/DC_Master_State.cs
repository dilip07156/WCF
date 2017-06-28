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
    public class DC_Master_State
    {
        Guid _State_Id;
        string _State_Name;
        string _State_Code;
        Guid _Country_Id;
        DateTime? _Create_Date;
        string _Create_User;
        DateTime? _Edit_Date;
        string _Edit_User;
        string _StateName_LocalLanguage;
        int _TotalRecords;

        [DataMember]
        public Guid State_Id
        {
            get
            {
                return _State_Id;
            }

            set
            {
                _State_Id = value;
            }
        }
        [DataMember]
        public string State_Name
        {
            get
            {
                return _State_Name;
            }

            set
            {
                _State_Name = value;
            }
        }
        [DataMember]
        public string State_Code
        {
            get
            {
                return _State_Code;
            }

            set
            {
                _State_Code = value;
            }
        }
        [DataMember]
        public Guid Country_Id
        {
            get
            {
                return _Country_Id;
            }

            set
            {
                _Country_Id = value;
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
        public string StateName_LocalLanguage
        {
            get
            {
                return _StateName_LocalLanguage;
            }

            set
            {
                _StateName_LocalLanguage = value;
            }
        }
        [DataMember]
        public int TotalRecords
        {
            get
            {
                return _TotalRecords;
            }

            set
            {
                _TotalRecords = value;
            }
        }
    }

    [DataContract]
    public class DC_State_Search_RQ
    {
        Guid? _Country_Id;
        string _Country_Name;
        Guid? _State_Id;
        string _State_Name;
        string _State_code;
        int? _PageNo;
        int? _PageSize;
        string _AlphaPageIndex;

        [DataMember]
        public Guid? Country_Id
        {
            get
            {
                return _Country_Id;
            }

            set
            {
                _Country_Id = value;
            }
        }

        [DataMember]
        public string Country_Name
        {
            get
            {
                return _Country_Name;
            }

            set
            {
                _Country_Name = value;
            }
        }
        [DataMember]
        public Guid? State_Id
        {
            get
            {
                return _State_Id;
            }

            set
            {
                _State_Id = value;
            }
        }
        [DataMember]
        public string State_Name
        {
            get
            {
                return _State_Name;
            }

            set
            {
                _State_Name = value;
            }
        }
        [DataMember]
        public int? PageNo
        {
            get
            {
                return _PageNo;
            }

            set
            {
                _PageNo = value;
            }
        }
        [DataMember]
        public int? PageSize
        {
            get
            {
                return _PageSize;
            }

            set
            {
                _PageSize = value;
            }
        }
        [DataMember]
        public string AlphaPageIndex
        {
            get
            {
                return _AlphaPageIndex;
            }

            set
            {
                _AlphaPageIndex = value;
            }
        }
        [DataMember]
        public string State_code
        {
            get
            {
                return _State_code;
            }

            set
            {
                _State_code = value;
            }
        }
    }

    [DataContract]
    public class State_AlphaPage
    {
        string _alpha;

        [DataMember]
        public string Alpha
        {
            get
            {
                return _alpha;
            }

            set
            {
                _alpha = value;
            }
        }
    }

    [DataContract]
    public class DC_State_Master_DDL
    {
        [DataMember]
        public string StateName { get; set; }
        [DataMember]
        public string StateCode { get; set; }
    }
}
