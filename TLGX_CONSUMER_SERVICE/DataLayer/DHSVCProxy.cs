using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;

namespace DataLayer
{
    public enum ProxyFor
    {
        DataHandler,
        SqlToMongo
    }
    public static class DHSVCProxy
    {
        public static string DHSVCURL
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DHSVCURL"];
            }
        }

        public static string MONGOSVCURL
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["MONGOSVCURL"];
            }
        }

        public static bool GetData(ProxyFor For, string uri, Type ResponseType, out object ReturnValue)
        {
            try
            {
                string AbsPath = string.Empty;
                if (For == ProxyFor.DataHandler)
                {
                    AbsPath = DHSVCURL;
                }
                else if (For == ProxyFor.SqlToMongo)
                {
                    AbsPath = MONGOSVCURL;
                }

                HttpWebRequest request;
                request = (HttpWebRequest)WebRequest.Create(AbsPath + uri);

                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream stream = response.GetResponseStream();
                    DataContractJsonSerializer obj = new DataContractJsonSerializer(ResponseType);
                    ReturnValue = obj.ReadObject(stream);
                    obj = null;
                    stream = null;
                    response.Dispose();
                    request = null;
                    return true;
                }
                else
                {
                    response.Dispose();
                    request = null;
                    ReturnValue = null;
                    return false;
                }
            }
            catch (Exception e)
            {
                ReturnValue = null;
                return false;
            }

        }

        public static bool PostData(ProxyFor For, string URI, object Param, Type RequestType, Type ResponseType, out object ReturnValue)
        {
            try
            {
                string AbsPath = string.Empty;
                if (For == ProxyFor.DataHandler)
                {
                    AbsPath = DHSVCURL;
                }
                else if (For == ProxyFor.SqlToMongo)
                {
                    AbsPath = MONGOSVCURL;
                }

                HttpWebRequest request;
                request = (HttpWebRequest)WebRequest.Create(AbsPath + URI);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.KeepAlive = false;
                DataContractJsonSerializer serializerToUpload = new DataContractJsonSerializer(RequestType);

                using (var memoryStream = new MemoryStream())
                {
                    using (var reader = new StreamReader(memoryStream))
                    {
                        serializerToUpload.WriteObject(memoryStream, Param);
                        memoryStream.Position = 0;
                        string body = reader.ReadToEnd();

                        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                        {
                            streamWriter.Write(body);
                        }
                    }
                }

                var response = request.GetResponse();

                if (((System.Net.HttpWebResponse)response).StatusCode != HttpStatusCode.OK)
                {
                    ReturnValue = null;
                }
                else
                {
                    var stream = response.GetResponseStream();

                    var obj = new DataContractJsonSerializer(ResponseType);
                    ReturnValue = obj.ReadObject(stream);

                    obj = null;
                    stream = null;
                }

                serializerToUpload = null;

                response.Dispose();
                response = null;
                request = null;

                if (ReturnValue != null)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                ReturnValue = null;
                return false;
            }

        }
    }
}
