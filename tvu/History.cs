using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;


namespace tvu
{
    public class History
    {

        public History()
        {

        }


        public static void MigrateFromXMLToDB()
        {
            string FileName;
#if DEBUG
            FileName = "History.xml";
#else
            FileName = Application.LocalUserAppDataPath;
            int rc = FileName.LastIndexOf("\\");
            FileName = FileName.Substring(0, rc) + "\\History.xml";
#endif

            if (!File.Exists(FileName))
            {
                return;
            }

            SQLiteConnection.CreateFile(Config.FileNameDB);
            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();

                string sql = @"CREATE TABLE History (
                                uuid INTEGER PRIMARY KEY AUTOINCREMENT,
                                FileName TEXT,
                                FileSize INTEGER,
                                HashMD4 TEXT,
                                HashSHA1 TEXT,
                                Ed2kLink TEXT,
                                FeedLink TEXT,
                                FeedSource TEXT,
                                seasonID INTEGER,
                                episodeID INTEGER,
                                LastUpdate TEXT DEFAULT ('CURRENT_TIMESTAMP'));";
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();

                connection.Close();
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(FileName);

            XmlNodeList ItemList = xDoc.GetElementsByTagName("Item");

            List<string> stringCache = new List<string>();


            for (int i = 0; i < ItemList.Count; i++)
            {

                string strDate = DateTime.Now.ToString("s"); // to avoid compatibility with old history file
                string strFeedLink = "";
                string strEd2k = "";
                string strFeedSource = "";

                XmlNode node = ItemList[i];
                foreach (XmlNode t in node.ChildNodes)
                {

                    if (t.Name == "Ed2k")
                    {
                        strEd2k = t.InnerText;
                    }

                    if (t.Name == "FeedLink")
                    {
                        strFeedLink = t.InnerText;
                    }

                    if (t.Name == "FeedSource")
                    {

                        strFeedSource = t.InnerText;
                    }

                    if (t.Name == "Date")
                    {
                        strDate = t.InnerText;
                    }


                }

                History.Add(strEd2k, strFeedLink, strFeedSource, strDate);
            }
        }

        ///
        /// <summary>Add a element to list </summary>
        /// <param name='ed2k'>ED2K Link</param>
        /// <param name='FeedLink'>Link in Feed</param>
        /// <param name='FeedSource'>Rss Feed Link</param>
        ///
        public static void Add(string ed2k, string FeedLink, string FeedSource)
        {
            Add(ed2k, FeedLink, FeedSource, string.Empty);

        }
        ///
        /// <summary>Add a element to list </summary>
        /// <param name='ed2k'>ED2K Link</param>
        /// <param name='FeedLink'>Link in Feed</param>
        /// <param name='FeedSource'>Rss Feed Link</param>
        /// <param name="Date">Date</param>
        ///
        public static void Add(string ed2k, string FeedLink, string FeedSource, string Date)
        {
            fileHistory fh = new fileHistory(ed2k, FeedLink, FeedSource, Date);

            // http://tvunderground.org.ru/index.php?show=ed2k&season=73528&sid[815433]=1


            string seasonID = string.Empty, episodeID = string.Empty;

            Regex Pattern = new Regex(@"season.?(\d{3,6}).?sid.?(\d{3,6})");

            MatchCollection matchCollection = Pattern.Matches(FeedLink);
            
            if(matchCollection.Count > 0)
            {
                seasonID = matchCollection[0].Groups[1].ToString();
                episodeID = matchCollection[0].Groups[2].ToString();
            }

            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();
                const string sqlTemplate = @"INSERT INTO History (FileName , FileSize ,  HashMD4 , HashSHA1 , Ed2kLink , FeedLink , FeedSource, seasonID , episodeID , LastUpdate )
                                                VALUES
                                                (@FileName , @FileSize ,  @HashMD4 , @HashSHA1 , @Ed2kLink , @FeedLink , @FeedSource , @seasonID , @episodeID , @LastUpdate )";

                SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@FileName", fh.FileName));
                command.Parameters.Add(new SQLiteParameter("@FileSize", fh.FileSize));
                command.Parameters.Add(new SQLiteParameter("@HashMD4", fh.HashMD4));
                command.Parameters.Add(new SQLiteParameter("@HashSHA1", fh.HashSHA1));
                command.Parameters.Add(new SQLiteParameter("@Ed2kLink", fh.Ed2kLink));
                command.Parameters.Add(new SQLiteParameter("@FeedLink", fh.FeedLink));
                command.Parameters.Add(new SQLiteParameter("@FeedSource", fh.FeedSource));
                command.Parameters.Add(new SQLiteParameter("@seasonID", seasonID));
                command.Parameters.Add(new SQLiteParameter("@episodeID", episodeID));
                command.Parameters.Add(new SQLiteParameter("@LastUpdate", fh.Date));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }


