using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DL_Master_Region : IDisposable
    {
        public void Dispose()
        { }

        public List<DataContracts.DC_Master_Region> GetRegionMaster()
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var regions = (from c in context.m_CountryMaster
                                   orderby c.RegionName
                                   where c.Status == "ACTIVE" && c.RegionCode != null
                                   select new DataContracts.DC_Master_Region { RegionName = c.RegionName, RegionCode = c.RegionCode }).Distinct();

                    return regions.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
