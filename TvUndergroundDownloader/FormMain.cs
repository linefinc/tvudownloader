using NLog;
using NLog.Config;
using NLog.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog.LayoutRenderers;
using TvUndergroundDownloader.Extensions;
using TvUndergroundDownloader.Properties;
using TvUndergroundDownloaderLib;
using TvUndergroundDownloaderLib.EmbendedWebServer;

namespace TvUndergroundDownloader
{
    public partial class FormMain : Form
    {
        public ConfigWindows MainConfig;
        private readonly Dictionary<string, Image> _flagDictionary;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Worker _worker;
        private EmbendedWebServer _embendedWebServer;
        private bool allowClose;
        private DateTime autoCloseDataTime;
        private ContextMenu contextMenu1;
        private DateTime downloadDataTime;
        private Icon iconUp;
        private MenuItem menuItemAutoCloseEmule;
        private MenuItem menuItemAutoStartEmule;
        private MenuItem menuItemCheckNow;
        private MenuItem menuItemEnable;
        private MenuItem menuItemExit;
        private bool mVisible = true;
        private NotifyIcon notifyIcon1;
        public FormMain()
        {
            // load configuration
            MainConfig = new ConfigWindows();
            try
            {
                MainConfig.Load();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            InitializeComponent();
            SetupNotify();
            _worker = new Worker
            {
                Config = MainConfig
            };

            _worker.WorkerCompleted += Task_RunWorkerCompleted;


            string versionFull = ((AssemblyInformationalVersionAttribute)Assembly
            .GetAssembly(typeof(FormMain))
            .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false)[0]).InformationalVersion;
#if DEBUG
            Text += " - DEBUG - " + versionFull;
#else
            Text += " - " + versionFull;
#endif
            GoogleAnalyticsHelper.Cid = MainConfig.tvudwid;
            GoogleAnalyticsHelper.AppVersion = Config.Version;

            _flagDictionary = new Dictionary<string, Image>
            {
                {"gb", Properties.Resources.gb},
                {"it", Properties.Resources.it},
                {"de", Properties.Resources.de},
                {"fr", Properties.Resources.fr},
                {"jp", Properties.Resources.jp},
                {"es", Properties.Resources.es}
            };

            toolTip1.SetToolTip(checkBoxDeleteWhenCompleted, "Delete the feed when all downloads are completed\r\nand after 3 days");

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
                _logger.Info("[AutoClose Mule] MainConfig.CloseEmuleIfAllIsDone == false");
                return;
            }

