using NLog;
using System;
using System.Windows.Forms;

namespace TvUndergroundDownloader
{
    public partial class FormLogin : Form
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private static char[] charsToTrim = { ' ', 't', 'h', 'i', '=' };

        public string CookieT { private set; get; } = string.Empty;
        public string CookieI { private set; get; } = string.Empty;
        public string CookieH { private set; get; } = string.Empty;

        public FormLogin()
        {
            //
            //  this avoid strang behaviour
            //
            this.DialogResult = DialogResult.Cancel;

            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://tvunderground.org.ru/");
            GoogleAnalyticsHelper.TrackScreen("Login");
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            string tempT = string.Empty;
            string tempI = string.Empty;
            string tempH = string.Empty;

            var page = webBrowser1.Document;

            if (page.Cookie == string.Empty)
            {
                return;
            }

            if (string.IsNullOrEmpty(page.Cookie) == true)
                return;

            string[] cookies = page.Cookie.Split(';');

#if DEBUG
            //
            //  show debug log
            //
            _logger.Debug(string.Format("Form Login ({0}):{1}",webBrowser1.Url, page.Cookie));
            foreach (string cookie in cookies)
            {
                _logger.Debug(cookie);
            }
#endif
            foreach (string cookie in cookies)
            {
                //
                //  search T
                //
                if (cookie.IndexOf("t=") > -1)
                {
                    tempT = cookie.Trim(charsToTrim);
                    continue;
                }
                //
                //  search I
                //
                if (cookie.IndexOf("i=") > -1)
                {
                    tempI = cookie.Trim(charsToTrim);
                    continue;
                }
                //
                //  search H
                //
                if (cookie.IndexOf("h=") > -1)
                {
                    tempH = cookie.Trim(charsToTrim);
                    continue;
                }
            }

            //
            //  check that all 3 cookies (T, I , H) are not empty
            //
            if ((tempT == string.Empty) | (tempI == string.Empty) | (tempH == string.Empty))
            {
                return;
            }

            _logger.Debug("cookie t=" + tempT);
            _logger.Debug("cookie i=" + tempI);
            _logger.Debug("cookie h=" + tempH);

            this.CookieT = tempT;
            this.CookieI = tempI;
            this.CookieH = tempH;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}