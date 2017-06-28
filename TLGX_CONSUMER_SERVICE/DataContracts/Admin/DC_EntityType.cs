using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System;

namespace DataContracts.Admin
{
    [DataContract]
    public class DC_EntityType
    {
        int _enityTypeID;
        string _entityTypeName;
        [DataMember]
        public int EnityTypeID
        {
            get
            {
                return _enityTypeID;
            }

            set
            {
                _enityTypeID = value;
            }
        }

        [DataMember]
        public string EntityTypeName
        {
            get
            {
                return _entityTypeName;
            }

            set
            {
                _entityTypeName = value;
            }
        }
    }
}
