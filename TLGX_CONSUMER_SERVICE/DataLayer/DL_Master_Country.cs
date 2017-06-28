using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataContracts;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace DataLayer
{
    public class DL_Master_Country : IDisposable
    {
        public void Dispose()
        { }

        public List<DataContracts.DC_Master_Country> GetCountryMaster()
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var country = from c in context.m_CountryMaster
                                  orderby c.Name
                                  where c.Status == "ACTIVE"
                                  select new DataContracts.DC_Master_Country { Country_Id = c.Country_Id, Country_Name = c.Name, Country_Code = c.Code };
                    return country.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching country master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
    }
}
