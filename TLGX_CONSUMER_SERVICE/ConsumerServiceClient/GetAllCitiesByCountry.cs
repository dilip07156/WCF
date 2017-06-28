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
    public partial class GetAllCitiesByCountry : Form
    {
        public GetAllCitiesByCountry()
        {
            InitializeComponent();
        }

        private void btnGetCities_Click(object sender, EventArgs e)
        {
            System.Net.WebClient proxy = new System.Net.WebClient();
            byte[] data = proxy.DownloadData("http://localhost:57643/Consumer.svc/GetCitiesByCountry/" + cmbCountries.SelectedValue.ToString());
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            cmbCountries.SelectedIndex = 0;
            dgv.DataSource = null;
        }

        private void GetAllCitiesByCountry_Load(object sender, EventArgs e)
        {
            System.Net.WebClient proxy = new System.Net.WebClient();
            byte[] data = proxy.DownloadData("http://10.21.32.196:8080/Consumer.svc/GetAllCountries");
            Stream stream = new MemoryStream(data);
            DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(List<ConsumerService.DC_Master_Country>));
            var result = obj.ReadObject(stream) as List<ConsumerService.DC_Master_Country>;

            cmbCountries.DataSource = (from r in result select new { r.Country_Id, r.Country_Name }).ToList();
            cmbCountries.ValueMember = "Country_Id";
            cmbCountries.DisplayMember = "Country_Name";
            cmbCountries.Refresh();

            result = null;
            obj = null;
            stream = null;
            data = null;
            proxy = null;
        }

    }


   // public static class ConvertToDatatable
    //{
    //    public static DataTable ToDataTable<T>(this List<T> iList)
    //    {
    //        DataTable dataTable = new DataTable();
    //        PropertyDescriptorCollection propertyDescriptorCollection =
    //            TypeDescriptor.GetProperties(typeof(T));
    //        for (int i = 0; i < propertyDescriptorCollection.Count; i++)
    //        {
    //            PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
    //            Type type = propertyDescriptor.PropertyType;

    //            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
    //                type = Nullable.GetUnderlyingType(type);


    //            dataTable.Columns.Add(propertyDescriptor.Name, type);
    //        }
    //        object[] values = new object[propertyDescriptorCollection.Count];
    //        foreach (T iListItem in iList)
    //        {
    //            for (int i = 0; i < values.Length; i++)
    //            {
    //                values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
    //            }
    //            dataTable.Rows.Add(values);
    //        }
    //        return dataTable;
    //    }
    //}
    
}
