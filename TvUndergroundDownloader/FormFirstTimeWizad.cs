using System;
using System.Diagnostics;
using System.Windows.Forms;
using TvUndergroundDownloader.Properties;
using TvUndergroundDownloaderLib;

namespace TvUndergroundDownloader
{
    public partial class FormFirstTimeWizad : Form
    {
        public FormFirstTimeWizad()
        {
            InitializeComponent();
            comboBoxClientType.Text = "eMule";
        }

        public string eMuleApp { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string ServerUrl { set; get; } = "http://localhost:4711";

        public string ServiceAddress { get; set; } = "localhost";
        public int ServicePortNumber { get; set; } = 4711;
        public Config.eServiceType ServiceType { get; set; }

        public string TvuPassword { get; set; } = string.Empty;
        public string TvuUserName { get; set; } = string.Empty;
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

      

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                tabControl1.SelectedTab = tabPage2;
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;
                return;
            }

            if (tabControl1.SelectedTab == tabPage2)
            {
                tabControl1.SelectedTab = tabPage3;
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;
                return;
            }

            if (tabControl1.SelectedTab == tabPage2)
            {
                tabControl1.SelectedTab = tabPage3;
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;
                return;
            }

            if (tabControl1.SelectedTab == tabPage3)
            {
                tabControl1.SelectedTab = tabPage4;
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;

                return;
            }

            if (tabControl1.SelectedTab == tabPage4)
            {
                tabControl1.SelectedTab = tabPage5;
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;

                return;
            }

            if (tabControl1.SelectedTab == tabPage5)
            {
                IMuleWebManager service = null;
                switch (ServiceType)
                {
                    case Config.eServiceType.aMule:
                        service = new aMuleWebManager(ServerUrl, textBoxPassword.Text);
                        break;

                    case Config.eServiceType.eMule:
                    default:
                        service = new eMuleWebManager(ServerUrl, textBoxPassword.Text);
                        break;
                }

                LoginStatus rc = service.Connect();

                string msg = null;

                if (rc == LoginStatus.ServiceNotAvailable)
                {
                    msg = "Unable to conncet to URL";
                }

                if (rc == LoginStatus.PasswordError)
                {
                    msg = "Password error";
                }

                if (msg != null)
                {
                    msg = "An error occurred, do you want continue?\r\n" + msg;
                    if (MessageBox.Show(msg, "Warning", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return;
                    }
                }

                tabControl1.SelectedTab = tabPage6;
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;

                return;
            }

        

            if (tabControl1.SelectedTab == tabPage6)
            {
                string msg = null;
                if (string.IsNullOrEmpty(this.TvuUserName))
                {
                    msg = "An error occurred, do you want continue?\r\nTvu UserName are not setted";
                }

                if (string.IsNullOrEmpty(this.TvuPassword))
                {
                    msg = "An error occurred, do you want continue?\r\nTvu Password are not setted";
                }

                if (msg != null)
                {
                    if (MessageBox.Show(msg, "Warning", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return;
                    }
                }

                tabControl1.SelectedTab = tabPage9;
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;
                buttonNext.Text = "Complete";
                return;
            }

            if (tabControl1.SelectedTab == tabPage9)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                return;
            }

            if (tabControl1.SelectedTab == tabPage2)
            {
                tabControl1.SelectedTab = tabPage1;
                buttonPrev.Enabled = false;
                buttonNext.Enabled = true;
                buttonNext.Text = Resources.FormFirstTimeWizad_buttonPrev_Click_Next;
                return;
            }

            if (tabControl1.SelectedTab == tabPage3)
            {
                tabControl1.SelectedTab = tabPage2;
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;
                buttonNext.Text = Resources.FormFirstTimeWizad_buttonPrev_Click_Next;
                return;
            }

            if (tabControl1.SelectedTab == tabPage4)
            {
                tabControl1.SelectedTab = tabPage3;
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;
                buttonNext.Text = Resources.FormFirstTimeWizad_buttonPrev_Click_Next;
                return;
            }

            if (tabControl1.SelectedTab == tabPage5)
            {
                tabControl1.SelectedTab = tabPage4;
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;
                buttonNext.Text = Resources.FormFirstTimeWizad_buttonPrev_Click_Next;
                return;
            }

            if (tabControl1.SelectedTab == tabPage6)
            {
                tabControl1.SelectedTab = tabPage5;
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;
                buttonNext.Text = Resources.FormFirstTimeWizad_buttonPrev_Click_Next;
                return;
            }

          

          
        }

