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
        public string Category { private set; get; }
        public bool PauseDownload { private set; get; }
        public int maxSimultaneousDownload { private set; get; }
        public bool feedEnable { private set; get; }

        public EditFeedForm(string Category, bool PauseDownload, bool feedEnable, int maxSimultaneousDownload)
        {
            this.PauseDownload = PauseDownload;
            this.Category = Category;
            this.maxSimultaneousDownload = maxSimultaneousDownload;
            this.feedEnable = feedEnable;
            InitializeComponent();


            // TODO: Add updated list category
        }

        private void EditFeedForm_Load(object sender, EventArgs e)
        {
            this.numericUpDown1.Value = this.maxSimultaneousDownload;
            this.comboBoxCategory.Text = this.Category;
            this.checkBoxDownloadinPause.Checked = this.PauseDownload;
            this.checkBoxEnable.Checked = this.feedEnable;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.Category = comboBoxCategory.Text;
            this.PauseDownload = this.checkBoxDownloadinPause.Checked;
            this.feedEnable = this.checkBoxEnable.Checked;
            this.DialogResult = DialogResult.OK;
            this.maxSimultaneousDownload = Convert.ToInt16(numericUpDown1.Value);
            
            this.Close();
        }

      

    }
}
