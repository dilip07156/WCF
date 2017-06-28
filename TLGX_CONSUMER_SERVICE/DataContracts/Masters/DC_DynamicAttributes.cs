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
    public class DC_DynamicAttributes
    {
        System.Guid _DynamicAttribute_Id;
        string _ObjectType;
        string _AttributeClass;
        string _AttributeName;
        string _AttributeValue;
        Nullable<System.Guid> _Object_Id;
        Nullable<System.Guid> _ObjectSubElement_Id;
        bool _IsActive;
        string _Create_User;
        string _Edit_User;
        DateTime? _Create_Date;
        DateTime? _Edit_Date;

        [DataMember]
        public Guid DynamicAttribute_Id
        {
            get
            {
                return _DynamicAttribute_Id;
            }

            set
            {
                _DynamicAttribute_Id = value;
            }
        }

        [DataMember]
        public string ObjectType
        {
            get
            {
                return _ObjectType;
            }

            set
            {
                _ObjectType = value;
            }
        }

        [DataMember]
        public string AttributeClass
        {
            get
            {
                return _AttributeClass;
            }

            set
            {
                _AttributeClass = value;
            }
        }

        [DataMember]
        public string AttributeName
        {
            get
            {
                return _AttributeName;
            }

            set
            {
                _AttributeName = value;
            }
        }

        [DataMember]
        public string AttributeValue
        {
            get
            {
                return _AttributeValue;
            }

            set
            {
                _AttributeValue = value;
            }
        }

        [DataMember]
        public Guid? Object_Id
        {
            get
            {
                return _Object_Id;
            }

            set
            {
                _Object_Id = value;
            }
        }

        [DataMember]
        public Guid? ObjectSubElement_Id
        {
            get
            {
                return _ObjectSubElement_Id;
            }

            set
            {
                _ObjectSubElement_Id = value;
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