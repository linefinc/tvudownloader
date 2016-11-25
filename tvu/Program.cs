using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace TvUndergroundDownloader
{
    static class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Process aProcess = Process.GetCurrentProcess();
            string aProcName = aProcess.ProcessName;

            if (Process.GetProcessesByName(aProcName).Length > 1)
            {
                MessageBox.Show("Application just started");
                Application.ExitThread();
                return;
            }


            //
            //  Setup Nlog
            //
            LoggingConfiguration config = LogManager.Configuration;
            if (config == null)
            {
                config = new LoggingConfiguration();
            }
           
            FileTarget fileTarget = new FileTarget();
            fileTarget.Name = "logfile";
            fileTarget.FileName = Config.FileNameLog;
            config.AddTarget(fileTarget.Name, fileTarget);
            LoggingRule m_loggingRule = new LoggingRule("*", LogLevel.Info, fileTarget);
            config.LoggingRules.Insert(0, m_loggingRule);
            LogManager.Configuration = config;

            //
            //
            //

            //
            //  try to restore old configuration from previous folder
            //
            #region update old folder
            if (!File.Exists(Config.FileNameConfig))
            {
                logger.Warn("Migration old configuration file");
                // generate old path
                //
                // old path
                //      C:\Users\User\AppData\Local\tvu\tvu\config.xml
                // new path
                //      C:\Users\User\AppData\Local\TvUndergroundDownloader\config.xml
                //
                string oldFileNameConfig = Config.FileNameConfig.Replace(@"TvUndergroundDownloader\config.xml", @"tvu\tvu\config.xml");

                // check if old configuration exist
                if (File.Exists(oldFileNameConfig))
                {
                    logger.Info("config.xml founded");
                    File.Copy(oldFileNameConfig, Config.FileNameConfig);
                    File.Copy(oldFileNameConfig, Config.FileNameConfig + ".old");
                }
              
            }
            #endregion

            #region initialize db
            // create db if not exit
            if (File.Exists(Config.FileNameDB) == false)
            {
                logger.Info("Initialize new database");
                SQLiteConnection.CreateFile(Config.FileNameDB);

                History.InitDB();
                FeedLinkCache.InitDB();

                // migrate History.xml only for backup because it's replace by sqlite
                //
                // old path
                //      C:\Users\User\AppData\Local\tvu\tvu\History.xml
                // new path, not yet used 
                //      C:\Users\User\AppData\Local\TvUndergroundDownloader\History.xml
                //

                // try to load history from old storage position
                string oldHistoryFile = Config.FileNameHistory;
                oldHistoryFile = oldHistoryFile.Replace(@"TvUndergroundDownloader\History.xml", @"tvu\tvu\History.xml");

                if (File.Exists(oldHistoryFile))
                {
                    bool rc = History.MigrateFromXMLToDB(oldHistoryFile);
                    if (rc == false)
                    {
                        MessageBox.Show("An error occurred during migration to new data system");
                    }
                }


            }
            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

    }
}
