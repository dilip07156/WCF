using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BL_Master_Region : IDisposable
    {
        public void Dispose()
        { }

        public List<DataContracts.DC_Master_Region> GetRegionMaster()
        {
            using (DataLayer.DL_Master_Region obj = new DataLayer.DL_Master_Region())
            {
                return obj.GetRegionMaster();
            }
        }
    }
}