            // check if Auto Close Data Time is not set
            if (autoCloseDataTime == DateTime.MinValue)
            {
                _logger.Info("[AutoClose Mule] AutoCloseDataTime = DateTime.Now.AddMinutes(30);");
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
                _logger.Info("[AutoClose Mule] Check Login");
                eMuleWebManager service = new eMuleWebManager(MainConfig.ServiceUrl, MainConfig.Password);
                LoginStatus returnCode = service.Connect();

                // if mule close ... end of game
                if (returnCode != LoginStatus.Logged)
                {
                    autoCloseDataTime = DateTime.Now.AddMinutes(30); // do control every 30 minutes
                    _logger.Info("[AutoClose Mule] Login failed");
                    return;
                }
                _logger.Info("[AutoClose Mule] Login ok");

                List<Ed2kfile> knownFiles = new List<Ed2kfile>();
                MainConfig.RssFeedList.ForEach(file => knownFiles.AddRange(file.DownloadedFiles));

                _logger.Info("[AutoClose Mule] Actual Downloads " + service.GetCurrentDownloads(knownFiles).Count);
                // if donwload > 0 ... there' s some download ... end
                if (service.GetCurrentDownloads(knownFiles).Count > 0)
                {
                    _logger.Info("[AutoClose Mule] GetActualDownloads return >0");
                    autoCloseDataTime = DateTime.Now.AddMinutes(30);
                    _logger.Info("[AutoClose Mule] LogOut");
                    service.Close();
                    return;
                }

                _logger.Info("[AutoClose Mule] Show dialog ");
                // pop up form to advise user
                FormAlerteMuleClose dialog = new FormAlerteMuleClose();
                dialog.ShowDialog();

                _logger.Info("[AutoClose Mule] Dialog return " + dialog.AlertChoice);
                switch (dialog.AlertChoice)
                {
                    case AlertChoiceEnum.Close:// Close
                        _logger.Info("[AutoClose Mule: CLOSE] Close Service");
                        dialog.Dispose();
                        service.CloseEmuleApp();
                        service.Close();
                        timerAutoClose.Enabled = true;  // enable timer
                        break;
                    // to fix here
                    case AlertChoiceEnum.Skip: // SKIP
                        autoCloseDataTime = DateTime.Now.AddMinutes(30); // do controll every 30 minuts
                        _logger.Info("[AutoClose Mule: SKIP] Skip");
                        _logger.Info("[AutoClose Mule: SKIP] Next Tock " + autoCloseDataTime);
                        dialog.Dispose();
                        _logger.Info("[AutoClose Mule] LogOut");
                        service.Close();
                        timerAutoClose.Enabled = true;  // enable timer
                        break;

                    case AlertChoiceEnum.Disable:    // disable autoclose
                        _logger.Info("[AutoClose Mule: DISABLE] Disable");
                        dialog.Dispose();
                        _logger.Info("[AutoClose Mule] LogOut");
                        service.Close();
                        DisableAutoCloseEmule();
                        timerAutoClose.Enabled = true;  // enable timer
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Autoclose error");
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
                rssSubscription.MaxSimultaneousDownload = (int)MainConfig.MaxSimultaneousFeedDownloadsDefault;
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

        private void cancelCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _worker.Abort();
        }

        private void checkBoxFeedPauseDownload_CheckedChanged(object sender, EventArgs e)
        {
            MainConfig.Save();
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


        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            MainConfig.Save();
        }

        private void dataGridViewMain_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == ColumnStatus.Index)
            {
                DataGridViewRow dataGridViewRowedItem = dataGridViewMain.Rows[e.RowIndex];
                var rssSubscription = dataGridViewRowedItem.DataBoundItem as RssSubscription;

                string text = "Error";
                if (rssSubscription != null)
                {
                    switch (rssSubscription.CurrentTVUStatus)
                    {
                        case TvuStatus.Complete:
                            text = "Complete";
                            break;
                        case TvuStatus.StillRunning:
                            text = "Still Running";
                            break;
                        case TvuStatus.Unknown:
                            text = "Unknown";
                            break;
                        case TvuStatus.StillIncomplete:
                            text = "Still Incomplete";
                            break;
                        case TvuStatus.OnHiatus:
                            text = "On Hiatus";
                            break;
                        default:
                            text = "Error";
                            break;
                    }
                }
                dataGridViewRowedItem.Cells[e.ColumnIndex].Value = text;
            }

            if (e.ColumnIndex == ColumnImageDub.Index)
            {
                DataGridViewRow dataGridViewRowedItem = dataGridViewMain.Rows[e.RowIndex];
                var rssSubscription = dataGridViewRowedItem.DataBoundItem as RssSubscription;

                if (rssSubscription == null)
                    return;

                if (!_flagDictionary.ContainsKey(rssSubscription.DubLanguage))
                    return;

                var image = _flagDictionary[rssSubscription.DubLanguage];
                if (dataGridViewRowedItem.Cells[e.ColumnIndex].Value != image)
                {
                    dataGridViewRowedItem.Cells[e.ColumnIndex].Value = image;
                }

            }
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
                var rssSubscription = selectedItem.DataBoundItem as RssSubscription;
                if (rssSubscription == null)
                    continue;
                MainConfig.RssFeedList.Remove(rssSubscription);
            }

            MainConfig.Save();

