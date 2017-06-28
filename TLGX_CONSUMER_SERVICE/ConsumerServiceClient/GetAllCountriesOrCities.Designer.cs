namespace ConsumerServiceClient
{
    partial class GetAllCountriesOrCities
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
            this.dgv = new System.Windows.Forms.DataGridView();
            this.btnGetAllCities = new System.Windows.Forms.Button();
            this.btnGetAllCountries = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(0, 49);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.Size = new System.Drawing.Size(489, 321);
            this.dgv.TabIndex = 0;
            // 
            // btnGetAllCities
            // 
            this.btnGetAllCities.Location = new System.Drawing.Point(137, 13);
            this.btnGetAllCities.Name = "btnGetAllCities";
            this.btnGetAllCities.Size = new System.Drawing.Size(75, 23);
            this.btnGetAllCities.TabIndex = 1;
            this.btnGetAllCities.Text = "Get All Cities";
            this.btnGetAllCities.UseVisualStyleBackColor = true;
            this.btnGetAllCities.Click += new System.EventHandler(this.btnGetAllCities_Click);
            // 
            // btnGetAllCountries
            // 
            this.btnGetAllCountries.Location = new System.Drawing.Point(12, 13);
            this.btnGetAllCountries.Name = "btnGetAllCountries";
            this.btnGetAllCountries.Size = new System.Drawing.Size(93, 23);
            this.btnGetAllCountries.TabIndex = 2;
            this.btnGetAllCountries.Text = "Get All Countries";
            this.btnGetAllCountries.UseVisualStyleBackColor = true;
            this.btnGetAllCountries.Click += new System.EventHandler(this.btnGetAllCountries_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(247, 13);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // GetAllCountriesOrCities
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 370);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnGetAllCountries);
            this.Controls.Add(this.btnGetAllCities);
            this.Controls.Add(this.dgv);
            this.Name = "GetAllCountriesOrCities";
            this.Text = "GetAllCountriesOrCities";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button btnGetAllCities;
        private System.Windows.Forms.Button btnGetAllCountries;
        private System.Windows.Forms.Button btnClear;
    }
}