using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DataContracts;
using DataContracts.Masters;

namespace BusinessLayer
{
    public class BL_Activity : IDisposable
    {
        public void Dispose()
        { }

        #region Activity Search
        public List<DC_ActivitySearch_RS> ActivitySearch(DC_Activity_Search_RQ RQ)
        {
            using (DataLayer.DL_Activity obj = new DataLayer.DL_Activity())
            {
                return obj.ActivitySearch(RQ);
            }
        }
        #endregion
    }
}