            UpdateRssFeedGUI(); ///upgrade GUI
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteRssChannel();
        }

        private void disableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count == 0)
            {
                return;
            }

            foreach (DataGridViewRow selectedItem in dataGridViewMain.SelectedRows)
            {
                var rssSubscription = selectedItem.DataBoundItem as RssSubscription;
                if (rssSubscription == null)
                    continue;
                rssSubscription.Enabled = false;
            }

            MainConfig.Save();
            UpdateRssFeedGUI(); ///upgrade GUI

        }

        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count == 0)
            {
                return;
            }

            foreach (DataGridViewRow selectedItem in dataGridViewMain.SelectedRows)
            {
                var rssSubscription = selectedItem.DataBoundItem as RssSubscription;
                if (rssSubscription == null)
                    continue;
                rssSubscription.Enabled = true;
            }

            MainConfig.Save();

            UpdateRssFeedGUI(); ///upgrade GUI
        }

        private void excludeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var item in listBoxPending.SelectedItems)
            {
                _logger.Info("selected item ", item);
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
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = @"XML (*.xml)|*.xml|All files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            using (Stream myStream = saveFileDialog1.OpenFile())
            {
                if (myStream == null)
                {
                    return;
                }
                ExportImportHelper.Export(MainConfig, myStream);
                myStream.Close();
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _logger.Info("FormClosing Event");
            _logger.Debug("{0} = {1}", "CloseReason", e.CloseReason);
            _logger.Debug("{0} = {1}", "Cancel", e.Cancel);
            _logger.Debug("{0} = {1}", "mAllowClose", allowClose);

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
                _worker.Abort();
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //
            //  Setup Nlog
            //

            #region Nlog config

            LoggingConfiguration nLogLoggingConfiguration = LogManager.Configuration;
            if (nLogLoggingConfiguration == null)
            {
                nLogLoggingConfiguration = new LoggingConfiguration();
            }

            RichTextBoxTarget textBoxTarget = new RichTextBoxTarget
            {
                AutoScroll = true,
                ControlName = richTextBoxLog.Name,
                FormName = Name,
                MaxLines = 1024,
                Name = "TextBox",
                Layout = "${message} ${exception:format=tostring}"
            };
            textBoxTarget.RowColoringRules.Add(new RichTextBoxRowColoringRule("level==LogLevel.Warn", "Empty", "Empty", FontStyle.Bold));
            textBoxTarget.RowColoringRules.Add(new RichTextBoxRowColoringRule("level==LogLevel.Error", "Red", "Empty", FontStyle.Bold));
            textBoxTarget.RowColoringRules.Add(new RichTextBoxRowColoringRule("level==LogLevel.Trace", "Gray", "Empty", FontStyle.Regular));
            nLogLoggingConfiguration.AddTarget(textBoxTarget.Name, textBoxTarget);

            LoggingRule nLogloggingRule = new LoggingRule("*", LogLevel.Info, textBoxTarget);
            //add before final rules from config
            nLogLoggingConfiguration.LoggingRules.Insert(0, nLogloggingRule);

            LogManager.Configuration = nLogLoggingConfiguration;

            #endregion

            // setup data binding
            rssSubscriptionListBindingSource.DataSource = new SortableBindingList<RssSubscription>(this.MainConfig.RssFeedList);

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
            FormOptions optDialog = new FormOptions(MainConfig);
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

                autoStartEMuleToolStripMenuItem.Checked = autoStartEMuleToolStripMenuItem.Checked;

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
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = @"XML (*.xml)|*.xml|All files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ExportImportHelper.Import(MainConfig, openFileDialog1.FileName);
                MainConfig.Save();
                UpdateRecentActivity();
                UpdateRssFeedGUI();
                UpdatePendingFiles();
            }
        }

        private void linkLabelFeedLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count != 1)
            {
                return;
            }


            var rssSubscription = dataGridViewMain.SelectedRows[0].DataBoundItem as RssSubscription;
            if (rssSubscription == null)
                return;
            Process.Start(rssSubscription.Url);
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
            _logger.Info("Auto Start Emule");
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
            if (_worker.IsBusy)
            {
                _logger.Info("Thread is busy");
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

            toolStripButtonCheckNow.Enabled = false;
            toolStripButtonAddFeed.Enabled = false;

            menuItemCheckNow.Enabled = false;

            cancelCheckToolStripMenuItem.Enabled = true;
            toolStripButtonStop.Enabled = true;

            listBoxPending.Items.Clear();
            listBoxPending.Refresh();
            _worker.Run();
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
                _logger.Info("Unable start application");
            }
        }

        private void timerAutoClose_Tick(object sender, EventArgs e)
        {
            Autoclose();
        }

        private void timerDelayStartup_Tick(object sender, EventArgs e)
        {
            _logger.Info("Configuration loaded {0} ", MainConfig.FileNameConfig);

            timerDelayStartup.Enabled = false;

            // show web guide
            if (MainConfig.IsFirstStart())
            {
                FormFirstTimeWizad wizad = new FormFirstTimeWizad();
                wizad.ShowDialog();
                if (wizad.DialogResult == DialogResult.OK)
                {
                    MainConfig.ServiceType = wizad.ServiceType;
                    MainConfig.Password = wizad.Password;
                    MainConfig.TVUCookieH = wizad.TVUCookieH;
                    MainConfig.TVUCookieI = wizad.TVUCookieI;
                    MainConfig.TVUCookieT = wizad.TVUCookieT;
                    MainConfig.Save();
                }
            }

            if (MainConfig.StartMinimized)
            {
                mVisible = false;
                Visible = false;
            }

            StartDownloadThread();

            if ((MainConfig.WebServerEnable) && (_embendedWebServer == null))
            {
                Task.Factory.StartNew(() =>
                {
                    _embendedWebServer = new EmbendedWebServer
                    {
                        Config = MainConfig,
                        Worker = _worker
                    };
                    _embendedWebServer.Start();
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

                _logger.Info("Now : {0}", DateTime.Now);
                _logger.Info("next tick : " + downloadDataTime);
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
            _worker.Abort();
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

        private void toolStripMenuItemMarkAsDownload_Click(object sender, EventArgs e)
        {
            if (dataGridViewFeedFiles.SelectedRows.Count == 0)
            {
                return;
            }

            foreach (DataGridViewRow selectedItem in dataGridViewFeedFiles.SelectedRows)
            {
                var downloadFile = selectedItem.DataBoundItem as DownloadFile;
                if (downloadFile == null)
                {
                    continue;
                }

                var subscription = downloadFile.Subscription;
                if (subscription == null)
                {
                    continue;
                }
                subscription.SetFileDownloaded(downloadFile);
            }

            MainConfig.Save();
            dataGridViewFeedFiles.Refresh();
            dataGridViewFeedFiles.Update();
        }

        private void toolStripMenuItemRedownload_Click(object sender, EventArgs e)
        {
            if (dataGridViewFeedFiles.SelectedRows.Count == 0)
            {
                return;
            }

            foreach (DataGridViewRow selectedItem in dataGridViewFeedFiles.SelectedRows)
            {
                var downloadFile = selectedItem.DataBoundItem as DownloadFile;
                if (downloadFile == null)
                {
                    continue;
                }

                var subscription = downloadFile.Subscription;
                if (subscription == null)
                {
                    continue;
                }
                subscription.SetFileNotDownloaded(downloadFile);
            }

            MainConfig.Save();
            dataGridViewFeedFiles.Refresh();
            dataGridViewFeedFiles.Update();

        }

        private void toolStripMenuItemRunFirstTimeWizard_Click(object sender, EventArgs e)
        {
            FormFirstTimeWizad wizad = new FormFirstTimeWizad();
            wizad.Password = MainConfig.Password;
            wizad.ServiceType = MainConfig.ServiceType;
            wizad.TVUCookieH = MainConfig.TVUCookieH;
            wizad.TVUCookieI = MainConfig.TVUCookieI;
            wizad.TVUCookieT = MainConfig.TVUCookieT;

            wizad.ShowDialog();
            if (wizad.DialogResult != DialogResult.OK)
            {
                return;
            }

            MainConfig.ServiceType = wizad.ServiceType;
            MainConfig.Password = wizad.Password;
            MainConfig.TVUCookieH = wizad.TVUCookieH;
            MainConfig.TVUCookieI = wizad.TVUCookieI;
            MainConfig.TVUCookieT = wizad.TVUCookieT;
            MainConfig.Save();


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
                //DataGridViewColumn col = DataGridViewTextBoxColumnTitle;
                //string titleCompact = selectedItem.Cells[col.Name].Value.ToString();
                //RssSubscription feed = MainConfig.RssFeedList.Find(x => (x.TitleCompact == titleCompact));

                //if (feed != null)
                //{
                //    feed.UpdateTVUStatus(cookieContainer);
                //}
            }
            MainConfig.Save();
            UpdateRssFeedGUI(); ///upgrade GUI
        }

        private void UpdatePendingFiles()
        {
            listBoxPending.Items.Clear();

            foreach (var subscrission in MainConfig.RssFeedList)
            {
                foreach (DownloadFile file in subscrission.GetPendingFiles())
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
            rssSubscriptionListBindingSource.DataSource = new SortableBindingList<RssSubscription>();
            rssSubscriptionListBindingSource.DataSource = new SortableBindingList<RssSubscription>(this.MainConfig.RssFeedList);
            dataGridViewMain.Refresh();
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