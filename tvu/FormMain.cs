using NLog;
using NLog.Config;
using NLog.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using TvUndergroundDownloader.Properties;
using TvUndergroundDownloaderLib;
using TvUndergroundDownloaderLib.EmbendedWebServer;

namespace TvUndergroundDownloader
{
    public partial class FormMain : Form
    {
        public ConfigWindows MainConfig;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private bool allowClose;
        private DateTime autoCloseDataTime;
        private ContextMenu contextMenu1;
        private DateTime downloadDataTime;
        private EmbendedWebServer embendedWebServer;
        private Icon iconUp;
        private MenuItem menuItemAutoCloseEmule;
        private MenuItem menuItemAutoStartEmule;
        private MenuItem menuItemCheckNow;
        private MenuItem menuItemEnable;
        private MenuItem menuItemExit;
        private bool mVisible = true;
        private NotifyIcon notifyIcon1;
        private Worker worker;

        public FormMain()
        {
            // load configuration
            MainConfig = new ConfigWindows();
            MainConfig.Load();

            InitializeComponent();
            SetupNotify();
            worker = new Worker();
            worker.Config = MainConfig;
            worker.WorkerCompleted += Task_RunWorkerCompleted;

#if DEBUG
            string versionFull = ((AssemblyInformationalVersionAttribute)Assembly
            .GetAssembly(typeof(FormMain))
            .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false)[0]).InformationalVersion;

            Text += " - DEBUG - " + versionFull;
#endif
            GoogleAnalyticsHelper.cid = MainConfig.tvudwid;
            GoogleAnalyticsHelper.appVersion = Config.Version;
        }

