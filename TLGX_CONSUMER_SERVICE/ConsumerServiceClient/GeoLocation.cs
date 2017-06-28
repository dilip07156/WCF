using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsumerServiceClient
{
    public partial class GeoLocation : Form
    {
        public GeoLocation()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCityAreaOrDistrict.Clear();
            txtCityOrTownOrVillage.Clear();
            txtCountry.Clear();
            txtCountyOrState.Clear();
            txtPostalCode.Clear();
            txtStreet.Clear();
            lblFA.Text = "";
            lblLatitude.Text = "";
            lblLocationType.Text = "";
            lblLongitude.Text = "";
            lblPartialMatch.Text = "";
            lblStatus.Text = "";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ConsumerService.DC_Address_Physical address = new ConsumerService.DC_Address_Physical();
                address.Street = txtStreet.Text;
                address.CityAreaOrDistrict = txtCityAreaOrDistrict.Text;
                address.CityOrTownOrVillage = txtCityOrTownOrVillage.Text;
                address.CountyOrState = txtCountyOrState.Text;
                address.PostalCode = txtPostalCode.Text;
                address.Country = txtCountry.Text;

                //var request = (HttpWebRequest)WebRequest.Create("http://localhost:57643/Consumer.svc/GetGeoLocation/ByAddress");
                var request = (HttpWebRequest)WebRequest.Create("http://10.21.32.196:8080/Consumer.svc/GetGeoLocation/ByAddress");
                request.Method = "POST";
                request.ContentType = "application/json";
                request.KeepAlive = false;
                //request.Credentials = CredentialCache.DefaultCredentials;
                DataContractJsonSerializer serializerToUpload = new DataContractJsonSerializer(typeof(ConsumerService.DC_Address_Physical));

                using (var memoryStream = new MemoryStream())
                {
                    using (var reader = new StreamReader(memoryStream))
                    {
                        serializerToUpload.WriteObject(memoryStream, address);
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
                    MessageBox.Show(((System.Net.HttpWebResponse)response).StatusDescription);
                }
                else
                {
                    var stream = response.GetResponseStream();

                    var obj = new DataContractJsonSerializer(typeof(ConsumerService.DC_GeoLocation));
                    var resultSearch = obj.ReadObject(stream) as ConsumerService.DC_GeoLocation;

                    if (resultSearch.results != null)
                    {
                        if (resultSearch.status != "OK")
                        {
                            lblFA.Text = "";
                            lblLatitude.Text = "";
                            lblLocationType.Text = "";
                            lblLongitude.Text = "";
                            lblPartialMatch.Text = "";
                        }
                        else
                        {
                            if (resultSearch.results.Length > 0)
                            {
                                lblFA.Text = resultSearch.results[0].formatted_address;
                                lblLatitude.Text = resultSearch.results[0].geometry.location.lat.ToString();
                                lblLocationType.Text = resultSearch.results[0].geometry.location_type;
                                lblLongitude.Text = resultSearch.results[0].geometry.location.lng.ToString();
                                lblPartialMatch.Text = resultSearch.results[0].partial_match.ToString();
                            }

                        }

                        lblStatus.Text = resultSearch.status;
                    }

                    obj = null;
                    stream = null;
                }

                serializerToUpload = null;

                response.Dispose();
                response = null;
                request = null;
                address = null;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }
    }
}
