using System;
using System.Windows.Forms;
using TvUndergroundDownloaderLib;

namespace TvUndergroundDownloader
{
    public partial class FormOptions : Form
    {
        public FormOptions(ConfigWindows inConfig)
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

            textBoxServiceUrl.Text = inConfig.ServiceUrl;
            textBoxPassword.Text = inConfig.Password;

            textBoxEmuleExe.Text = inConfig.eMuleExe;
            textBoxDefaultCategory.Text = inConfig.DefaultCategory;

            numericUpDownIntervalTime.Value = inConfig.IntervalTime;
            numericUpDownMinDownloadToStrarTEmule.Value = inConfig.MinToStartEmule;
            numericUpDownIntervalCheck.Value = inConfig.IntervalBetweenUpgradeCheck;

            numericUpDownMaxSimultaneousDownloadForFeed.Value = inConfig.MaxSimultaneousFeedDownloadsDefault;
            checkBoxStartMinimized.Checked = inConfig.StartMinimized;
            checkBoxStartEmuleIfClose.Checked = inConfig.StartEmuleIfClose;
            checkBoxCloseEmuleIfAllIsDone.Checked = inConfig.CloseEmuleIfAllIsDone;
            checkBoxStartWithWindows.Checked = ConfigWindows.StartWithWindows;
            checkBoxAutoClear.Checked = inConfig.AutoClearLog;
            checkBoxVerbose.Checked = inConfig.Verbose;

            // EMAIL
            checkBoxEmailNotification.Checked = inConfig.EmailNotification;
            buttonTestEmailNotification.Enabled = checkBoxEmailNotification.Checked;
            textBoxSmtpServerAddress.Enabled = checkBoxEmailNotification.Checked;
            textBoxSmtpServerPort.Enabled = checkBoxEmailNotification.Checked;
            checkBoxEnableSSL.Enabled = checkBoxEmailNotification.Checked;
            checkBoxEnableAuthentication.Enabled = checkBoxEmailNotification.Checked;
            textBoxSmtpServerUsername.Enabled = checkBoxEmailNotification.Checked;
            textBoxSmtpServerPassword.Enabled = checkBoxEmailNotification.Checked;

            textBoxSmtpServerUsername.Enabled = checkBoxEnableAuthentication.Checked;
            textBoxSmtpServerPassword.Enabled = checkBoxEnableAuthentication.Checked;

            textBoxMailSender.Enabled = checkBoxEmailNotification.Checked;
            textBoxMailReceiver.Enabled = checkBoxEmailNotification.Checked;

            textBoxSmtpServerAddress.Text = inConfig.SmtpServerAddress;
            textBoxSmtpServerPort.Text = inConfig.SmtpServerPort.ToString();
            checkBoxEnableSSL.Checked = inConfig.SmtpServerEnableSsl;
            checkBoxEnableAuthentication.Checked = inConfig.SmtpServerEnableAuthentication;
            textBoxSmtpServerUsername.Text = inConfig.SmtpServerUserName;
            textBoxSmtpServerPassword.Text = inConfig.SmtpServerPassword;
            textBoxMailReceiver.Text = inConfig.MailReceiver;
            textBoxMailSender.Text = inConfig.MailSender;

            // Coockie
            textBoxTvuUsername.Text = inConfig.TvuUserName;
            textBoxTvuPassword.Text = inConfig.TvuPassword;


            textBoxWebServerPortNumber.Text = inConfig.WebServerPort.ToString();

            checkBoxWebServerEnabled.Checked = inConfig.WebServerEnable;
            textBoxWebServerPortNumber.Enabled = inConfig.WebServerEnable;
            textBoxWebServerPortNumber.Text = inConfig.WebServerPort.ToString();
        }

        public bool AutoClearLog => checkBoxAutoClear.Checked;

        public bool CloseEmuleIfAllIsDone => checkBoxCloseEmuleIfAllIsDone.Checked;

        public string DefaultCategory => textBoxDefaultCategory.Text;

        public bool EmailNotification => checkBoxEmailNotification.Checked;
        public string eMuleExe => textBoxEmuleExe.Text;

        public int IntervalBetweenUpgradeCheck => Convert.ToInt32(numericUpDownIntervalCheck.Value);

