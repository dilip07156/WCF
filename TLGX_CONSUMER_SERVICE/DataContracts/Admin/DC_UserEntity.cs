using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System;

namespace DataContracts.Admin
{
    [DataContract]
    public class DC_UserEntity
    {
        string _ID;
        Guid _UserID;
        Guid? _EntityID;
        int? _EntityTypeID;
        string _managerID;
        Guid _applicationId;

        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string Edit_User { get; set; }

        [DataMember]
        public string ID
        {
            get
            {
                return _ID;
            }

            set
            {
                _ID = value;
            }
        }
        [DataMember]
        public Guid UserID
        {
            get
            {
                return _UserID;
            }

            set
            {
                _UserID = value;
            }
        }

        [DataMember]
        public Guid? EntityID
        {
            get
            {
                return _EntityID;
            }

            set
            {
                _EntityID = value;
            }
        }

        [DataMember]
        public int? EntityTypeID
        {
            get
            {
                return _EntityTypeID;
            }

            set
            {
                _EntityTypeID = value;
            }
        }
        [DataMember]
        public string ManagerID
        {
            get
            {
                return _managerID;
            }

            set
            {
                _managerID = value;
            }
        }
        [DataMember]
        public Guid ApplicationId
        {
            get
            {
                return _applicationId;
            }

            set
            {
                _applicationId = value;
            }
        }
    }
}
