using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Security;
using System.Text.RegularExpressions;
using System.Xml;


namespace TvUndergroundDownloader
{
    public class History
    {

        public History()
        {

        }

        public static void InitDB()
        {
            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();

                string sql = @"CREATE TABLE IF NOT EXISTS History (
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
            }


            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();

                string sql = @"CREATE TABLE IF NOT EXISTS GlobalHistory (
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
            }
        }

        public static void MigrateFromXMLToDB()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Config.FileNameHistory);

            XmlNodeList ItemList = xDoc.GetElementsByTagName("Item");

            List<string> stringCache = new List<string>();

            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {

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
                        History.Add(transaction, strEd2k, strFeedLink, strFeedSource, strDate);
                    }
                    transaction.Commit();
                }
            }
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

            // Static Regex "https?://(www\.)?tvunderground.org.ru/index.php\?show=ed2k&season=(\d{1,10})&sid\[(\d{1,10})\]=\d{1,10}"
            MatchCollection matchCollection = fileHistory.regexFeedLink.Matches(FeedLink);


            seasonID = matchCollection[0].Groups["season"].ToString();
            episodeID = matchCollection[0].Groups["sid"].ToString();


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
            }
        }

        ///
        /// <summary>Add a element to list </summary>
        /// <param name='ed2k'>ED2K Link</param>
        /// <param name='FeedLink'>Link in Feed</param>
        /// <param name='FeedSource'>Rss Feed Link</param>
        /// <param name="Date">Date</param>
        /// <param name="SQLiteTransaction">transaction</param>
        ///
        private static void Add(SQLiteTransaction transaction, string ed2k, string FeedLink, string FeedSource, string Date)
        {
            fileHistory fh = new fileHistory(ed2k, FeedLink, FeedSource, Date);

            // http://tvunderground.org.ru/index.php?show=ed2k&season=73528&sid[815433]=1


            string seasonID = string.Empty, episodeID = string.Empty;
            // Static Regex "https?://(www\.)?tvunderground.org.ru/index.php\?show=ed2k&season=(\d{1,10})&sid\[(\d{1,10})\]=\d{1,10}"
            MatchCollection matchCollection = fileHistory.regexFeedLink.Matches(FeedLink);

            if (matchCollection.Count > 0)
            {
                seasonID = matchCollection[0].Groups[2].ToString();
                episodeID = matchCollection[0].Groups[3].ToString();
            }


            const string sqlTemplate = @"INSERT INTO History (FileName , FileSize ,  HashMD4 , HashSHA1 , Ed2kLink , FeedLink , FeedSource, seasonID , episodeID , LastUpdate )
                                                VALUES
                                                (@FileName , @FileSize ,  @HashMD4 , @HashSHA1 , @Ed2kLink , @FeedLink , @FeedSource , @seasonID , @episodeID , @LastUpdate )";

            SQLiteCommand command = new SQLiteCommand(sqlTemplate);
            command.CommandType = CommandType.Text;
            command.Transaction = transaction;
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

        }


        public bool FileExist(Ed2kfile file)
        {
            DataTable dt = new DataTable();
            dt.Reset();

            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();
                const string sqlTemplate = @"SELECT uuid, FeedLink, FeedSource, LastUpdate 
                                                                    FROM History 
                                                             WHERE 
                                                                    History.HashMD4 = @HashMD4 AND 
                                                                    History.FileSize = @FileSize AND
                                                                    ((History.HashSHA1 = "") OR (@HashSHA1 = "" ) OR (History.HashSHA1 = @HashSHA1));";
                SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@FileSize", file.FileSize));
                command.Parameters.Add(new SQLiteParameter("@HashMD4", file.HashMD4));
                command.Parameters.Add(new SQLiteParameter("@HashSHA1", file.HashSHA1));
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);
                dataAdapter.Fill(dt);
            }

            return dt.Rows.Count > 0;
        }

        public fileHistory getFileHistoryFromDB(Ed2kfile file)
        {
            DataTable dt = new DataTable();
            dt.Reset();

            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();
                const string sqlTemplate = @"SELECT uuid, FeedLink, FeedSource, LastUpdate 
                                                    FROM History 
                                             WHERE 
                                                    History.HashMD4 = @HashMD4 AND 
                                                    History.FileSize = @FileSize;";

                SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@FileSize", file.FileSize));
                command.Parameters.Add(new SQLiteParameter("@HashMD4", file.HashMD4));
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);
                dataAdapter.Fill(dt);
            }

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dt.Rows[0];
            return new fileHistory(file, row["FeedLink"].ToString(),
                                         row["FeedSource"].ToString(),
                                         row["LastUpdate"].ToString());
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
        public void DeleteFile(Ed2kfile file)
        {
            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();
                const string sqlTemplate = @"DELETE FROM History WHERE 
                                                    History.HashMD4 = @HashMD4 AND 
                                                    History.FileSize = @FileSize AND
                                                    ((History.HashSHA1 = "") OR (@HashSHA1 = "" ) OR (History.HashSHA1 = @HashSHA1));";

                SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@FileSize", file.FileSize));
                command.Parameters.Add(new SQLiteParameter("@HashMD4", file.HashMD4));
                command.Parameters.Add(new SQLiteParameter("@HashSHA1", file.HashSHA1));
                command.ExecuteNonQuery();
            }

        }

        /// <summary>
        /// Delete entry by fila name
        /// </summary>
        /// <param name="FileName"></param>
        [Obsolete]
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

                return table;
            }

        }


        public DataTable GetDownloadedFileByFeedSoruce(string FeedSource)
        {
            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3; UseUTF16Encoding=True;", Config.FileNameDB)))
            {
                DataTable table;

                connection.Open();
                const string sqlTemplate = @"SELECT  History.FileName, History.LastUpdate FROM History WHERE FeedSource = @FeedSource ORDER BY LastUpdate DESC ";

                SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                command.Parameters.Add(new SQLiteParameter("@FeedSource", FeedSource));
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);

                table = ds.Tables[0];
                return table;
            }
        }

        public int GetDownloadedFileCountByFeedSoruce(string FeedSoruce)
        {
            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3; UseUTF16Encoding=True;", Config.FileNameDB)))
            {
                connection.Open();
                const string sqlTemplate = @"SELECT  count(*) FROM History WHERE History.FeedSource = @FeedSource ";

                SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                command.Parameters.Add(new SQLiteParameter("@FeedSource", FeedSoruce));
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataTable table = new DataTable();
                table.Reset();
                dataAdapter.Fill(table);

                if (table.Rows.Count == 0)
                {
                    return -1;
                }

                int count;
                if (int.TryParse(table.Rows[0][0].ToString(), out count) == false)
                {
                    return -1;
                }

                return count;
            }

        }
        //
        //legacy function to maintain back compatibility
        //
        public void Save()
        {
            DataTable dt = new DataTable();
            dt.Reset();

            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();
                const string sqlTemplate = @"SELECT Ed2kLink , FeedLink, FeedSource , LastUpdate FROM History ORDER BY History.LastUpdate ASC;";


                SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                command.CommandType = CommandType.Text;

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);
                dataAdapter.Fill(dt);
            }

            XmlTextWriter textWritter = new XmlTextWriter(Config.FileNameHistory, null);
            textWritter.Formatting = Formatting.Indented;
            textWritter.Indentation = 4;
            textWritter.WriteStartDocument();
            textWritter.WriteStartElement("History");

            foreach (DataRow row in dt.Rows)
            {
                textWritter.WriteStartElement("Item");// open Item
                textWritter.WriteElementString("Ed2k", SecurityElement.Escape(row["Ed2kLink"].ToString()));
                textWritter.WriteElementString("FeedLink", SecurityElement.Escape(row["FeedLink"].ToString()));
                textWritter.WriteElementString("FeedSource", SecurityElement.Escape(row["FeedSource"].ToString()));
                textWritter.WriteElementString("Date", row["LastUpdate"].ToString());
                textWritter.WriteEndElement(); // close Item
            }

            textWritter.Close();

        }


    }
}
