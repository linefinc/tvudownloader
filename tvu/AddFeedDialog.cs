using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace tvu
{
    public partial class AddFeedDialog : Form
    {
        public AddFeedDialog()
        {
            InitializeComponent();
        }

        public AddFeedDialog(string Url, string Category, bool DownloadPause)
        {
            InitializeComponent();

            textUrl.Text = Url;

            this.comboBoxCategory.Items.Add(Category);

            checkBoxPause.Checked = DownloadPause;

        }

        private void ButClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }






    }
}
