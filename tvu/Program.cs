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

            // copy file from old position to new position
            try
            {
                if (!File.Exists(Config.FileNameConfig))
                {
                    // generate old path
                    string basePath = Environment.GetEnvironmentVariable("LocalAppData");

                    // check if old config exist
                    if (File.Exists(basePath + @"\tvu\tvu\config.xml"))
                    {
                        // copy file
                        File.Copy(basePath + @"\tvu\tvu\config.xml", basePath + @"\TvUndergroundDownloader\config.xml");
                        File.Copy(basePath + @"\tvu\tvu\History.xml", basePath + @"\TvUndergroundDownloader\History.xml");
                        File.Copy(basePath + @"\tvu\tvu\log.txt", basePath + @"\TvUndergroundDownloader\log.txt");
                    }

                }
            }
            catch
            {

            }



            // create db if not exit
            if (File.Exists(Config.FileNameDB) == false)
            {
                SQLiteConnection.CreateFile(Config.FileNameDB);

                History.InitDB();
                FeedLinkCache.InitDB();
                if (File.Exists(Config.FileNameHistory))
                {
                    History.MigrateFromXMLToDB();
                    File.Move(Config.FileNameHistory, Config.FileNameHistory + ".old");
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }


    }
}
