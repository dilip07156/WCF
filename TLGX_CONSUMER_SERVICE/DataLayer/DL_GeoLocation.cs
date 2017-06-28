using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.ServiceModel.Web;
using System.ServiceModel;

namespace DataLayer
{
    public class DL_GeoLocation : IDisposable
    {
        public void Dispose()
        { }

        public DataContracts.DC_GeoLocation GetGeoLocationByAddress(DataContracts.DC_Address.DC_Address_Physical PA)
        {
            try
            {
                string Address = String.Empty;

                if (!string.IsNullOrEmpty(PA.Street))
                {
                    Address = Address + PA.Street.Trim().TrimEnd(',').Replace(" ", "+") + ",";
                }

                if (!string.IsNullOrEmpty(PA.CityAreaOrDistrict))
                {
                    Address = Address + PA.CityAreaOrDistrict.Trim().TrimEnd(',').Replace(" ", "+") + ",";
                }

                if (!string.IsNullOrEmpty(PA.CityOrTownOrVillage))
                {
                    Address = Address + PA.CityOrTownOrVillage.Trim().TrimEnd(',').Replace(" ", "+") + ",";
                }

                if (!string.IsNullOrEmpty(PA.CountyOrState))
                {
                    Address = Address + PA.CountyOrState.Trim().TrimEnd(',').Replace(" ", "+") + ",";
                }

                if (!string.IsNullOrEmpty(PA.PostalCode))
                {
                    Address = Address + PA.PostalCode.Trim().TrimEnd(',').Replace(" ", "+") + ",";
                }

                if (!string.IsNullOrEmpty(PA.Country))
                {
                    Address = Address + PA.Country.Trim().TrimEnd(',').Replace(" ", "+") + ",";
                }

                Address = Address.TrimEnd(',').Trim();

                DataContracts.DC_GeoLocation mapdata = null;

                if (!string.IsNullOrEmpty(Address))
                {
                    var request = (HttpWebRequest)WebRequest.Create("https://maps.googleapis.com/maps/api/geocode/json?address=" + Address + "&key=" + System.Configuration.ConfigurationManager.AppSettings["GoogleKey"].ToString());

                    var proxyAddress = System.Configuration.ConfigurationManager.AppSettings["ProxyUri"];
                    if (System.Configuration.ConfigurationManager.AppSettings["ProxyUri"] != null)
                    {
                        WebProxy myProxy = new WebProxy();
                        Uri newUri = new Uri(proxyAddress);
                        // Associate the newUri object to 'myProxy' object so that new myProxy settings can be set.
                        myProxy.Address = newUri;
                        // Create a NetworkCredential object and associate it with the 
                        // Proxy property of request object.
                        //myProxy.Credentials = new NetworkCredential(username, password);
                        request.Proxy = myProxy;
                    }

                    request.KeepAlive = false;
                    //request.Credentials = CredentialCache.DefaultCredentials;
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    if (response.StatusCode == HttpStatusCode.OK) //response.StatusDescription
                    {
                        Stream dataStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(dataStream);
                        string responseFromServer = reader.ReadToEnd();
                        reader.Close();

                        mapdata = JsonConvert.DeserializeObject<DataContracts.DC_GeoLocation>(responseFromServer);

                        if (mapdata != null)
                        {
                            using (ConsumerEntities context = new ConsumerEntities())
                            {
                                GoogleGeoCode GC = new GoogleGeoCode();
                                GC.GoogleGeoCode_Id = Guid.NewGuid();
                                GC.Product_Id = PA.Product_Id;
                                GC.JobType = "ProductGeoLookup_ByAddress";
                                GC.Input = request.Address.AbsoluteUri;
                                GC.OutPut = responseFromServer;

                                context.GoogleGeoCodes.Add(GC);
                                context.SaveChanges();
                            }
                        }

                    }

                    //DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(DataContracts.DC_GeoLocation));
                    //var result = obj.ReadObject(dataStream) as DataContracts.DC_GeoLocation;

                    response.Close();

                }

                return mapdata;

            }
            catch (WebException ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = ex.Message, ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DataContracts.DC_GeoLocation GetGeoLocationByLatLng(DataContracts.DC_Address.DC_Address_GeoCode AG)
        {
            try
            {
                string LatLng = String.Empty;

                LatLng = AG.Latitude.ToString() + "," + AG.Longitude.ToString();

                DataContracts.DC_GeoLocation mapdata = null;

                var request = (HttpWebRequest)WebRequest.Create("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + LatLng + "&key=" + System.Configuration.ConfigurationManager.AppSettings["GoogleKey"].ToString());

                var proxyAddress = System.Configuration.ConfigurationManager.AppSettings["ProxyUri"];
                if (System.Configuration.ConfigurationManager.AppSettings["ProxyUri"] != null)
                {
                    WebProxy myProxy = new WebProxy();
                    Uri newUri = new Uri(proxyAddress);
                    // Associate the newUri object to 'myProxy' object so that new myProxy settings can be set.
                    myProxy.Address = newUri;
                    // Create a NetworkCredential object and associate it with the 
                    // Proxy property of request object.
                    //myProxy.Credentials = new NetworkCredential(username, password);
                    request.Proxy = myProxy;
                }


                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                if (response.StatusCode == HttpStatusCode.OK) //response.StatusDescription
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    reader.Close();

                    mapdata = JsonConvert.DeserializeObject<DataContracts.DC_GeoLocation>(responseFromServer);

                    if (mapdata != null)
                    {
                        using (ConsumerEntities context = new ConsumerEntities())
                        {
                            GoogleGeoCode GC = new GoogleGeoCode();
                            GC.GoogleGeoCode_Id = Guid.NewGuid();
                            GC.Product_Id = AG.Product_Id;
                            GC.JobType = "ProductGeoLookup_ByLatLng";
                            GC.Input = request.Address.AbsoluteUri;
                            GC.OutPut = responseFromServer;

                            context.GoogleGeoCodes.Add(GC);
                            context.SaveChanges();
                        }
                    }

                }

                //DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(DataContracts.DC_GeoLocation));
                //var result = obj.ReadObject(dataStream) as DataContracts.DC_GeoLocation;

                response.Close();

                return mapdata;

            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching address", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.DC_Accommodation_NearbyPlaces> GetNearByPlacesByLatLng(DataContracts.DC_Address.DC_Address_GeoCodeForNearBy AG) 
        {
            try
            {
                List<DataContracts.DC_Accommodation_NearbyPlaces> _lst = new List<DataContracts.DC_Accommodation_NearbyPlaces>();
                string LatLng = String.Empty;

                LatLng = AG.Latitude.ToString() + "," + AG.Longitude.ToString();

                DataContracts.DC_GeoLocation mapdata = null;

                var request = (HttpWebRequest)WebRequest.Create("https://maps.googleapis.com/maps/api/place/radarsearch/json?location=" + LatLng + "&radius=" + AG.radius + "&type=" + AG.PlaceType + "&key=" +System.Configuration.ConfigurationManager.AppSettings["GoogleKey"].ToString());

                var proxyAddress = System.Configuration.ConfigurationManager.AppSettings["ProxyUri"];
                if (System.Configuration.ConfigurationManager.AppSettings["ProxyUri"] != null)
                {
                    WebProxy myProxy = new WebProxy();
                    Uri newUri = new Uri(proxyAddress);
                    // Associate the newUri object to 'myProxy' object so that new myProxy settings can be set.
                    myProxy.Address = newUri;
                    // Create a NetworkCredential object and associate it with the 
                    // Proxy property of request object.
                    //myProxy.Credentials = new NetworkCredential(username, password);
                    request.Proxy = myProxy;
                }


                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                if (response.StatusCode == HttpStatusCode.OK) //response.StatusDescription
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    reader.Close();

                    mapdata = JsonConvert.DeserializeObject<DataContracts.DC_GeoLocation>(responseFromServer);

                    if (mapdata != null)
                    {
                        //using (ConsumerEntities context = new ConsumerEntities())
                        //{
                        //    GoogleGeoCode GC = new GoogleGeoCode();
                        //    GC.GoogleGeoCode_Id = Guid.NewGuid();
                        //    GC.JobType = "ProductGeoLookup_ByLatLng";
                        //    GC.Input = request.Address.AbsoluteUri;
                        //    GC.OutPut = responseFromServer;

                        //    context.GoogleGeoCodes.Add(GC);
                        //    context.SaveChanges();
                        //}
                    }

                }

                //DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(DataContracts.DC_GeoLocation));
                //var result = obj.ReadObject(dataStream) as DataContracts.DC_GeoLocation;

                response.Close();

                return _lst;

            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching address", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
    }
}
