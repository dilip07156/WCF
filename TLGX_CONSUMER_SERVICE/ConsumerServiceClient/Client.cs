using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsumerServiceClient
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }

        private void countryMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetAllCountriesOrCities oFrm = new GetAllCountriesOrCities();
            oFrm.StartPosition = FormStartPosition.CenterScreen;
            oFrm.ShowDialog(this);
            oFrm.Dispose();
            oFrm = null;
        }

        private void cityMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetAllCitiesByCountry oFrm = new GetAllCitiesByCountry();
            oFrm.StartPosition = FormStartPosition.CenterScreen;
            oFrm.ShowDialog(this);
            oFrm.Dispose();
            oFrm = null;
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HotelSearch oFrm = new HotelSearch();
            oFrm.StartPosition = FormStartPosition.CenterScreen;
            oFrm.ShowDialog(this);
            oFrm.Dispose();
            oFrm = null;
        }

        private void geoLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GeoLocation oFrm = new GeoLocation();
            oFrm.StartPosition = FormStartPosition.CenterScreen;
            oFrm.ShowDialog(this);
            oFrm.Dispose();
            oFrm = null;
        }
    }
}
