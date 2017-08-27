using System;
using System.Windows.Forms;
using TvUndergroundDownloaderLib;

namespace TvUndergroundDownloader
{
    public partial class OptionsDialog : Form
    {
        public bool AutoClearLog;

        public bool CloseEmuleIfAllIsDone;

        public bool debug;

        public string DefaultCategory;

        public bool EmailNotification;

        public string eMuleExe;

        public bool Enebled;

        public int intervalBetweenUpgradeCheck;

        public int IntervalTime;

        public string LastUpgradeCheck;

        public string MailReceiver;

        public string MailSender;

        public uint MaxSimultaneousFeedDownloads;

        public int MinToStartEmule;

        public string Password;

        public string ServerSMTP;

        //public Config LocalConfig;
        public Config.eServiceType ServiceType;

        public bool StartEmuleIfClose;
        public bool StartMinimized;
        public bool StartWithWindows;
        public string tvuCookieH;
        public string tvuCookieI;
        public string tvuCookieT;
        public string tvuPassword;
        public string tvuUsername;
        public bool Verbose;

        public OptionsDialog(ConfigWindows inConfig)
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
            numericUpDownIntervalCheck.Value = intervalBetweenUpgradeCheck = inConfig.IntervalBetweenUpgradeCheck;

            numericUpDownMaxSimultaneousDownloadForFeed.Value = MaxSimultaneousFeedDownloads = inConfig.MaxSimultaneousFeedDownloadsDefault;
            checkBoxStartMinimized.Checked = StartMinimized = inConfig.StartMinimized;
            checkBoxStartEmuleIfClose.Checked = StartEmuleIfClose = inConfig.StartEmuleIfClose;
            checkBoxCloseEmuleIfAllIsDone.Checked = CloseEmuleIfAllIsDone = inConfig.CloseEmuleIfAllIsDone;
            checkBoxStartWithWindows.Checked = StartWithWindows = ConfigWindows.StartWithWindows;
            checkBoxAutoClear.Checked = AutoClearLog = inConfig.AutoClearLog;
            checkBoxVerbose.Checked = Verbose = inConfig.Verbose;
            checkBoxEmailNotification.Checked = EmailNotification = inConfig.EmailNotification;
            tvuCookieH = textBoxCookieH.Text = inConfig.TVUCookieH;
            tvuCookieI = textBoxCookieI.Text = inConfig.TVUCookieI;
            tvuCookieT = textBoxCookieT.Text = inConfig.TVUCookieT;

            textBoxWebServerPortNumber.Text = inConfig.WebServerPort.ToString();
            WebServerEnable = checkBoxWebServerEnabled.Checked = inConfig.WebServerEnable;
            WebServerPort = inConfig.WebServerPort;

        }

        public string ServiceUrl { private set; get; }
        public bool WebServerEnable { get; set; }

        public int WebServerPort { get; set; }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
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
            //  web server
            //
            WebServerEnable = checkBoxWebServerEnabled.Checked;
            WebServerPort = Convert.ToInt32(textBoxWebServerPortNumber.Text);

            this.DialogResult = DialogResult.OK;
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