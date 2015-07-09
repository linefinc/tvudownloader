using System.Data;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace TvUndergroundDownloader
{
    public class FeedLinkCacheRow
    {
        public string FeedLink;
        public string Ed2kLink;
        public string Date;
    }

    public class FeedLinkCache
    {

        public FeedLinkCache()
        {
        }


        public static void InitDB()
        {
            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();

                string sql = @"CREATE TABLE IF NOT EXISTS FeedLinkCache (
                                uuid INTEGER PRIMARY KEY AUTOINCREMENT,
                                Ed2kLink TEXT,
                                FeedLink TEXT,
                                seasonID INTEGER,
                                episodeID INTEGER,
                                LastUpdate TEXT DEFAULT ('CURRENT_TIMESTAMP'));";
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();

                connection.Close();
            }
            //
            //  Note: no data will be imported
            //
        }


     
        public static void AddFeedLink(string FeedLink, string Ed2kLink, string Date)
        {

            string seasonID = string.Empty;
            string episodeID = string.Empty;

            // http://tvunderground.org.ru/index.php?show=ed2k&season=73528&sid[815433]=1
            Regex Pattern = new Regex(@"season.?(\d{3,6}).?sid.?(\d{3,6})");

            MatchCollection matchCollection = Pattern.Matches(FeedLink);

            if (matchCollection.Count > 0)
            {
                seasonID = matchCollection[0].Groups[1].ToString();
                episodeID = matchCollection[0].Groups[2].ToString();
            }

            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();
                const string sqlTemplate = @"INSERT INTO FeedLinkCache ( Ed2kLink , FeedLink , seasonID , episodeID , LastUpdate )
                                                VALUES
                                                ( @Ed2kLink , @FeedLink  , @seasonID , @episodeID , @LastUpdate )";

                SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@Ed2kLink", Ed2kLink));
                command.Parameters.Add(new SQLiteParameter("@FeedLink", FeedLink));
                command.Parameters.Add(new SQLiteParameter("@seasonID", seasonID));
                command.Parameters.Add(new SQLiteParameter("@episodeID", episodeID));
                command.Parameters.Add(new SQLiteParameter("@LastUpdate", Date));
                command.ExecuteNonQuery();
                connection.Close();



            }

        }

        


        public string FindFeedLink(string FeedLink)
        {
            DataTable dt = new DataTable();
            dt.Reset();

            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();
                const string sqlTemplate = @"SELECT FeedLinkCache.Ed2kLink FROM FeedLinkCache WHERE FeedLinkCache.FeedLink = @FeedLink LIMIT 1;";

                SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@FeedLink", FeedLink));
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);


                dataAdapter.Fill(dt);

                connection.Close();
            }

            if(dt.Rows.Count == 0)
            {
                return string.Empty;
            }

            return dt.Rows[0][0].ToString();
  
        }

        public void DeleteFileByEd2kLink(string ed2kLink)
        {
            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();
                const string sqlTemplate = @"DELETE FROM FeedLinkCache WHERE FeedLinkCache.Ed2kLink = @Ed2kLink;";

                SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@Ed2kLink", ed2kLink));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void CleanUp()
        {
            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();
                const string sqlTemplate = @"DELETE FROM FeedLinkCache WHERE FeedLinkCache.FeedLink IN (SELECT History.FeedLink FROM History);";


                SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
                connection.Close();
            }

            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
            {
                connection.Open();
                const string sqlTemplate = @"DELETE FROM FeedLinkCache WHERE FeedLinkCache.LastUpdate < DATE('now','-15 days');";

                SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
                connection.Close();
            }
            
        }
       
    }
}
