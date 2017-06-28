using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DataContracts.Admin
{
    [DataContract]
    public class DC_Roles
    {
        string _RoleID;
        string _RoleName;
        //string _EntityTypeID;
        //string _EntityType;
        Guid _ApplicationID;
        int _totalRecords;

        [DataMember]
        public string RoleID
        {
            get
            {
                return _RoleID;
            }

            set
            {
                _RoleID = value;
            }
        }

        [DataMember]
        public string RoleName
        {
            get
            {
                return _RoleName;
            }

            set
            {
                _RoleName = value;
            }
        }

      //  [DataMember]
        //public string EntityTypeID
        //{
        //    get
        //    {
        //        return _EntityTypeID;
        //    }

        //    set
        //    {
        //        _EntityTypeID = value;
        //    }
        //}
        //[DataMember]
        //public string EntityType
        //{
        //    get
        //    {
        //        return _EntityType;
        //    }

        //    set
        //    {
        //        _EntityType = value;
        //    }
        //}
        [DataMember]
        public int TotalRecords
        {
            get
            {
                return _totalRecords;
            }

            set
            {
                _totalRecords = value;
            }
        }
        [DataMember]
        public Guid ApplicationID
        {
            get
            {
                return _ApplicationID;
            }

            set
            {
                _ApplicationID = value;
            }
        }
    }
}
