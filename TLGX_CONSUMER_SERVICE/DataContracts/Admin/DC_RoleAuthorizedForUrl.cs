using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Admin
{
    [DataContract]
    public class DC_RoleAuthorizedForUrl
    {
        public string user;
        public string url;

        [DataMember]
        public string User
        {
            get
            {
                return user;
            }

            set
            {
                user = value;
            }
        }
        [DataMember]
        public string Url
        {
            get
            {
                return url;
            }

            set
            {
                url = value;
            }
        }
    }
}
