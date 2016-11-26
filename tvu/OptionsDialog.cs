using System;
using System.Windows.Forms;

namespace TvUndergroundDownloader
{
    public partial class OptionsDialog : Form
    {
        //public Config LocalConfig;
        public Config.eServiceType ServiceType;
        public string ServiceUrl { private set; get; }
        public string Password;
        public string tvuUsername;
        public string tvuPassword;
        public string tvuCookieH;
        public string tvuCookieI;
        public string tvuCookieT;
        public int IntervalTime;
        public bool StartMinimized;
        public bool CloseEmuleIfAllIsDone;
        public bool StartEmuleIfClose;
        public bool AutoClearLog;
        public string eMuleExe;
        public bool debug;
        public string DefaultCategory;
        public bool Enebled;
        public uint MaxSimultaneousFeedDownloads;
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

            ServiceType = inConfig.ServiceType;
            switch (ServiceType)
            {
                case Config.eServiceType.aMule:
                    comboBoxClientType.Text = "aMule";
                    break;
                case Config.eServiceType.eMule:
                default:
                    comboBoxClientType.Text = "eMule";
                    break;
            }

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

            numericUpDownMaxSimultaneousDownloadForFeed.Value = MaxSimultaneousFeedDownloads = inConfig.MaxSimultaneousFeedDownloadsDefault;
            checkBoxStartMinimized.Checked = StartMinimized = inConfig.StartMinimized;
            checkBoxStartEmuleIfClose.Checked = StartEmuleIfClose = inConfig.StartEmuleIfClose;
            checkBoxCloseEmuleIfAllIsDone.Checked = CloseEmuleIfAllIsDone = inConfig.CloseEmuleIfAllIsDone;
            checkBoxStartWithWindows.Checked = StartWithWindows = Config.StartWithWindows;
            checkBoxAutoClear.Checked = AutoClearLog = inConfig.AutoClearLog;
            checkBoxVerbose.Checked = Verbose = inConfig.Verbose;
            checkBoxEmailNotification.Checked = EmailNotification = inConfig.EmailNotification;
            checkBoxSaveLogToFile.Checked = saveLog = inConfig.saveLog;
            tvuCookieH = textBoxCookieH.Text = inConfig.tvuCookieH;
            tvuCookieI = textBoxCookieI.Text = inConfig.tvuCookieI;
            tvuCookieT = textBoxCookieT.Text = inConfig.tvuCookieT;
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
            if (ServiceUrl.IndexOf("http://") == -1)
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
            MaxSimultaneousFeedDownloads = Convert.ToUInt32(numericUpDownMaxSimultaneousDownloadForFeed.Value);


            tvuCookieH = textBoxCookieH.Text;
            tvuCookieI = textBoxCookieI.Text;
            tvuCookieT = textBoxCookieT.Text;

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
            IMuleWebManager service = null;
            switch (ServiceType)
            {
                case Config.eServiceType.aMule:
                    service = new aMuleWebManager(textBoxServiceUrl.Text, textBoxPassword.Text);
                    break;
                case Config.eServiceType.eMule:
                default:
                    service = new eMuleWebManager(textBoxServiceUrl.Text, textBoxPassword.Text);
                    break;
            }

            LoginStatus rc = service.Connect();

            if (rc == LoginStatus.ServiceNotAvailable)
            {
                MessageBox.Show("Unable conncet with target URL");
                return;
            }

            if (rc == LoginStatus.PasswordError)
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

        private void comboBoxClientType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxClientType.Text)
            {
                case "aMule":
                    ServiceType = Config.eServiceType.aMule;
                    break;
                case "eMule":
                default:
                    ServiceType = Config.eServiceType.eMule;
                    break;
            }
        }
    }
}