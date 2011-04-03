using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace tvu
{
    public partial class OptionsDialog : Form
    {
        public Config LocalConfig;
        
        public OptionsDialog(Config inConfig)
        {
            InitializeComponent();
            LocalConfig = new Config();


            textBoxServiceUrl.Text = inConfig.ServiceUrl;
            textBoxPassword.Text = inConfig.Password;
            textBoxEmuleExe.Text = inConfig.eMuleExe;
            textBoxDefaultCategory.Text = LocalConfig.DefaultCategory;
            numericUpDownIntervalTime.Value = inConfig.IntervalTime;
            numericUpDownMinDownloadToStrarTEmule.Value = inConfig.MinToStartEmule;
            checkBoxStartMinimized.Checked = inConfig.StartMinimized;
            checkBoxStartEmuleIfClose.Checked = inConfig.StartEmuleIfClose;
            checkBoxCloseEmuleIfAllIsDone.Checked = inConfig.CloseEmuleIfAllIsDone;
            checkBoxStartWithWindows.Checked = inConfig.StartWithWindows;
            checkBoxAutoClear.Checked = inConfig.AutoClearLog;
            checkBoxVerbose.Checked = inConfig.Verbose;



        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            //
            // StartMinimized
            //
            LocalConfig.StartMinimized = false;
            if (checkBoxStartMinimized.Checked == true)
            {
                LocalConfig.StartMinimized = true;
            }
            //
            // StartWithWindows
            //
            LocalConfig.StartWithWindows = false;
            if (checkBoxStartWithWindows.Checked == true)
            {
                LocalConfig.StartWithWindows = true;
            }
            //
            // StartEmuleIfClose
            //
            LocalConfig.StartEmuleIfClose = false;
            if (checkBoxStartEmuleIfClose.Checked == true)
            {
                LocalConfig.StartEmuleIfClose = true;
            }
            //
            // CloseWhenAllDone
            //
            LocalConfig.CloseEmuleIfAllIsDone = false;
            if (checkBoxCloseEmuleIfAllIsDone.Checked == true)
            {
                LocalConfig.CloseEmuleIfAllIsDone = true;
            }

            LocalConfig.AutoClearLog = false;
            if (checkBoxAutoClear.Checked == true)
            {
                LocalConfig.AutoClearLog = true;
            }
            //
            //  Verbosen
            LocalConfig.Verbose = checkBoxVerbose.Checked;
            //

            //
            //  Service Url
            //
            LocalConfig.ServiceUrl = textBoxServiceUrl.Text;
            //
            //  Password
            //
            LocalConfig.Password = textBoxPassword.Text;
            //
            // Default Category
            //
            LocalConfig.DefaultCategory = textBoxDefaultCategory.Text; 
            //
            // Emule Exe
            //
            LocalConfig.eMuleExe = textBoxEmuleExe.Text;
            //
            //  Interval time
            //
            LocalConfig.IntervalTime = Convert.ToInt32(numericUpDownIntervalTime.Value);
            //
            //  Min download to start emule
            //
            LocalConfig.MinToStartEmule = Convert.ToInt32(numericUpDownMinDownloadToStrarTEmule.Value);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCheckNow_Click(object sender, EventArgs e)
        {
            eMuleWebManager service = new eMuleWebManager(textBoxServiceUrl.Text, textBoxPassword.Text);
            bool? rc = service.LogIn();

            if (rc == null)
            {
                MessageBox.Show("Unable conncet with target URL");
                return;
            }

            if (rc == false)
            {
                MessageBox.Show("Password error");
                return;
            }

            MessageBox.Show("OK service is correctly configured");
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


  
    }
}
