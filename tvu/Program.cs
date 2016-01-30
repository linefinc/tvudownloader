using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace TvUndergroundDownloader
{
    static class Program
    {
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
            // enable file logging
            //
            Log.Instance.AddLogTarget(new LogTargetFile(Config.FileNameLog));


            //
            //  try to restore old configuration from previous folder
            //
            #region update old folder
            if (!File.Exists(Config.FileNameConfig))
            {
                Log.logInfo("Migration old configuration file");
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
                    Log.logInfo("config.xml founded");
                    File.Copy(oldFileNameConfig, Config.FileNameConfig);
                    File.Copy(oldFileNameConfig, Config.FileNameConfig + ".old");
                }

                // migrate History.xml only for backup because it's replace by sqlite
                //
                // old path
                //      C:\Users\User\AppData\Local\tvu\tvu\History.xml
                // new path
                //      C:\Users\User\AppData\Local\TvUndergroundDownloader\History.xml.old
                //
                string oldFileNameHistroy = Config.FileNameConfig.Replace(@"TvUndergroundDownloader\History.xml.old", @"tvu\tvu\History.xml");

                if (File.Exists(oldFileNameHistroy))
                {
                    Log.logInfo("History.xml founded");
                    File.Copy(oldFileNameHistroy, Config.FileNameHistory + ".old");
                }
            }
            #endregion

            #region initialize db
            // create db if not exit
            if (File.Exists(Config.FileNameDB) == false)
            {
                Log.logInfo("Initialize new database");
                SQLiteConnection.CreateFile(Config.FileNameDB);

                History.InitDB();
                FeedLinkCache.InitDB();
                if (File.Exists(Config.FileNameHistory + ".old"))
                {
                    // try to load history backup copy
                    bool rc = History.MigrateFromXMLToDB(Config.FileNameHistory + ".old");
                    if (rc == false)
                    {
                        // try to load history from old storage position
                        string oldHistoryFile = Config.FileNameHistory.Replace(@"TvUndergroundDownloader\History.xml", @"tvu\tvu\History.xml");

                        if (File.Exists(oldHistoryFile))
                        {
                            rc = History.MigrateFromXMLToDB(oldHistoryFile);
                            if (rc == false)
                            {
                                MessageBox.Show("An error occurred during migration to new data system");
                            }
                        }

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
