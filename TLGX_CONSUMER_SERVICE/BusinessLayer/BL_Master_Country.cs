using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataContracts;

namespace BusinessLayer
{
    public class BL_Master_Country : IDisposable
    {
        public void Dispose()
        { }

        public List<DataContracts.DC_Master_Country> GetCountryList()
        {
            using (DataLayer.DL_Master_Country obj = new DataLayer.DL_Master_Country())
            {
                return obj.GetCountryMaster();
            }
        }
    }
}