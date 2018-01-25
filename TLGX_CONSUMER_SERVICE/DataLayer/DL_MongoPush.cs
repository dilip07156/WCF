using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DL_MongoPush
    {
        public void SyncActivityFlavour(Guid Activity_Flavour_id)
        {
            if (Activity_Flavour_id != Guid.Empty)
            {
                DHSVCProxyAsync DHP = new DHSVCProxyAsync();
                string strURI = string.Format(System.Configuration.ConfigurationManager.AppSettings["Sync_Activity_Flavour"], Convert.ToString(Activity_Flavour_id));
                DHP.GetAsync(ProxyFor.SqlToMongo, strURI);
            }

        }
    }
}
