using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace tvu
{
    public partial class EditFeedForm : Form
    {
        public string category { private set; get; }
        public bool pause { private set; get; }

        public EditFeedForm(string category, bool pause)
        {
            this.pause = pause;
            this.category = category;

            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.category = comboBoxCategory.Text;
            this.pause = this.checkBoxDownloadinPause.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
