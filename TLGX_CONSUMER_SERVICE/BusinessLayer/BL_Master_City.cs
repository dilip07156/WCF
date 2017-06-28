using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BL_Master_City : IDisposable
    {
        public void Dispose()
        { }

        public List<DataContracts.DC_Master_City> GetCityList()
        {
            using (DataLayer.DL_Master_City obj = new DataLayer.DL_Master_City())
            {
                return obj.GetCityMaster();
            }
        }

        public List<DataContracts.DC_Master_City> GetCityList(Guid Country_Id)
        {
            using (DataLayer.DL_Master_City obj = new DataLayer.DL_Master_City())
            {
                return obj.GetCityMaster(Country_Id);
            }
        }
    }
}
