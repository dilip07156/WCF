namespace ConsumerServiceClient
{
    partial class Client
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mastersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.countryMasterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cityMasterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accomodationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.geoLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mastersToolStripMenuItem,
            this.accomodationToolStripMenuItem,
            this.geoLocationToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(703, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mastersToolStripMenuItem
            // 
            this.mastersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.countryMasterToolStripMenuItem,
            this.cityMasterToolStripMenuItem});
            this.mastersToolStripMenuItem.Name = "mastersToolStripMenuItem";
            this.mastersToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.mastersToolStripMenuItem.Text = "Masters";
            // 
            // countryMasterToolStripMenuItem
            // 
            this.countryMasterToolStripMenuItem.Name = "countryMasterToolStripMenuItem";
            this.countryMasterToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.countryMasterToolStripMenuItem.Text = "CountryMaster";
            this.countryMasterToolStripMenuItem.Click += new System.EventHandler(this.countryMasterToolStripMenuItem_Click);
            // 
            // cityMasterToolStripMenuItem
            // 
            this.cityMasterToolStripMenuItem.Name = "cityMasterToolStripMenuItem";
            this.cityMasterToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.cityMasterToolStripMenuItem.Text = "CityMaster";
            this.cityMasterToolStripMenuItem.Click += new System.EventHandler(this.cityMasterToolStripMenuItem_Click);
            // 
            // accomodationToolStripMenuItem
            // 
            this.accomodationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchToolStripMenuItem});
            this.accomodationToolStripMenuItem.Name = "accomodationToolStripMenuItem";
            this.accomodationToolStripMenuItem.Size = new System.Drawing.Size(86, 20);
            this.accomodationToolStripMenuItem.Text = "Accomodation";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.searchToolStripMenuItem.Text = "Search";
            this.searchToolStripMenuItem.Click += new System.EventHandler(this.searchToolStripMenuItem_Click);
            // 
            // geoLocationToolStripMenuItem
            // 
            this.geoLocationToolStripMenuItem.Name = "geoLocationToolStripMenuItem";
            this.geoLocationToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.geoLocationToolStripMenuItem.Text = "Geo Location";
            this.geoLocationToolStripMenuItem.Click += new System.EventHandler(this.geoLocationToolStripMenuItem_Click);
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 424);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Client";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Client - Consumer Service";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mastersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem countryMasterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cityMasterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accomodationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem geoLocationToolStripMenuItem;
    }
}

