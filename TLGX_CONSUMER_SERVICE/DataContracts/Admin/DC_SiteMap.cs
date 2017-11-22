using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DataContracts.Admin
{
    [DataContract]
    public class DC_SiteMap
    {
        Guid _SiteMap_ID;
        int _ID;
        string _Title;
        string _Description;
        string _Url;
        string _Roles;
        Nullable<int> _ParentID;
        string _ParentTitle;
        Nullable<System.DateTime> _Create_Date;
        string _Create_User;
        Nullable<System.DateTime> _Edit_Date;
        string _Edit_User;
        bool _IsActive;
        bool _IsSiteMapNode;
        Guid _applicationID;

        [DataMember]
        public int ID
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
        public string Title
        {
            get
            {
                return _Title;
            }

            set
            {
                _Title = value;
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
            }
        }

        [DataMember]
        public string Url
        {
            get
            {
                return _Url;
            }

            set
            {
                _Url = value;
            }
        }

        [DataMember]
        public string Roles
        {
            get
            {
                return _Roles;
            }

            set
            {
                _Roles = value;
            }
        }

        [DataMember]
        public int? ParentID
        {
            get
            {
                return _ParentID;
            }

            set
            {
                _ParentID = value;
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
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }

            set
            {
                _IsActive = value;
            }
        }

        [DataMember]
        public bool IsSiteMapNode
        {
            get
            {
                return _IsSiteMapNode;
            }

            set
            {
                _IsSiteMapNode = value;
            }
        }
        
        [DataMember]
        public string ParentTitle
        {
            get
            {
                return _ParentTitle;
            }

            set
            {
                _ParentTitle = value;
            }
        }
        [DataMember]
        public Guid ApplicationID
        {
            get
            {
                return _applicationID;
            }

            set
            {
                _applicationID = value;
            }
        }
        [DataMember]
        public Guid SiteMap_ID
        {
            get
            {
                return _SiteMap_ID;
            }

            set
            {
                _SiteMap_ID = value;
            }
        }
    }
}
