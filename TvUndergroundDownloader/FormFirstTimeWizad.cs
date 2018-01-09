using System;
using System.Diagnostics;
using System.Windows.Forms;
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

        public string Password { get; set; }
        public string ServerUrl { set; get; } = "http://localhost:4711";
        public string ServiceAddress { get; set; } = "localhost";
        public int ServicePortNumber { get; set; } = 4711;
        public Config.eServiceType ServiceType { get; set; }
        public string TVUCookieH { set; get; }
        public string TVUCookieI { set; get; }
        public string TVUCookieT { set; get; }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void buttonLoginToTVUnderground_Click(object sender, EventArgs e)
        {

            FormLogin form = new FormLogin();
            form.ShowDialog();

            if (form.DialogResult != DialogResult.OK)
            {
                return;
            }

            this.TVUCookieT = form.CookieT;
            this.textBoxCookieT.Text = form.CookieT;
            this.TVUCookieI = form.CookieI;
            this.textBoxCookieI.Text = form.CookieI;
            this.TVUCookieH = form.CookieH;
            this.textBoxCookieH.Text = form.CookieH;
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
                if (string.IsNullOrEmpty(this.TVUCookieI))
                {
                    msg = "An error occurred, do you want continue?\r\nCookies are not setted";
                }

                if (string.IsNullOrEmpty(this.TVUCookieH))
                {
                    msg = "An error occurred, do you want continue?\r\nCookies are not setted";
                }

                if (string.IsNullOrEmpty(this.TVUCookieT))
                {
                    msg = "An error occurred, do you want continue?\r\nCookies are not setted";
                }
                if (msg != null)
                {
                    if (MessageBox.Show(msg, "Warning", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return;
                    }
                }


                tabControl1.SelectedTab = tabPage7;
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;
                buttonNext.Text = "Finish";
                return;
            }


            if (tabControl1.SelectedTab == tabPage7)
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
                return;
            }

            if (tabControl1.SelectedTab == tabPage3)
            {
                tabControl1.SelectedTab = tabPage2;
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;
                return;
            }

            if (tabControl1.SelectedTab == tabPage4)
            {
                tabControl1.SelectedTab = tabPage3;
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;
                return;
            }

            if (tabControl1.SelectedTab == tabPage5)
            {

                tabControl1.SelectedTab = tabPage4;
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;
                return;
            }


            if (tabControl1.SelectedTab == tabPage6)
            {
                tabControl1.SelectedTab = tabPage5;
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;
                return;
            }


            if (tabControl1.SelectedTab == tabPage7)
            {
                tabControl1.SelectedTab = tabPage6;
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;
                buttonNext.Text = "Next";
                return;
            }

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

            textBoxCookieT.Text = this.TVUCookieT;
            textBoxCookieH.Text = this.TVUCookieH;
            textBoxCookieI.Text = this.TVUCookieI;

            textBoxAddress.Text = this.ServiceAddress;
            textBoxServicePort.Text = this.ServicePortNumber.ToString();
            textBoxPassword.Text = this.Password;
            linkLabelAddress.Text = string.Format("http://{0}:{1}", ServiceAddress, ServicePortNumber);
        }

        private void linkLabelAddress_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabelAddress.Text);
        }

        private void textBoxAddress_TextChanged(object sender, EventArgs e)
        {
            this.ServiceAddress = textBoxAddress.Text;
            this.ServerUrl = string.Format("http://{0}:{1}", ServiceAddress, ServicePortNumber);

            linkLabelAddress.Text = string.Format("http://{0}:{1}", ServiceAddress, ServicePortNumber);
        }

        private void textBoxCookieH_TextChanged(object sender, EventArgs e)
        {
            this.TVUCookieH = textBoxCookieH.Text;
        }

        private void textBoxCookieI_TextChanged(object sender, EventArgs e)
        {
            this.TVUCookieI = textBoxCookieI.Text;
        }

        private void textBoxCookieT_TextChanged(object sender, EventArgs e)
        {
            this.TVUCookieT = textBoxCookieT.Text;
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
    }
}