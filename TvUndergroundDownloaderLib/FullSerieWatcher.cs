using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using TvUndergroundDownloaderLib.Interfaces;

namespace TvUndergroundDownloaderLib
{
    public class FullSerieWatcher : ISubscription
    {
        public FullSerieWatcher(string inUrl, string dubLanguage, string subLanguage, CookieContainer cookieContainer)
        {
            var regexPageSource = new Regex("http(s)?://(www\\.)?((tvunderground)|(tvu)).org.ru/index.php\\?show=season&sid=(?<sid>\\d{1,10})");

            Match match = regexPageSource.Match(inUrl);

            if (!match.Success)
            {
                throw new ApplicationException("Wrong Url");
            }

            Url = inUrl;

            int integerBuffer;
            if (int.TryParse(match.Groups["sid"].ToString(), out integerBuffer) == false)
            {
                throw new ApplicationException("Wrong URL");
            }

            SeridId = integerBuffer;
        }

        public string Category { get; set; }
        public TvuStatus CurrentTVUStatus { get; }
        public bool DeleteWhenCompleted { get; set; }
        public ReadOnlyCollection<DownloadFile> DownloadedFiles { get; }
        public string DubLanguage { get; }
        public bool Enabled { get; set; }
        public ReadOnlyCollection<DownloadFile> Files { get; }
        public int LastChannelUpdate { get; }
        public DateTime? LastDownload { get; }
        public DateTime LastSerieStatusUpgradeDate { get; }
        public DateTime LastUpdate { get; set; }
        public int MaxSimultaneousDownload { get; set; }
        public bool PauseDownload { get; set; }
        public int SeasonId { get; }
        public int SeridId { get; }
        public string Title { get; }
        public string TitleCompact { get; }
        public int TotalFilesDownloaded { get; }
        public string TVUStatus { get; }
        public string Url { get; private set; }

        private readonly List<MyClass> _list = new List<MyClass>();

        public DateTime GetLastDownloadDate()
        {
            throw new NotImplementedException();
        }

        public List<Ed2kfile> GetNewDownload(int maxSimultaneousDownload)
        {
            throw new NotImplementedException();
        }

        public List<DownloadFile> GetPendingFiles()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parse Html Page
        /// </summary>
        /// <param name="htmlPage"></param>
        public void ParseSeriePage(string htmlPage)
        {
            const string tableHeadMarker =
                @"<table align=""center"" class=""seriestable"" style=""table-layout: fixed;"">";

            const string tableCloseMarker = @"</table>";
            var currentPos = htmlPage.IndexOf(tableHeadMarker, StringComparison.Ordinal);
            if (currentPos == -1)
            {
                throw new WrongPageFormatException();
            }

            while (currentPos > -1)
            {
                var close = htmlPage.IndexOf(tableCloseMarker, currentPos, StringComparison.Ordinal);
                if (close == -1)
                {
                    throw new Exception("Missing close table");
                }

                string tableContent = htmlPage.Substring(currentPos, close - currentPos);

                ParseTableContent(tableContent);

                currentPos = currentPos + 1;
                currentPos = htmlPage.IndexOf(tableHeadMarker, currentPos, StringComparison.Ordinal);
            }
        }

        public Ed2kfile ProcessGuid(string url, CookieContainer cookieContainer)
        {
            throw new NotImplementedException();
        }

        public void SetFileDownloaded(Ed2kfile file)
        {
            throw new NotImplementedException();
        }

        public void SetFileNotDownloaded(Ed2kfile file)
        {
            throw new NotImplementedException();
        }

        public void Update(CookieContainer cookieContainer)
        {
            throw new NotImplementedException();
        }

        public void UpdateTVUStatus(CookieContainer cookieContainer)
        {
            throw new NotImplementedException();
        }

        public void Write(XmlTextWriter writer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parse Table Content
        /// </summary>
        /// <param name="tableContent"></param>
        private void ParseTableContent(string tableContent)
        {
            var regexFlag = new Regex("<img src=\"pic/flag-?.*.png\" alt=\"(?<alt>.*)\" title=\".*\" .*/>", RegexOptions.CultureInvariant);

            File.WriteAllText("table.html", tableContent);
            //
            //  read table head
            //
            const string headEndDelimeiter = "</tr>";

            var headerEndIndex = tableContent.IndexOf(headEndDelimeiter, StringComparison.InvariantCultureIgnoreCase);
            if (headerEndIndex == -1)
            {
                throw new WrongPageFormatException();
            }

            string headTable = tableContent.Substring(0, headerEndIndex);

            var match = regexFlag.Match(headTable);
            if (!match.Success)
            {
                throw new WrongPageFormatException();
            }

            var lenguage = match.Groups["alt"].ToString();
            var currentPos = -1;

            const string bodyDelimiter = "</tr>\r\n\t\t\t\t\t<tr>";
            currentPos = tableContent.IndexOf(bodyDelimiter, headerEndIndex, StringComparison.CurrentCultureIgnoreCase);

            while (currentPos > -1)
            {
                const string rowEndDelimiter = "</td></tr>";

                var rowEndIndex = tableContent.IndexOf(rowEndDelimiter, currentPos,
                    StringComparison.CurrentCultureIgnoreCase);

                if (rowEndIndex == -1)
                {
                    throw new WrongPageFormatException();
                }

                var row = tableContent.Substring(currentPos, rowEndIndex - currentPos);

                var outdata = ParseTableRow(row, lenguage);
                _list.Add(outdata);

                currentPos = tableContent.IndexOf(bodyDelimiter, currentPos + 1, StringComparison.CurrentCultureIgnoreCase);
            }

            XmlSerializer ser = new XmlSerializer(_list.GetType());
            TextWriter writer = new StreamWriter("out.xml");
            ser.Serialize(writer, _list);
            writer.Close();
        }

        /// <summary>
        /// parse table body
        /// </summary>
        /// <param name="row"></param>
        private MyClass ParseTableRow(string row, string lenguage)
        {
            var outRow = new MyClass { Lenguage = lenguage };

            //
            //  Parse Serie Id
            //
            var regexSerieId = new Regex("index.php\\?show=episodes&amp;sid=(?<serieid>\\d{2,10})");
            var match = regexSerieId.Match(row);
            if (!match.Success)
            {
                throw new WrongPageFormatException();
            }

            outRow.SerieId = int.Parse(match.Groups["serieid"].ToString());

            //
            //  Source type
            //
            StringBuilder sb = new StringBuilder(row);
            sb.Replace("\n", "");
            sb.Replace("\r", "");
            sb.Replace("\t", "");
            sb.Replace("</tr><tr>", "");
            sb.Replace("</td><td>", "\t");
            sb.Replace("<td>", "");

            var cells = sb.ToString().Split('\t');
            if (cells.Length != 7)
            {
                throw new WrongPageFormatException();
            }

            outRow.Source = cells[0].Trim();

            //
            //  Subtitle
            //
            var regexFlag = new Regex("<img src=\"pic/flag-?.*.png\" alt=\"(?<alt>.*)\" title=\".*\" .*/>", RegexOptions.CultureInvariant);
            var matchFlag = regexFlag.Match(row);
            if (matchFlag.Success)
            {
                outRow.Subtitle = matchFlag.Groups["alt"].ToString();
            }

            return outRow;
        }

        public class MyClass
        {
            public string Lenguage { set; get; }
            public int SerieId { get; set; }
            public string Source { get; set; }
            public string Subtitle { get; set; }
        }
    }
}