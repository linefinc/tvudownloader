using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Data.SQLite;

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
            //  try to resotre old config from previus foulder
            //
            #region update old folder
            if (!File.Exists(Config.FileNameConfig))
            {
                Log.logInfo("Migration old config file");
                // generate old path
                string basePath = Application.LocalUserAppDataPath;
                basePath = Directory.GetParent(basePath).FullName;
                basePath = Directory.GetParent(basePath).FullName;
                basePath = Directory.GetParent(basePath).FullName;
                //
                // old path
                //      C:\Users\User\AppData\Local\tvu\tvu\config.xml
                // new path
                //      C:\Users\User\AppData\Local\TvUndergroundDownloader\TvUndergroundDownloader\config.xml
                //

                // check if old config exist

                if (File.Exists(basePath + @"\tvu\tvu\config.xml"))
                {
                    Log.logInfo("config.xml founded");
                    File.Copy(basePath + @"\tvu\tvu\config.xml", Config.FileNameConfig);
                    File.Copy(basePath + @"\tvu\tvu\config.xml", Config.FileNameConfig + ".old");
                }


                if (File.Exists(basePath + @"\tvu\tvu\History.xml"))
                {
                    Log.logInfo("History.xml founded");
                    File.Copy(basePath + @"\tvu\tvu\History.xml", Config.FileNameHistory);
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
                if (File.Exists(Config.FileNameHistory))
                {
                    History.MigrateFromXMLToDB();
                    File.Move(Config.FileNameHistory, Config.FileNameHistory + ".old");
                }
            }
            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }


    }
}