        private void buttonSelectEmuleApp_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "eMule|emule.exe|Application|*.exe|All files (*.*)|*.*";

            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            eMuleApp = openFileDialog.FileName;
            textBoxEmuleApp.Text = openFileDialog.FileName;
        }

        private void buttonTestNow_Click(object sender, EventArgs e)
        {
            IMuleWebManager service = null;
            switch (ServiceType)
            {
                case Config.eServiceType.aMule:
                    service = new aMuleWebManager(ServerUrl, textBoxPassword.Text);
                    break;

                case Config.eServiceType.eMule:
                default:
                    service = new eMuleWebManager(ServerUrl, textBoxPassword.Text);
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

        private void FormFirstTimeWizad_Load(object sender, EventArgs e)
        {
            switch (ServiceType)
            {
                case Config.eServiceType.aMule:
                    comboBoxClientType.Text = "aMule";
                    break;

                default:
                case Config.eServiceType.eMule:
                    comboBoxClientType.Text = "eMule";
                    break;
            }
            textBoxTvuUserName.Text = this.TvuUserName;
            textBoxTvuPassword.Text = this.TvuPassword;
            textBoxEmuleApp.Text = this.eMuleApp;
            textBoxAddress.Text = this.ServiceAddress;
            textBoxServicePort.Text = this.ServicePortNumber.ToString();
            textBoxPassword.Text = this.Password;
            linkLabelAddress.Text = string.Format("http://{0}:{1}", ServiceAddress, ServicePortNumber);
        }

        private void linkLabelAddress_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabelAddress.Text);
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            GoogleAnalyticsHelper.TrackScreen("WizardPage1");
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            GoogleAnalyticsHelper.TrackScreen("WizardPage2");
        }

        private void tabPage3_Enter(object sender, EventArgs e)
        {
            GoogleAnalyticsHelper.TrackScreen("WizardPage3");
        }

        private void tabPage4_Enter(object sender, EventArgs e)
        {
            GoogleAnalyticsHelper.TrackScreen("WizardPage4");
        }

        private void tabPage5_Enter(object sender, EventArgs e)
        {
            GoogleAnalyticsHelper.TrackScreen("WizardPage5");
        }

        private void tabPage6_Enter(object sender, EventArgs e)
        {
            GoogleAnalyticsHelper.TrackScreen("WizardPage6");
        }

        private void tabPage7_Enter(object sender, EventArgs e)
        {
            GoogleAnalyticsHelper.TrackScreen("WizardPage7");
        }

        private void tabPage8_Enter(object sender, EventArgs e)
        {
            GoogleAnalyticsHelper.TrackScreen("WizardPage8");
        }

        private void tabPage9_Enter(object sender, EventArgs e)
        {
            GoogleAnalyticsHelper.TrackScreen("WizardPage9");
        }

        private void textBoxAddress_TextChanged(object sender, EventArgs e)
        {
            this.ServiceAddress = textBoxAddress.Text;
            this.ServerUrl = string.Format("http://{0}:{1}", ServiceAddress, ServicePortNumber);

            linkLabelAddress.Text = string.Format("http://{0}:{1}", ServiceAddress, ServicePortNumber);
        }

     
        private void textBoxEmuleApp_TextChanged(object sender, EventArgs e)
        {
            this.eMuleApp = textBoxEmuleApp.Text;
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            this.Password = textBoxPassword.Text;
        }

        private void textBoxServicePort_TextChanged(object sender, EventArgs e)
        {
            int portNumber;
            if (!int.TryParse(textBoxServicePort.Text, out portNumber))
            {
                MessageBox.Show("Port number is not valid");
                return;
            }
            this.ServicePortNumber = portNumber;
            this.ServerUrl = string.Format("http://{0}:{1}", ServiceAddress, ServicePortNumber);

            linkLabelAddress.Text = string.Format("http://{0}:{1}", ServiceAddress, ServicePortNumber);
        }

        private void textBoxTvuPassword_TextChanged(object sender, EventArgs e)
        {
            this.TvuPassword = textBoxTvuPassword.Text;
        }

        private void textBoxTvuUserName_TextChanged(object sender, EventArgs e)
        {
            this.TvuUserName = textBoxTvuUserName.Text;
        }
    }
}