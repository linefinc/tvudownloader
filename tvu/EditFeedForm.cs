using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace TvUndergroundDownloader
{
    public partial class EditFeedForm : Form
    {
        public string Category { private set; get; }
        public bool PauseDownload { private set; get; }
        public uint maxSimultaneousDownload { private set; get; }
        public bool feedEnable { private set; get; }
        private Config MainConfig;

        public EditFeedForm(Config MainConfig, string Category, bool PauseDownload, bool feedEnable, uint maxSimultaneousDownload)
        {
            InitializeComponent();

            this.PauseDownload = PauseDownload;
            this.Category = Category;
            this.maxSimultaneousDownload = maxSimultaneousDownload;
            this.feedEnable = feedEnable;
            this.MainConfig = MainConfig;
        }

        private void EditFeedForm_Load(object sender, EventArgs e)
        {
            // check max and min value allowed to numericUpDownMaxSimulDown
            decimal tempDec = Math.Max(this.maxSimultaneousDownload, numericUpDown1.Minimum);
            tempDec = Math.Min(tempDec, numericUpDown1.Maximum);

            this.numericUpDown1.Value = tempDec;
            this.comboBoxCategory.Text = this.Category;
            this.checkBoxDownloadinPause.Checked = this.PauseDownload;
            this.checkBoxEnable.Checked = this.feedEnable;

            backgroundWorker1.RunWorkerAsync();
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
            this.maxSimultaneousDownload = Convert.ToUInt16(numericUpDown1.Value);

            this.Close();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //
            // update category list from mule
            //
            try
            {
                List<string> ListCategories = new List<string>();

                eMuleWebManager Service = new eMuleWebManager(MainConfig.ServiceUrl, MainConfig.Password);
                Service.Connect();

                if (Service.isConnected == false)
                {
                    return;
                }

                ListCategories.AddRange(Service.GetCategories(true));

                Service.Close();

                foreach (string category in ListCategories)
                {
                    //
                    //  Use invoke to avoid thread issue
                    //
                    if (this.comboBoxCategory.InvokeRequired == true)
                    {
                        Invoke(new MethodInvoker(
                            delegate { this.comboBoxCategory.Items.Add(category); }
                        ));
                    }
                    else
                    {
                        this.comboBoxCategory.Items.Add(category);
                    }
                }
            }
            catch
            {
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }
    }
}