        public bool FileExist(string FileName)
        {
            DataTable dt = new DataTable();
            dt.Reset();

            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();
                const string sqlTemplate = @"SELECT uuid FROM History WHERE FileName = @FileName";

                SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@FileName", FileName));
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);


                dataAdapter.Fill(dt);

                connection.Close();
            }

            return dt.Rows.Count > 0;
        }

        public bool FileExistByFeedLink(string feedlink)
        {
            DataTable dt = new DataTable();
            dt.Reset();

            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();
                const string sqlTemplate = @"SELECT uuid FROM History WHERE feedlink = @feedlink";

                SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@feedlink", feedlink));
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);


                dataAdapter.Fill(dt);

                connection.Close();
            }

            return dt.Rows.Count > 0;
        }

        public bool ExistInHistoryByEd2k(string Ed2kLink)
        {
            DataTable dt = new DataTable();
            dt.Reset();

            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();
                const string sqlTemplate = @"SELECT uuid FROM History WHERE Ed2kLink = @Ed2kLink";

                SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@Ed2kLink", Ed2kLink));
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);


                dataAdapter.Fill(dt);

                connection.Close();
            }

            return dt.Rows.Count > 0;
        }



        public string LastDownloadDateByFeedSource(string FeedSource)
        {
            DataTable dt = new DataTable();
            dt.Reset();

            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();
                const string sqlTemplate = @"SELECT MAX(History.LastUpdate) FROM History WHERE FeedSource = @FeedSource";

                SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@FeedSource", FeedSource));
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);


                dataAdapter.Fill(dt);

                connection.Close();
            }

            if (dt.Rows.Count == 0)
            {
                return string.Empty;
            }
            return dt.Rows[0][0].ToString();

        }

        /// <summary>
        /// Delete entry by fila name
        /// </summary>
        /// <param name="FileName"></param>
        public void DeleteFile(string FileName)
        {

            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();
                const string sqlTemplate = @"DELETE FROM History WHERE History.FileName = @FileName";

                SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@FileName", FileName));
                command.ExecuteNonQuery();
                connection.Close();
            }

        }
        /// <summary>
        /// Delete entry by Feed Source
        /// </summary>
        /// <param name="FeedSource"></param>
        public void DeleteFileByFeedSource(string FeedSource)
        {
            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();
                const string sqlTemplate = @"DELETE FROM History WHERE History.FeedSource = @FeedSource";

                SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@FeedSource", FeedSource));
                command.ExecuteNonQuery();
                connection.Close();
            }

        }

        public DataTable GetRecentActivity()
        {
            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3; UseUTF16Encoding=True;", Config.FileNameDB)))
            {
                DataTable table;


                connection.Open();
                const string sqlTemplate = @"SELECT  History.FileName, History.LastUpdate FROM History ORDER BY LastUpdate DESC LIMIT 32";

                SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);


                table = ds.Tables[0];
                connection.Close();

                return table;
            }

        }


    }
}
