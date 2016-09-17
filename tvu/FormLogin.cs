using System;
using System.Windows.Forms;

namespace TvUndergroundDownloader
{
    public partial class FormLogin : Form
    {
        static char[] charsToTrim = { ' ', 't', 'h', 'i', '=' };

        public string cookieT { private set; get; }
        public string cookieI { private set; get; }
        public string cookieH { private set; get; }

        public FormLogin()
        {
            cookieT = string.Empty;
            cookieI = string.Empty;
            cookieH = string.Empty;

            //
            //  this avoid strang behaviour
            //
            this.DialogResult = DialogResult.Cancel;
            
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://tvunderground.org.ru/");
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
            Log.logDebug(string.Format("Form Login ({0}):{1}",webBrowser1.Url, page.Cookie));
            foreach (string cookie in cookies)
            {
                Log.logDebug(cookie);
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
            if ((tempT == string.Empty) |(tempI == string.Empty) |(tempH == string.Empty))
            {
                return;
            }

            Log.logVerbose("cookie t=" + tempT);
            Log.logVerbose("cookie i=" + tempI);
            Log.logVerbose("cookie h=" + tempH);

            this.cookieT = tempT;
            this.cookieI = tempI;
            this.cookieH = tempH;

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
