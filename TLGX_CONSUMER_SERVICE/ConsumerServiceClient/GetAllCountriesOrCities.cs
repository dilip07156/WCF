using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConsumerServiceClient.ConsumerService;
using System.Runtime.Serialization.Json;
using System.IO;

namespace ConsumerServiceClient
{
    public partial class GetAllCountriesOrCities : Form
    {
        public GetAllCountriesOrCities()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            dgv.DataSource = null;
        }

        private void btnGetAllCountries_Click(object sender, EventArgs e)
        {
            System.Net.WebClient proxy = new System.Net.WebClient();
            byte[] data = proxy.DownloadData("http://localhost:57643/Consumer.svc/GetAllCountries");
            Stream stream = new MemoryStream(data);
            DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(List<ConsumerService.DC_Master_Country>));
            var result = obj.ReadObject(stream) as List<ConsumerService.DC_Master_Country>;
            dgv.DataSource = result;
            result = null;
            obj = null;
            stream = null;
            data = null;
            proxy = null;
        }

        private void btnGetAllCities_Click(object sender, EventArgs e)
        {
            System.Net.WebClient proxy = new System.Net.WebClient();
            byte[] data = proxy.DownloadData("http://localhost:57643/Consumer.svc/GetAllCities");
            Stream stream = new MemoryStream(data);
            DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(List<ConsumerService.DC_Master_City>));
            var result = obj.ReadObject(stream) as List<ConsumerService.DC_Master_City>;
            dgv.DataSource = result;
            result = null;
            obj = null;
            stream = null;
            data = null;
            proxy = null;
        }
    }
}
