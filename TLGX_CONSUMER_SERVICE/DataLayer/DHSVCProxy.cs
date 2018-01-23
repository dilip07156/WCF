using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Net.Http;
using System.Xml.Serialization;
using System.Xml;

namespace DataLayer
{
    public enum ProxyFor
    {
        DataHandler,
        SqlToMongo,
        Pentaho
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

        public static string PENTAHOSVCURL
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["PENTAHOSVCURL"];
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
                else if (For == ProxyFor.Pentaho)
                {
                    AbsPath = PENTAHOSVCURL;
                }

                HttpWebRequest request;
                request = (HttpWebRequest)WebRequest.Create(AbsPath + uri);

                if (For == ProxyFor.Pentaho)
                {
                    //string credidentials =  "cluster:cluster";
                    //var authorization = Convert.ToBase64String(Encoding.Default.GetBytes(credidentials));
                    //request.Headers["Authorization"] = "Basic " + authorization;
                    request.Credentials = new NetworkCredential("cluster", "cluster");
                }

                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream stream = response.GetResponseStream();

                    if(For == ProxyFor.Pentaho)
                    {
                        if (ResponseType != typeof(void))
                        {
                            var objXMLReader = new XmlTextReader(stream);

                            XmlDocument xmldoc = new XmlDocument();
                            xmldoc.Load(objXMLReader);
                            string res = xmldoc.InnerXml;
                            objXMLReader.Close();

                            var reader = new StringReader(res);
                            XmlSerializer serializer = new XmlSerializer(ResponseType);
                            ReturnValue = serializer.Deserialize(reader);
                        }
                        else
                        {
                            ReturnValue = null;
                        }
                    }
                    else
                    {
                        DataContractJsonSerializer obj = new DataContractJsonSerializer(ResponseType);
                        ReturnValue = obj.ReadObject(stream);
                        obj = null;
                    }

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

    public class DHSVCProxyAsync
    {
        public void PostAsync(ProxyFor For, string URI, object Param, Type RequestType)
        {
            string AbsPath = string.Empty;
            if (For == ProxyFor.DataHandler)
            {
                AbsPath = DHSVCProxy.DHSVCURL;
            }
            else if (For == ProxyFor.SqlToMongo)
            {
                AbsPath = DHSVCProxy.MONGOSVCURL;
            }

            string requestUri = AbsPath + URI;

            DataContractJsonSerializer serializerToUpload = new DataContractJsonSerializer(RequestType);
            string body = string.Empty;
            using (var memoryStream = new MemoryStream())
            {
                using (var reader = new StreamReader(memoryStream))
                {
                    serializerToUpload.WriteObject(memoryStream, Param);
                    memoryStream.Position = 0;
                    body = reader.ReadToEnd();
                }
            }
            serializerToUpload = null;

            HttpClient hc = new HttpClient();
            StringContent json = new StringContent(body, Encoding.UTF8, "application/json");
            hc.PostAsync(requestUri, json);
        }

        public void GetAsync(ProxyFor For, string URI)
        {
            string AbsPath = string.Empty;
            if (For == ProxyFor.DataHandler)
            {
                AbsPath = DHSVCProxy.DHSVCURL;
            }
            else if (For == ProxyFor.SqlToMongo)
            {
                AbsPath = DHSVCProxy.MONGOSVCURL;
            }

            string requestUri = AbsPath + URI;

            HttpClient hc = new HttpClient();
            hc.GetAsync(requestUri);
            hc.Dispose();
        }
    }
}
