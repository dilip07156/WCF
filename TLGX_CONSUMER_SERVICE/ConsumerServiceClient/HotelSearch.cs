using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Net;

namespace ConsumerServiceClient
{
    public partial class HotelSearch : Form
    {
        public HotelSearch()
        {
            InitializeComponent();
        }

        private void HotelSearch_Load(object sender, EventArgs e)
        {
            GetAllCountries();
        }

        private void GetAllCountries()
        {
            //System.Net.WebClient proxy = new System.Net.WebClient();
            //byte[] data = proxy.DownloadData("http://10.21.32.196:8080/Consumer.svc/GetAllCountries");
            //Stream stream = new MemoryStream(data);
            //DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(List<ConsumerService.DC_Master_Country>));
            //var result = obj.ReadObject(stream) as List<ConsumerService.DC_Master_Country>;

            //cmbCountries.DataSource = (from r in result select new { r.Country_Id, r.Country_Name }).ToList();
            //cmbCountries.ValueMember = "Country_Id";
            //cmbCountries.DisplayMember = "Country_Name";
            //cmbCountries.Refresh();

            //result = null;
            //obj = null;
            //stream = null;
            //data = null;
            //proxy = null;

            var request = (HttpWebRequest)WebRequest.Create("http://10.21.32.196:8080/Consumer.svc/GetAllCountries");
            request.KeepAlive = false;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream stream = response.GetResponseStream();
            DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(List<ConsumerService.DC_Master_Country>));
            var result = obj.ReadObject(stream) as List<ConsumerService.DC_Master_Country>;

            cmbCountries.DataSource = (from r in result select new { r.Country_Id, r.Country_Name }).ToList();
            cmbCountries.ValueMember = "Country_Id";
            cmbCountries.DisplayMember = "Country_Name";
            cmbCountries.Refresh();

            result = null;
            obj = null;
            stream = null;
            response.Dispose();
            request = null;

            //StreamReader reader = new StreamReader(stream);
            //string result = reader.ReadToEnd();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ConsumerService.DC_Accomodation_Search_RQ RQ = new ConsumerService.DC_Accomodation_Search_RQ();
            RQ.ProductCategory = txtProductCategory.Text;
            RQ.ProductCategorySubType = txtProductCategorySubType.Text;
            RQ.Status = cmbStatus.Text == "Active" ? true : false;

            if (txtHotelName.Text.Length != 0)
                RQ.HotelName = txtHotelName.Text;


            if (txtCompanyHotelId.Text.Length != 0)
            {
                int CompanyHotelId = 0;
                int.TryParse(txtCompanyHotelId.Text, out CompanyHotelId);

                if (CompanyHotelId != 0)
                    RQ.CompanyHotelId = Convert.ToInt32(txtCompanyHotelId.Text);
            }


            RQ.Country = cmbCountries.Text;
            RQ.City = cmbCity.Text;

            RQ.PageNo = 0;
            RQ.PageSize = 100;

            //System.Net.WebClient proxy = new System.Net.WebClient();

            //proxy.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";

            //MemoryStream ms = new MemoryStream();

            //DataContractJsonSerializer serializerToUplaod = new DataContractJsonSerializer(typeof(ConsumerService.DC_Accomodation_Search_RQ));
            //serializerToUplaod.WriteObject(ms, RQ);
            //byte[] data = proxy.UploadData("http://localhost:57643/Consumer.svc/Accomodation/Search", "POST", ms.ToArray());
            //var stream = new MemoryStream(data);
            //var obj = new DataContractJsonSerializer(typeof(List<ConsumerService.DC_Accomodation_Search_RS>));
            //var resultSearch = obj.ReadObject(stream) as List<ConsumerService.DC_Accomodation_Search_RS>;
            //dgv.DataSource = resultSearch;

            var request = (HttpWebRequest)WebRequest.Create("http://10.21.32.196:8080/Consumer.svc/Accomodation/Search");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Token", "sdfs");
            request.KeepAlive = false;
            DataContractJsonSerializer serializerToUpload = new DataContractJsonSerializer(typeof(ConsumerService.DC_Accomodation_Search_RQ));
            
            using (var memoryStream = new MemoryStream())
            {
                using (var reader = new StreamReader(memoryStream))
                {
                    serializerToUpload.WriteObject(memoryStream, RQ);
                    memoryStream.Position = 0;
                    string body = reader.ReadToEnd();

                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(body);
                    }
                }
            }

            var response = request.GetResponse();

            if(((System.Net.HttpWebResponse)response).StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show(((System.Net.HttpWebResponse)response).StatusDescription);
                dgv.DataSource = null;
            }
            else
            {
                var stream = response.GetResponseStream();

                var obj = new DataContractJsonSerializer(typeof(List<ConsumerService.DC_Accomodation_Search_RS>));
                var resultSearch = obj.ReadObject(stream) as List<ConsumerService.DC_Accomodation_Search_RS>;
                dgv.DataSource = resultSearch;

                obj = null;
                stream = null;
            }
            
            serializerToUpload = null;
            
            response.Dispose();
            response = null;
            request = null;
            RQ = null;

        }

        private void cmbCountries_SelectedIndexChanged(object sender, EventArgs e)
        {
            Guid Country_Id;
            Guid.TryParse(cmbCountries.SelectedValue.ToString(), out Country_Id);

            if (Country_Id != Guid.Empty)
            {
                System.Net.WebClient proxy = new System.Net.WebClient();
                byte[] data = proxy.DownloadData("http://10.21.32.196:8080/Consumer.svc/GetCitiesByCountry/" + cmbCountries.SelectedValue.ToString());
                Stream stream = new MemoryStream(data);
                DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(List<ConsumerService.DC_Master_City>));
                var result = obj.ReadObject(stream) as List<ConsumerService.DC_Master_City>;

                cmbCity.DataSource = (from r in result select new { r.City_Id, r.City_Name }).ToList();
                cmbCity.ValueMember = "City_Id";
                cmbCity.DisplayMember = "City_Name";
                cmbCity.Refresh();

                result = null;
                obj = null;
                stream = null;
                data = null;
                proxy = null;
            }
        }
    }

}