        public int IntervalTime => Convert.ToInt32(numericUpDownIntervalTime.Value);
        public string MailReceiver => textBoxMailReceiver.Text;
        public string MailSender => textBoxMailSender.Text;
        public uint MaxSimultaneousFeedDownloads => Convert.ToUInt32(numericUpDownMaxSimultaneousDownloadForFeed.Value);
        public int MinToStartEmule => Convert.ToInt32(numericUpDownMinDownloadToStrarTEmule.Value);
        public string Password => textBoxPassword.Text;
        public Config.eServiceType ServiceType { get; set; }
        public string ServiceUrl => textBoxServiceUrl.Text;
        public string SmtpServerAddress => textBoxSmtpServerAddress.Text;
        public bool SmtpServerEnableAuthentication => checkBoxEnableAuthentication.Checked;
        public bool SmtpServerEnableSsl => checkBoxEnableSSL.Checked;
        public string SmtpServerPassword => textBoxSmtpServerPassword.Text;
        public int SmtpServerPort => int.Parse(textBoxSmtpServerPort.Text);
        public string SmtpServerUserName => textBoxSmtpServerUsername.Text;
        public bool StartEmuleIfClose => checkBoxStartEmuleIfClose.Checked;
        public bool StartMinimized => checkBoxStartMinimized.Checked;
        public bool StartWithWindows => checkBoxStartWithWindows.Checked;
        public string TvuUsername => textBoxTvuUsername.Text;
        public string TvuPassword => textBoxTvuPassword.Text;
        public bool Verbose => checkBoxVerbose.Checked;
        public bool WebServerEnable => checkBoxWebServerEnabled.Checked;
        public int WebServerPort => int.Parse(textBoxSmtpServerPort.Text);

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
                MessageBox.Show("Unable to conncet to URL");
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
            //  Service Url
            //
            if (ServiceUrl.IndexOf("http://") == -1)
            {
                MessageBox.Show("Service url not valid");
                return;
            }

            //
            //  Check numeric text
            //
            int tempInt;
            if (!int.TryParse(textBoxSmtpServerPort.Text, out tempInt))
            {
                MessageBox.Show("Invalid Smtp port number");
                return;
            }

            if (!int.TryParse(textBoxWebServerPortNumber.Text, out tempInt))
            {
                MessageBox.Show("Invalid web server port number");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonTestEmailNotification_Click(object sender, EventArgs e)
        {
            try
            {
                string subject = "Email Test";
                string message = "Email Test";
                SmtpSimpleClient simpleClient = new SmtpSimpleClient();
                simpleClient.SmtpClientHost = textBoxSmtpServerAddress.Text;
                simpleClient.SmtpClientPort = int.Parse(textBoxSmtpServerPort.Text);
                simpleClient.SmtpServerEnableSsl = checkBoxEnableSSL.Checked;
                simpleClient.SmtpServerEnableCredential = checkBoxEnableAuthentication.Checked;
                simpleClient.SmtpServerUserName = textBoxSmtpServerUsername.Text;
                simpleClient.SmtpServerPassword = textBoxSmtpServerPassword.Text;
                simpleClient.MailSender = textBoxMailSender.Text;
                simpleClient.MailReceiver = textBoxMailReceiver.Text;
                simpleClient.SendEmail(subject, message);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void checkBoxEmailNotification_CheckedChanged(object sender, EventArgs e)
        {
            buttonTestEmailNotification.Enabled = checkBoxEmailNotification.Checked;
            textBoxSmtpServerAddress.Enabled = checkBoxEmailNotification.Checked;
            textBoxSmtpServerPort.Enabled = checkBoxEmailNotification.Checked;
            checkBoxEnableSSL.Enabled = checkBoxEmailNotification.Checked;
            checkBoxEnableAuthentication.Enabled = checkBoxEmailNotification.Checked;
            textBoxSmtpServerUsername.Enabled = checkBoxEnableAuthentication.Checked;
            textBoxSmtpServerPassword.Enabled = checkBoxEnableAuthentication.Checked;
            textBoxMailSender.Enabled = checkBoxEmailNotification.Checked;
            textBoxMailReceiver.Enabled = checkBoxEmailNotification.Checked;
        }

        private void checkBoxEnableAutentication_CheckedChanged(object sender, EventArgs e)
        {
            textBoxSmtpServerUsername.Enabled = checkBoxEnableAuthentication.Checked;
            textBoxSmtpServerPassword.Enabled = checkBoxEnableAuthentication.Checked;
        }

        private void checkBoxWebServerEnabled_CheckedChanged(object sender, EventArgs e)
        {
            textBoxWebServerPortNumber.Enabled = checkBoxWebServerEnabled.Checked;
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

        private void OptionsDialog_Load(object sender, EventArgs e)
        {
            GoogleAnalyticsHelper.TrackScreen("Options");
        }


    }
}