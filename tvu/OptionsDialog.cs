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
        //public Config LocalConfig;

        public string ServiceUrl {private set;get;}
        public string Password;
        public int IntervalTime;
        public bool StartMinimized;
        public bool CloseEmuleIfAllIsDone;
        public bool StartEmuleIfClose;
        public bool AutoClearLog;
        public string eMuleExe;
        public bool debug;
        public string DefaultCategory;
        public bool Enebled;
        public int MaxSimultaneousFeedDownloads;
        public int MinToStartEmule;
        public bool Verbose;
        public bool EmailNotification;
        public string ServerSMTP;
        public string MailReceiver;
        public string MailSender;
        public int intervalBetweenUpgradeCheck;
        public string LastUpgradeCheck;

        public bool saveLog;
        public bool StartWithWindows;

        public OptionsDialog(Config inConfig)
        {
            InitializeComponent();

            textBoxServiceUrl.Text = ServiceUrl = inConfig.ServiceUrl;
            textBoxPassword.Text = Password = inConfig.Password;
            textBoxEmuleExe.Text = eMuleExe = inConfig.eMuleExe;
            textBoxDefaultCategory.Text = DefaultCategory = inConfig.DefaultCategory;
            textBoxMailReceiver.Text = MailReceiver = inConfig.MailReceiver;
            textBoxMailSender.Text = MailSender = inConfig.MailSender;
            textBoxServerSmtp.Text = ServerSMTP = inConfig.ServerSMTP;

            numericUpDownIntervalTime.Value = IntervalTime = inConfig.IntervalTime;
            numericUpDownMinDownloadToStrarTEmule.Value = MinToStartEmule = inConfig.MinToStartEmule;
            numericUpDownIntervalCheck.Value = intervalBetweenUpgradeCheck = inConfig.intervalBetweenUpgradeCheck;

            numericUpDownMaxSimultaneousDownloadForFeed.Value = MaxSimultaneousFeedDownloads = inConfig.MaxSimultaneousFeedDownloads;
            checkBoxStartMinimized.Checked = StartMinimized = inConfig.StartMinimized;
            checkBoxStartEmuleIfClose.Checked = StartEmuleIfClose = inConfig.StartEmuleIfClose;
            checkBoxCloseEmuleIfAllIsDone.Checked = CloseEmuleIfAllIsDone = inConfig.CloseEmuleIfAllIsDone;
            checkBoxStartWithWindows.Checked = StartWithWindows = Config.StartWithWindows;
            checkBoxAutoClear.Checked = AutoClearLog = inConfig.AutoClearLog;
            checkBoxVerbose.Checked = Verbose = inConfig.Verbose;
            checkBoxEmailNotification.Checked = EmailNotification = inConfig.EmailNotification;
            checkBoxSaveLogToFile.Checked = saveLog = inConfig.saveLog;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            //
            // StartMinimized
            //
            StartMinimized = false;
            if (checkBoxStartMinimized.Checked == true)
            {
                StartMinimized = true;
            }
            //
            // StartWithWindows
            //
            if (checkBoxStartWithWindows.Checked == true)
            {
                StartWithWindows = true;
            }
            else
            {
                StartWithWindows = false;
            }
            //
            // StartEmuleIfClose
            //
            StartEmuleIfClose = false;
            if (checkBoxStartEmuleIfClose.Checked == true)
            {
                StartEmuleIfClose = true;
            }
            //
            // CloseWhenAllDone
            //
            CloseEmuleIfAllIsDone = false;
            if (checkBoxCloseEmuleIfAllIsDone.Checked == true)
            {
                CloseEmuleIfAllIsDone = true;
            }

            AutoClearLog = false;
            if (checkBoxAutoClear.Checked == true)
            {
                AutoClearLog = true;
            }
            //
            //  Verbosen
            Verbose = checkBoxVerbose.Checked;
            //

            //
            //  Service Url
            //
            ServiceUrl = textBoxServiceUrl.Text;
            if(  ServiceUrl.IndexOf("http://") == -1)
            {
                ServiceUrl = "http://" + ServiceUrl;
            }
            //
            //  Password
            //
            Password = textBoxPassword.Text;
            //
            // Default Category
            //
            DefaultCategory = textBoxDefaultCategory.Text;
            //
            // Emule Exe
            //
            eMuleExe = textBoxEmuleExe.Text;
            //
            //  Interval time
            //
            IntervalTime = Convert.ToInt32(numericUpDownIntervalTime.Value);
            //
            //  Min download to start emule
            //
            MinToStartEmule = Convert.ToInt32(numericUpDownMinDownloadToStrarTEmule.Value);
            //
            //
            //
            EmailNotification = checkBoxEmailNotification.Checked;
            //
            // emeil server
            //
            ServerSMTP = textBoxServerSmtp.Text;
            //
            //  email Sender
            //
            MailSender = textBoxMailSender.Text;
            //
            //  email receiver
            //
            MailReceiver = textBoxMailReceiver.Text;
            //
            //  Interval Check
            //
            intervalBetweenUpgradeCheck = Convert.ToInt32(numericUpDownIntervalCheck.Value);
            //
            //
            //
            MaxSimultaneousFeedDownloads = Convert.ToInt32(numericUpDownMaxSimultaneousDownloadForFeed.Value);
            
            //
            //  Save log to file
            //
            saveLog = false;
            if (checkBoxSaveLogToFile.Checked == true)
            {
                saveLog = true;
            }


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

        private void buttonTestEmailNotification_Click(object sender, EventArgs e)
        {
            string stmpServer = textBoxServerSmtp.Text;
            string EmailReceiver = textBoxMailReceiver.Text;
            string EmailSender = textBoxMailSender.Text;
            string Subject = "Email Test";
            string message = "Email Test";
            SmtpClient.SendEmail(stmpServer, EmailReceiver, EmailSender, Subject, message);

        }



    }
}