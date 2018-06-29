using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace DataLayer
{
    public class DL_Master_City : IDisposable
    {
        public void Dispose()
        {

        }

        public List<DataContracts.DC_Master_City> GetCityMaster()
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var city = from c in context.m_CityMaster
                                  orderby c.Name
                                  select new DataContracts.DC_Master_City { City_Id = c.City_Id, City_Name = c.Name, City_Code = c.Code, Country_Name = c.CountryName, Country_Id = c.Country_Id, State_Code = c.StateCode, State_Name = c.StateName };
                    return city.ToList();
                }
            }
            catch(Exception ex)
            {
                //throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching city master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                throw ex;
            }
        }

        public List<DataContracts.DC_Master_City> GetCityMaster(Guid Country_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var city = from c in context.m_CityMaster
                               orderby c.Name
                               where c.Country_Id == Country_Id && c.Status == "ACTIVE"
                               select new DataContracts.DC_Master_City { City_Id = c.City_Id, City_Name = c.Name, City_Code = c.Code, Country_Name = c.CountryName, Country_Id = c.Country_Id, State_Code = c.StateCode, State_Name = c.StateName };
                    return city.ToList();
                }
            }
            catch(Exception ex)
            {
                //throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching city master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                throw ex;
            }
        }
    }
}
