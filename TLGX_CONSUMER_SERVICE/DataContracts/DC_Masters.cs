using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
namespace DataContracts
{
    [DataContract]
    public class DC_Masters
    {

    }

    [DataContract]
    public class DC_Teams
    {
        Guid _Team_ID;
        string _Team_Name;
        string _Status;
        string _Create_User;
        Nullable<System.DateTime> _Create_Date;
        string _Edit_User;
        Nullable<System.DateTime> _Edit_Date;

        [DataMember]
        public Guid Team_ID
        {
            get
            {
                return _Team_ID;
            }

            set
            {
                _Team_ID = value;
            }
        }

        [DataMember]
        public string Team_Name
        {
            get
            {
                return _Team_Name;
            }

            set
            {
                _Team_Name = value;
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


    }

}