        public static string GetUserAppDataPath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path += '\\' + Application.ProductName + '\\';
            return path;
        }

        /// <summary>
        /// Show or Hide application form
        /// </summary>
        public void ApplicationShowHide()
        {
            if (Visible)
            {
                mVisible = false;
                Visible = false;
            }
            else
            {
                mVisible = true;
                Visible = true;
            }
        }

        //
        //  to insert inside a timer3
        //  here for debug
        //
        public void Autoclose()
        {
            if (MainConfig.CloseEmuleIfAllIsDone == false)
            {
                logger.Info("[AutoClose Mule] MainConfig.CloseEmuleIfAllIsDone == false");
                return;
            }

            // check if Auto Close Data Time is not set
            if (autoCloseDataTime == DateTime.MinValue)
            {
                logger.Info("[AutoClose Mule] AutoCloseDataTime = DateTime.Now.AddMinutes(30);");
                autoCloseDataTime = DateTime.Now.AddMinutes(30);
            }

            //
            // Auto close
            //
            if (DateTime.Now < autoCloseDataTime)
            {
                return;
            }

            // suspend event while connect with mule
            timerAutoClose.Enabled = false;

            // connect to mule
            try
            {
                logger.Info("[AutoClose Mule] Check Login");
                eMuleWebManager service = new eMuleWebManager(MainConfig.ServiceUrl, MainConfig.Password);
                LoginStatus returnCode = service.Connect();

                // if mule close ... end of game
                if (returnCode != LoginStatus.Logged)
                {
                    autoCloseDataTime = DateTime.Now.AddMinutes(30); // do control every 30 minutes
                    logger.Info("[AutoClose Mule] Login failed");
                    return;
                }
                logger.Info("[AutoClose Mule] Login ok");

                List<Ed2kfile> knownFiles = new List<Ed2kfile>();
                MainConfig.RssFeedList.ForEach(file => knownFiles.AddRange(file.GetDownloadedFiles()));

                logger.Info("[AutoClose Mule] Actual Downloads " + service.GetCurrentDownloads(knownFiles).Count);
                // if donwload > 0 ... there' s some download ... end
                if (service.GetCurrentDownloads(knownFiles).Count > 0)
                {
                    logger.Info("[AutoClose Mule] GetActualDownloads return >0");
                    autoCloseDataTime = DateTime.Now.AddMinutes(30);
                    logger.Info("[AutoClose Mule] LogOut");
                    service.Close();
                    return;
                }

                logger.Info("[AutoClose Mule] Show dialog ");
                // pop up form to advise user
                FormAlerteMuleClose dialog = new FormAlerteMuleClose();
                dialog.ShowDialog();

                logger.Info("[AutoClose Mule] Dialog return " + dialog.AlertChoice);
                switch (dialog.AlertChoice)
                {
                    case AlertChoiceEnum.Close:// Close
                        logger.Info("[AutoClose Mule: CLOSE] Close Service");
                        dialog.Dispose();
                        service.CloseEmuleApp();
                        service.Close();
                        timerAutoClose.Enabled = true;  // enable timer
                        break;
                    // to fix here
                    case AlertChoiceEnum.Skip: // SKIP
                        autoCloseDataTime = DateTime.Now.AddMinutes(30); // do controll every 30 minuts
                        logger.Info("[AutoClose Mule: SKIP] Skip");
                        logger.Info("[AutoClose Mule: SKIP] Next Tock " + autoCloseDataTime);
                        dialog.Dispose();
                        logger.Info("[AutoClose Mule] LogOut");
                        service.Close();
                        timerAutoClose.Enabled = true;  // enable timer
                        break;

                    case AlertChoiceEnum.Disable:    // disable autoclose
                        logger.Info("[AutoClose Mule: DISABLE] Disable");
                        dialog.Dispose();
                        logger.Info("[AutoClose Mule] LogOut");
                        service.Close();
                        DisableAutoCloseEmule();
                        timerAutoClose.Enabled = true;  // enable timer
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Autoclose error");
            }
        }

        public void DisableAutoCloseEmule()
        {
            menuItemAutoCloseEmule.Checked = false; // disable context menu
            autoCloseEMuleToolStripMenuItem.Checked = false; // File -> Menu -> Configure
            MainConfig.CloseEmuleIfAllIsDone = false; // disable function
            timerAutoClose.Enabled = false;
        }

        public void EnableAutoCloseEmule()
        {
            menuItemAutoCloseEmule.Checked = true;   // Enable trybar context menu
            autoCloseEMuleToolStripMenuItem.Checked = true; // File -> Menu -> Configure
            MainConfig.CloseEmuleIfAllIsDone = true; // Enable function
            autoCloseDataTime = DateTime.Now.AddMinutes(30);
            timerAutoClose.Enabled = true;
        }

       
        protected override void SetVisibleCore(bool value)
        {
            // MSDN http://social.msdn.microsoft.com/forums/en-US/csharpgeneral/thread/eab563c3-37d0-4ebd-a086-b9ea7bb03fed
            if (!mVisible)
                value = false;         // Prevent form getting visible
            base.SetVisibleCore(value);
        }

        /// <summary>
        /// wizard for add new feed
        /// </summary>
        private void AddRssChannel()
        {
            if ((MainConfig.TVUCookieH == string.Empty) | (MainConfig.TVUCookieI == string.Empty) | (MainConfig.TVUCookieT == string.Empty))
            {
                MessageBox.Show("Please login before add new RSS feed (File > Login)");
                return;
            }

            if (MainConfig.Password == string.Empty)
            {
                MessageBox.Show("Please configure eMule web interface (File > Option > Global Option > Network)");
            }

            List<string> currentRssUrlList = new List<string>();
            //
            //  Load Cookies from configuration
            //
            CookieContainer cookieContainer = new CookieContainer();
            Uri uriTvunderground = new Uri("http://tvunderground.org.ru/");
            cookieContainer.Add(uriTvunderground, new Cookie("h", MainConfig.TVUCookieH));
            cookieContainer.Add(uriTvunderground, new Cookie("i", MainConfig.TVUCookieI));
            cookieContainer.Add(uriTvunderground, new Cookie("t", MainConfig.TVUCookieT));

            //
            //  Get list of current feed URL
            //
            MainConfig.RssFeedList.ForEach(delegate (RssSubscription t) { currentRssUrlList.Add(t.Url); });

            //
            //  Open dialog 1 to path the URL
            //
            AddFeedDialogPage1 dialogPage1 = new AddFeedDialogPage1(currentRssUrlList);
            dialogPage1.ShowDialog();

            if (dialogPage1.DialogResult != DialogResult.OK)
            {
                dialogPage1.Dispose();
                return;
            }

            if (dialogPage1.RssUrlList.Count == 0)
            {
                MessageBox.Show("Nothing to downloads");
                return;
            }

            List<string> rssUrlList = dialogPage1.RssUrlList;
            bool fastAdd = dialogPage1.FastAdd;
            dialogPage1.Dispose();
            //
            //  Open dialog 2 to get all ed2k from feed
            //
            AddFeedDialogPage2 dialogPage2 = new AddFeedDialogPage2(rssUrlList, MainConfig.ServiceUrl, MainConfig.Password, cookieContainer, fastAdd);
            dialogPage2.ShowDialog();

            if (dialogPage2.DialogResult != DialogResult.OK)
            {
                dialogPage2.Dispose();
                return;
            }

            //
            //  check file count
            //
            if ((dialogPage2.rssSubscriptionsList.Count == 0) & (fastAdd == false))
            {
                MessageBox.Show("Nothing to downloads");
                dialogPage2.Dispose();
                return;
            }

            List<RssSubscription> rssSubscriptionsList = dialogPage2.rssSubscriptionsList;
            List<string> eMuleCategoryList = dialogPage2.ListCategory;
            dialogPage2.Dispose();  // free dialog
                                    // setup default

            foreach (RssSubscription rssSubscription in rssSubscriptionsList)
            {
                rssSubscription.Category = MainConfig.DefaultCategory;
                rssSubscription.PauseDownload = MainConfig.PauseDownloadDefault;
                rssSubscription.Enabled = true;
                rssSubscription.MaxSimultaneousDownload = MainConfig.MaxSimultaneousFeedDownloadsDefault;
            }

            if (fastAdd == false)
            {
                ////
                //  show Page 3 : choose single files to download.
                //
                AddFeedDialogPage3 dialogPage3 = new AddFeedDialogPage3(rssSubscriptionsList, eMuleCategoryList);
                dialogPage3.ShowDialog();
                if (dialogPage3.DialogResult != DialogResult.OK)
                {
                    dialogPage3.Dispose();
                    return;
                }
            }

            MainConfig.RssFeedList.AddRange(dialogPage2.rssSubscriptionsList);
            MainConfig.Save();

            UpdateRssFeedGUI();
            StartDownloadThread();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRssChannel();
        }

        /// <summary>
        /// Close the application
        /// </summary>
        private void ApplicationExit()
        {
            mVisible = true;
            Visible = true;
            allowClose = true;
            Close();
        }

        private void autoCloseEMuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainConfig.CloseEmuleIfAllIsDone == false)
            {
                EnableAutoCloseEmule();
            }
            else
            {
                DisableAutoCloseEmule();
            }
        }

        private void autoStartEMuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainConfig.StartEmuleIfClose == false)
            {
                MainConfig.StartEmuleIfClose = true;
                autoStartEMuleToolStripMenuItem.Checked = true;
            }
            else
            {
                MainConfig.StartEmuleIfClose = false;
                autoStartEMuleToolStripMenuItem.Checked = false;
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void cancelCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            worker.Abort();
        }

        /// <summary>
        /// Check Now menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartDownloadThread();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Clear();
        }

        private void dataGridViewMain_SelectionChanged(object sender, EventArgs e)
        {
            if ((dataGridViewMain.Rows.Count == 0) | (dataGridViewMain.SelectedRows.Count == 0))
            {
                labelFeedCategory.Text = string.Empty;
                labelFeedPauseDownload.Text = string.Empty;
                labelFeedUrl.Text = string.Empty;

                labelLastDownloadDate.Text = string.Empty;
                labelTotalFiles.Text = string.Empty;
                labelMaxSimultaneousDownloads.Text = string.Empty;
                listViewFeedFilesList.Items.Clear();
                return;
            }

            string titleCompact = dataGridViewMain.SelectedRows[0].Cells[DataGridViewTextBoxColumnTitle.Name].Value.ToString();
            RssSubscription feed = MainConfig.RssFeedList.Find(x => (x.TitleCompact == titleCompact));
            if (feed == null)
            {
                return;
            }

            labelFeedCategory.Text = feed.Category;
            labelFeedPauseDownload.Text = feed.PauseDownload.ToString();
            labelFeedUrl.Text = feed.Url;

            DateTime lastDownloadDate = feed.GetLastDownloadDate();
            if (lastDownloadDate == DateTime.MinValue)
            {
                labelLastDownloadDate.Text = "-";
            }
            else
            {
                labelLastDownloadDate.Text = lastDownloadDate.Date.ToString("yyyy-MM-dd");
            }
            labelTotalFiles.Text = feed.GetDownloadFileCount().ToString();
            labelMaxSimultaneousDownloads.Text = feed.MaxSimultaneousDownload.ToString();

            UpdateSubscriptionFilesList();
        }

        private void DeleteRssChannel()
        {
            if (dataGridViewMain.SelectedRows.Count == 0)
            {
                return;
            }

            // check user
            DialogResult rc;
            rc = MessageBox.Show("Confirm delete", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            if (rc != DialogResult.OK)
            {
                return;
            }

            foreach (DataGridViewRow selectedItem in dataGridViewMain.SelectedRows)
            {
                DataGridViewColumn col = DataGridViewTextBoxColumnTitle;
                string titleCompact = selectedItem.Cells[col.Name].Value.ToString();
                RssSubscription feed = MainConfig.RssFeedList.Find(x => (x.TitleCompact == titleCompact));

                if (feed == null)
                {
                    continue;
                }
                MainConfig.RssFeedList.Remove(feed);
            }
            MainConfig.Save();

            listViewFeedFilesList.Items.Clear();
            UpdateRssFeedGUI(); ///upgrade GUI
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteRssChannel();
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count == 0)
                return;

            if (listViewFeedFilesList.Items.Count == 0)
                return;

            if (listViewFeedFilesList.SelectedItems.Count == 0)
                return;

            // get the feed
            DataGridViewColumn col = DataGridViewTextBoxColumnTitle;
            string titleCompact = dataGridViewMain.SelectedRows[0].Cells[col.Name].Value.ToString();
            RssSubscription feed = MainConfig.RssFeedList.Find(x => (x.TitleCompact == titleCompact));

            if (feed == null)
            {
                return;
            }

            foreach (ListViewItem selectedItem in listViewFeedFilesList.SelectedItems)
            {
                string strSelectItemText = selectedItem.Text;   // this contain name file
                logger.Info("Marked as not Downloaded \"{0}\"", strSelectItemText);

                Ed2kfile file = feed.GetDownloadFile().Find(t => t.FileName == strSelectItemText);

                feed.SetFileNotDownloaded(file);
            }
            // finally update GUI
            UpdateSubscriptionFilesList();
        }

        private void disableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //foreach (ListViewItem selectedItem in listViewFeed.SelectedItems)
            //{
            //    string feedTitle = selectedItem.Text;

            //    RssSubscription Feed = null;

            //    Feed = MainConfig.RssFeedList.Find(delegate (RssSubscription t)
            //    {
            //        return t.TitleCompact.IndexOf(feedTitle) > -1;
            //    });

            //    if (Feed != null)
            //    {
            //        Feed.Enabled = false;

            //    }
            //}
            //MainConfig.Save();

            listViewFeedFilesList.Items.Clear();
            UpdateRssFeedGUI(); ///upgrade GUI
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count == 0)
            {
                return;
            }

            string titleCompact = dataGridViewMain.SelectedRows[0].Cells[0].Value.ToString();
            RssSubscription feed = MainConfig.RssFeedList.Find(x => (x.TitleCompact == titleCompact));

            if (feed == null)
            {
                return;
            }

            EditFeedForm dialog = new EditFeedForm(MainConfig, feed.Category, feed.PauseDownload, feed.Enabled, feed.MaxSimultaneousDownload);
            dialog.ShowDialog();

            if (dialog.DialogResult != DialogResult.OK)
            {
                dialog.Dispose();
                return;
            }

            feed.Enabled = dialog.feedEnable;
            feed.Category = dialog.Category;
            feed.PauseDownload = dialog.PauseDownload;
            feed.MaxSimultaneousDownload = dialog.maxSimultaneousDownload;

            MainConfig.Save();

            listViewFeedFilesList.Items.Clear();
            UpdateRssFeedGUI(); ///upgrade GUI
        }

        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listViewFeedFilesList.Items.Clear();
            UpdateRssFeedGUI(); ///upgrade GUI
        }

        private void excludeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var item in listBoxPending.SelectedItems)
            {
                logger.Info("selected item ", item);
            }
        }

        private void exitNotifyIconMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationExit();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationExit();
        }

        private void exportDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "XML (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    ExportImportHelper.Export(MainConfig, myStream);
                    // Code to write the stream goes here.
                    myStream.Close();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            logger.Info("FormClosing Event");
            logger.Debug("{0} = {1}", "CloseReason", e.CloseReason);
            logger.Debug("{0} = {1}", "Cancel", e.Cancel);
            logger.Debug("{0} = {1}", "mAllowClose", allowClose);

            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                allowClose = true;
            }

            // MSDN http://social.msdn.microsoft.com/forums/en-US/csharpgeneral/thread/eab563c3-37d0-4ebd-a086-b9ea7bb03fed
            if (!allowClose)
            {                   // Hide when user clicks X
                mVisible = false;
                Visible = false;
                e.Cancel = true;
            }
            else
            {
                notifyIcon1.Visible = false;      // Avoid ghost
                worker.Abort();
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //
            //  Setup Nlog
            //
            LoggingConfiguration config = LogManager.Configuration;
            if (config == null)
            {
                config = new LoggingConfiguration();
            }
            RichTextBoxTarget textBoxTarget = new RichTextBoxTarget();
            textBoxTarget.AutoScroll = true;
            textBoxTarget.ControlName = richTextBoxLog.Name;
            textBoxTarget.FormName = Name;
            textBoxTarget.MaxLines = 1024;
            textBoxTarget.Name = "TextBox";
            textBoxTarget.Layout = "${message} ${exception:format=tostring}";
            textBoxTarget.RowColoringRules.Add(new RichTextBoxRowColoringRule("level==LogLevel.Warn", "Empty", "Empty", FontStyle.Bold));
            textBoxTarget.RowColoringRules.Add(new RichTextBoxRowColoringRule("level==LogLevel.Error", "Red", "Empty", FontStyle.Bold));
            textBoxTarget.RowColoringRules.Add(new RichTextBoxRowColoringRule("level==LogLevel.Trace", "Gray", "Empty", FontStyle.Regular));
            config.AddTarget(textBoxTarget.Name, textBoxTarget);

            LoggingRule m_loggingRule = new LoggingRule("*", LogLevel.Info, textBoxTarget);
            //add before final rules from config
            config.LoggingRules.Insert(0, m_loggingRule);

            LogManager.Configuration = config;

            // download date time
            downloadDataTime = DateTime.Now.AddMinutes(MainConfig.IntervalTime);

            //auto close mule
            if (MainConfig.CloseEmuleIfAllIsDone)
            {
                EnableAutoCloseEmule();
            }
            else
            {
                DisableAutoCloseEmule();
            }

            labelFeedCategory.Text = "";
            labelFeedPauseDownload.Text = "";
            labelFeedUrl.Text = "";
            labelLastDownloadDate.Text = "";
            labelTotalFiles.Text = "";
            labelMaxSimultaneousDownloads.Text = "";

            UpdateRecentActivity();
            UpdateRssFeedGUI();
            UpdatePendingFiles();
#if !DEBUG
            autoStartEMuleToolStripMenuItem.Visible = false;
            autoCloseEMuleToolStripMenuItem.Visible = false;
#endif
            // attach textBox to the logger
        }

        private void forumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://sourceforge.net/tracker/?group_id=357576&atid=1492909");
        }

        private void globalOptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsDialog optDialog = new OptionsDialog(MainConfig);
            optDialog.ShowDialog();

            if (optDialog.DialogResult == DialogResult.OK)
            {
                MainConfig.ServiceType = optDialog.ServiceType;
                MainConfig.IntervalTime = optDialog.IntervalTime;
                MainConfig.StartMinimized = optDialog.StartMinimized;
                MainConfig.StartEmuleIfClose = optDialog.StartEmuleIfClose;
                MainConfig.CloseEmuleIfAllIsDone = optDialog.CloseEmuleIfAllIsDone;

                ConfigWindows.StartWithWindows = optDialog.StartWithWindows;

                MainConfig.Verbose = optDialog.Verbose;
                MainConfig.ServiceUrl = optDialog.ServiceUrl;
                MainConfig.Password = optDialog.Password;
                MainConfig.DefaultCategory = optDialog.DefaultCategory;
                MainConfig.eMuleExe = optDialog.eMuleExe;
                MainConfig.IntervalTime = optDialog.IntervalTime;
                MainConfig.MinToStartEmule = optDialog.MinToStartEmule;
                MainConfig.EmailNotification = optDialog.EmailNotification;
                
                MainConfig.AutoClearLog = optDialog.AutoClearLog;
                MainConfig.IntervalBetweenUpgradeCheck = optDialog.IntervalBetweenUpgradeCheck;
                MainConfig.MaxSimultaneousFeedDownloadsDefault = optDialog.MaxSimultaneousFeedDownloads;
                MainConfig.WebServerEnable = optDialog.WebServerEnable;
                MainConfig.WebServerPort = optDialog.WebServerPort;

                MainConfig.SmtpServerAddress = optDialog.SmtpServerAddress;
                MainConfig.SmtpServerPort = optDialog.SmtpServerPort;
                MainConfig.SmtpServerEnableSsl = optDialog.SmtpServerEnableSsl;
                MainConfig.SmtpServerEnableAuthentication = optDialog.SmtpServerEnableAuthentication;
                MainConfig.SmtpServerUserName = optDialog.SmtpServerUserName;
                MainConfig.SmtpServerPassword = optDialog.SmtpServerPassword;
                MainConfig.MailSender = optDialog.MailSender;
                MainConfig.MailReceiver = optDialog.MailReceiver;

                // tvu save cookie
                MainConfig.TVUCookieH = optDialog.tvuCookieH;
                MainConfig.TVUCookieI = optDialog.tvuCookieI;
                MainConfig.TVUCookieT = optDialog.tvuCookieT;

                if (MainConfig.CloseEmuleIfAllIsDone)
                {
                    EnableAutoCloseEmule();
                }
                else
                {
                    DisableAutoCloseEmule();
                }

                if (MainConfig.StartEmuleIfClose)
                {
                    autoStartEMuleToolStripMenuItem.Checked = true;
                }
                else
                {
                    autoStartEMuleToolStripMenuItem.Checked = false;
                }

                MainConfig.Save();
            }

            optDialog.Dispose();
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Process.Start("http://tvudownloader.sourceforge.net/");
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationShowHide();
        }

        private void importDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "XML (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ExportImportHelper.Import(MainConfig, openFileDialog1.FileName);
                MainConfig.Save();
                UpdateRecentActivity();
                UpdateRssFeedGUI();
                UpdatePendingFiles();
            }
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLogin form = new FormLogin();
            form.ShowDialog();

            if (form.DialogResult != DialogResult.OK)
            {
                return;
            }

            MainConfig.TVUCookieT = form.CookieT;
            MainConfig.TVUCookieI = form.CookieI;
            MainConfig.TVUCookieH = form.CookieH;
            MainConfig.Save();
        }

        private void markAsDownloadedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count == 0)
                return;

            if (listViewFeedFilesList.Items.Count == 0)
                return;

            if (listViewFeedFilesList.SelectedItems.Count == 0)
                return;

            // get the feed
            DataGridViewColumn col = DataGridViewTextBoxColumnTitle;
            string titleCompact = dataGridViewMain.SelectedRows[0].Cells[col.Name].Value.ToString();
            RssSubscription feed = MainConfig.RssFeedList.Find(x => (x.TitleCompact == titleCompact));

            if (feed == null)
            {
                return;
            }

            foreach (ListViewItem selectedItem in listViewFeedFilesList.SelectedItems)
            {
                string strSelectItemText = selectedItem.Text;   // this contain name file
                logger.Info("Marked as Downloaded file:\"{0}\"", strSelectItemText);

                Ed2kfile file = feed.GetDownloadFile().Find(t => t.FileName == strSelectItemText);

                feed.SetFileDownloaded(file);
            }
            // finally update GUI
            UpdateSubscriptionFilesList();
        }

        private void menu_AutoCloseEmule(object sender, EventArgs e)
        {
            if (MainConfig.CloseEmuleIfAllIsDone)
            {
                menuItemAutoCloseEmule.Checked = false;
                MainConfig.CloseEmuleIfAllIsDone = false;
            }
            else
            {
                menuItemAutoCloseEmule.Checked = true;
                MainConfig.CloseEmuleIfAllIsDone = true;
                autoCloseDataTime = DateTime.Now.AddMinutes(30); // do control every 30 minutes
            }
        }

        private void menu_AutoStartEmule(object sender, EventArgs e)
        {
            logger.Info("Auto Start Emule");
            if (MainConfig.StartEmuleIfClose)
            {
                menuItemAutoStartEmule.Checked = false;
                MainConfig.StartEmuleIfClose = false;
            }
            else
            {
                menuItemAutoStartEmule.Checked = true;
                MainConfig.StartEmuleIfClose = true;
            }
        }

        private void Menu_CheckNow(object sender, EventArgs e)
        {
            StartDownloadThread();
        }

        private void Menu_Enable(object sender, EventArgs e)
        {
            if (MainConfig.Enebled == false)
            {
                menuItemEnable.Checked = false;
                MainConfig.Enebled = false;
                timerRssCheck.Enabled = false;
            }
            else
            {
                menuItemEnable.Checked = true;
                MainConfig.Enebled = true;
                timerRssCheck.Enabled = true;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            // Show the form when the user double clicks on the notify icon.

            // Set the WindowState to normal if the form is minimized.
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;

            // Activate the form.
            Activate();
        }

        private void notifyIcon1_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Clicks == 2) & (e.Button == MouseButtons.Left))
            {
                if (Visible)
                {
                    mVisible = false;
                    Visible = false;
                }
                else
                {
                    mVisible = true;
                    Visible = true;
                }
            }
        }

        private void reportABugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://sourceforge.net/tracker/?group_id=357576&atid=1492909");
        }

        private void SetupNotify()
        {
            iconUp = Resources.appicon1;

            components = new Container();
            contextMenu1 = new ContextMenu();
            menuItemCheckNow = new MenuItem();
            menuItemExit = new MenuItem();
            menuItemAutoStartEmule = new MenuItem();
            menuItemAutoCloseEmule = new MenuItem();
            menuItemEnable = new MenuItem();

            // Initialize menuItem1
            menuItemAutoStartEmule.Index = 0;
            menuItemAutoStartEmule.Text = "Auto Start eMule";

            if (MainConfig.StartEmuleIfClose)
            {
                menuItemAutoStartEmule.Checked = true;
            }
            else
            {
                menuItemAutoStartEmule.Checked = false;
            }

            menuItemAutoStartEmule.Click += menu_AutoStartEmule;
            contextMenu1.MenuItems.Add(menuItemAutoStartEmule);

            menuItemAutoCloseEmule.Index = 1;
            menuItemAutoCloseEmule.Text = "Auto Close eMule";

            if (MainConfig.CloseEmuleIfAllIsDone)
            {
                EnableAutoCloseEmule();
            }
            else
            {
                DisableAutoCloseEmule();
            }

            if (MainConfig.StartEmuleIfClose)
            {
                autoStartEMuleToolStripMenuItem.Checked = true;
            }
            else
            {
                autoStartEMuleToolStripMenuItem.Checked = false;
            }

            menuItemAutoCloseEmule.Click += menu_AutoCloseEmule;
            contextMenu1.MenuItems.Add(menuItemAutoCloseEmule);

            menuItemExit.Index = 5;
            menuItemExit.Text = "E&xit";
            menuItemExit.Click += exitNotifyIconMenuItem_Click;
            contextMenu1.MenuItems.Add(menuItemExit);

            menuItemEnable.Index = 3;
            menuItemEnable.Text = "Enable";
            menuItemEnable.Checked = true;
            menuItemEnable.Click += Menu_Enable;
            contextMenu1.MenuItems.Add(menuItemEnable);

            menuItemCheckNow.Index = 2;
            menuItemCheckNow.Text = "Check Now";
            menuItemCheckNow.Click += Menu_CheckNow;
            contextMenu1.MenuItems.Add(menuItemCheckNow);

            // Create the NotifyIcon.
            notifyIcon1 = new NotifyIcon(components);
            notifyIcon1.MouseDown += notifyIcon1_MouseDown; // add event

            // The Icon property sets the icon that will appear
            // in the systray for this application.
            notifyIcon1.Icon = iconUp;

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyIcon1.ContextMenu = contextMenu1;

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon1.Text = "TV Underground Downloader";
            notifyIcon1.Visible = true;

            // Handle the DoubleClick event to activate the form.
            notifyIcon1.DoubleClick += notifyIcon1_DoubleClick;
        }

        private void StartDownloadThread()
        {
            if (worker.IsBusy)
            {
                logger.Info("Thread is busy");
                return;
            }

            if (MainConfig.AutoClearLog)
            {
                richTextBoxLog.Clear();
            }

            checkNowToolStripMenuItem.Enabled = false;
            deleteToolStripMenuItem.Enabled = false;
            addToolStripMenuItem.Enabled = false;
            toolStripMenuItemAdd.Enabled = false;
            toolStripMenuItemDelete.Enabled = false;
            toolStripMenuItemEdit.Enabled = false;

            toolStripButtonCheckNow.Enabled = false;
            toolStripButtonAddFeed.Enabled = false;

            menuItemCheckNow.Enabled = false;

            cancelCheckToolStripMenuItem.Enabled = true;
            toolStripButtonStop.Enabled = true;

            listBoxPending.Items.Clear();
            listBoxPending.Refresh();
            worker.Run();
            GoogleAnalyticsHelper.TrackEvent("BackgroundWorker_Start");
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GoogleAnalyticsHelper.TrackScreen("MainPage_" + tabControl1.SelectedTab.Text);
        }

        /// <summary>
        /// This is on the main thread, so we can update a TextBox or anything.
        /// </summary>
        private void Task_RunWorkerCompleted(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                UpdateRecentActivity();
                UpdatePendingFiles();
                UpdateRssFeedGUI();

                menuItemCheckNow.Enabled = true;
                deleteToolStripMenuItem.Enabled = true;
                addToolStripMenuItem.Enabled = true;
                checkNowToolStripMenuItem.Enabled = true;
                deleteToolStripMenuItem.Enabled = true;
                addToolStripMenuItem.Enabled = true;
                toolStripMenuItemAdd.Enabled = true;
                toolStripMenuItemDelete.Enabled = true;
                toolStripMenuItemEdit.Enabled = true;
                toolStripButtonCheckNow.Enabled = true;
                toolStripButtonAddFeed.Enabled = true;
                toolStripButtonStop.Enabled = false;
                cancelCheckToolStripMenuItem.Enabled = false;
            });

            GoogleAnalyticsHelper.TrackEvent("BackgroundWorker_Ended");
        }

        private void testAutoCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // remove 1h from AutoClose Data Time to force function to work
            TimeSpan delta = new TimeSpan(1, 0, 0);
            autoCloseDataTime = DateTime.Now.Subtract(delta);
            Autoclose();
        }

        private void testAutoStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(MainConfig.eMuleExe);
            }
            catch
            {
                logger.Info("Unable start application");
            }
        }

        private void timerAutoClose_Tick(object sender, EventArgs e)
        {
            Autoclose();
        }

        private void timerDelayStartup_Tick(object sender, EventArgs e)
        {
            logger.Info("Configuration loaded {0} ", MainConfig.FileNameConfig);

            timerDelayStartup.Enabled = false;

            // show web guide
        //    if (MainConfig.TotalDownloads == 0)
            {
                var rc = MessageBox.Show(
                    "Do you want to see a small video that can help you to configure the app?","Info", MessageBoxButtons.YesNo);
                if (rc == DialogResult.Yes)
                {
                    Process.Start("https://youtu.be/i_CiFij4qHg");
                }
            }


            if (MainConfig.StartMinimized)
            {
                mVisible = false;
                Visible = false;
            }

            StartDownloadThread();

            if ((MainConfig.WebServerEnable) && (embendedWebServer == null))
            {
                Task task = Task.Factory.StartNew(() =>
                {
                    embendedWebServer = new EmbendedWebServer();
                    embendedWebServer.Config = MainConfig;
                    embendedWebServer.Worker = worker;
                    embendedWebServer.Start();
                });
            }

            if (VersionChecker.CheckNewVersion(MainConfig, true))
            {
                MessageBox.Show("New version is available at http://tvudownloader.sourceforge.net/");
            }
        }

        private void timerRssCheck_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now > downloadDataTime)
            {
                if (MainConfig.AutoClearLog)
                {
                    richTextBoxLog.Clear();
                }

                downloadDataTime = DateTime.Now.AddMinutes(MainConfig.IntervalTime);

                logger.Info("Now : {0}", DateTime.Now);
                logger.Info("next tick : " + downloadDataTime);
                StartDownloadThread();
                UpdateRssFeedGUI();
            }
        }

        private void toolStripButtonAddFeed_Click(object sender, EventArgs e)
        {
            AddRssChannel();
        }

        private void toolStripButtonCheckNow_Click(object sender, EventArgs e)
        {
            StartDownloadThread();
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            worker.Abort();
        }

        private void toolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            AboutBox1 dialog = new AboutBox1();
            dialog.ShowDialog();
        }

        private void toolStripMenuItemAdd_Click(object sender, EventArgs e)
        {
            AddRssChannel();
        }

        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            DeleteRssChannel();
        }

        private void toolStripMenuItemUpdateStatus_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count == 0)
            {
                return;
            }

            //
            //  Load Cookies from configure
            //
            var cookieContainer = new CookieContainer();
            var uriTvunderground = new Uri("http://tvunderground.org.ru/");
            if (string.IsNullOrEmpty(MainConfig.TVUCookieH))
            {
                throw new LoginException("H");
            }
            cookieContainer.Add(uriTvunderground, new Cookie("h", MainConfig.TVUCookieH));

            if (string.IsNullOrEmpty(MainConfig.TVUCookieI))
            {
                throw new LoginException("I");
            }
            cookieContainer.Add(uriTvunderground, new Cookie("i", MainConfig.TVUCookieI));

            if (string.IsNullOrEmpty(MainConfig.TVUCookieT))
            {
                throw new LoginException("T");
            }
            cookieContainer.Add(uriTvunderground, new Cookie("t", MainConfig.TVUCookieT));

            foreach (DataGridViewRow selectedItem in dataGridViewMain.SelectedRows)
            {
                DataGridViewColumn col = DataGridViewTextBoxColumnTitle;
                string titleCompact = selectedItem.Cells[col.Name].Value.ToString();
                RssSubscription feed = MainConfig.RssFeedList.Find(x => (x.TitleCompact == titleCompact));

                if (feed != null)
                {
                    feed.UpdateTVUStatus(cookieContainer);
                }
            }
            MainConfig.Save();
            UpdateRssFeedGUI(); ///upgrade GUI
        }

        private void UpdatePendingFiles()
        {
            listBoxPending.Items.Clear();

            foreach (var subscrission in MainConfig.RssFeedList)
            {
                foreach (Ed2kfile file in subscrission.GetPendingFiles())
                {
                    listBoxPending.Items.Add(file.FileName);
                }
            }
        }

        private void UpdateRecentActivity()
        {
            DataTable table = new DataTable();
            table.Columns.Add("FileName");
            table.Columns.Add("LastUpdate");

            foreach (DownloadFile file in MainConfig.RssFeedList.GetLastActivity())
            {
                if (file.DownloadDate.HasValue == false)
                {
                    continue;
                }

                var newRow = table.NewRow();

                newRow["FileName"] = file.FileName;
                newRow["LastUpdate"] = file.DownloadDate;
                table.Rows.Add(newRow);
            }

            dataGridViewRecentActivity.DataSource = table;
        }

        private void UpdateRssFeedGUI()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Title", typeof(String));
            dataTable.Columns.Add("TotalDownloads", typeof(Int32));
            dataTable.Columns.Add("LastUpgrade", typeof(Int32));
            dataTable.Columns.Add("Status", typeof(String));
            dataTable.Columns.Add("Enabled", typeof(Boolean));
            dataTable.Columns.Add("DubLanguage", typeof(Image));

            foreach (RssSubscription subscrission in MainConfig.RssFeedList)
            {
                var newRow = dataTable.NewRow();
                newRow["Title"] = subscrission.TitleCompact;
                newRow["TotalDownloads"] = subscrission.TotalFilesDownloaded;

                newRow["LastUpgrade"] = subscrission.LastChannelUpdate.Days;

                switch (subscrission.CurrentTVUStatus)
                {
                    case TvuStatus.Complete:
                        newRow["Status"] = "Complete";
                        break;

                    case TvuStatus.StillIncomplete:
                        newRow["Status"] = "Incomplete";
                        break;

                    case TvuStatus.StillRunning:
                        newRow["Status"] = "Running";
                        break;

                    case TvuStatus.OnHiatus:
                        newRow["Status"] = "On Hiatus";
                        break;

                    case TvuStatus.Unknown:
                    default:
                        newRow["Status"] = "Unknown";
                        break;
                }

                newRow["Enabled"] = subscrission.Enabled ? "True" : "False";

                switch (subscrission.DubLanguage)
                {
                    case "gb":
                        newRow["DubLanguage"] = new Bitmap(Resources.gb);
                        break;

                    case "it":
                        newRow["DubLanguage"] = new Bitmap(Resources.it);
                        break;

                    case "fr":
                        newRow["DubLanguage"] = new Bitmap(Resources.fr);
                        break;

                    case "jp":
                        newRow["DubLanguage"] = new Bitmap(Resources.jp);
                        break;

                    case "es":
                        newRow["DubLanguage"] = new Bitmap(Resources.es);
                        break;

                    default:
                        break;
                }

                dataTable.Rows.Add(newRow);
            }
            dataGridViewMain.DataSource = dataTable;
        }

        private void UpdateSubscriptionFilesList()
        {
            if (dataGridViewMain.SelectedRows.Count == 0)
            {
                return;
            }

            DataGridViewRow selectedItem = dataGridViewMain.SelectedRows[0];
            if (selectedItem == null)
            {
                return;
            }

            DataGridViewColumn col = DataGridViewTextBoxColumnTitle;
            string titleCompact = selectedItem.Cells[col.Name].Value.ToString();
            RssSubscription feed = MainConfig.RssFeedList.Find(x => (x.TitleCompact == titleCompact));

            if (feed == null)
            {
                return;
            }

            // update list history
            listViewFeedFilesList.Items.Clear();

            // extract file by feedLink
            List<DownloadFile> ldf = feed.GetDownloadFile();
            var listFile = feed.GetDownloadFile();
            listFile.Sort((a, b) => b.FileName.CompareTo(a.FileName));
            foreach (DownloadFile file in listFile)
            {
                ListViewItem item = new ListViewItem(file.GetFileName());

                if (file.PublicationDate.HasValue)
                {
                    item.SubItems.Add(file.PublicationDate.Value.ToString("d", CultureInfo.InvariantCulture));
                }
                else
                {
                    item.SubItems.Add(string.Empty);
                }

                if (file.DownloadDate.HasValue)
                {
                    if (file.DownloadDate.Value == DateTime.MinValue)
                    {
                        item.SubItems.Add("Skipped");
                    }
                    else
                    {
                        string date = file.DownloadDate.Value.ToString("o", CultureInfo.InvariantCulture);
                        item.SubItems.Add(date);
                    }
                }
                else
                {
                    item.SubItems.Add(string.Empty);
                }

                listViewFeedFilesList.Items.Add(item);
            }
        }

        private void versionCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (VersionChecker.CheckNewVersion(MainConfig, true))
            {
                MessageBox.Show("New Version is available at http://tvudownloader.sourceforge.net/");
            }
            else
            {
                MessageBox.Show("Software is already update");
            }
        }
    }
}