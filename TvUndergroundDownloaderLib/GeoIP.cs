using System.IO;
using System.Net;
using System.Xml;

namespace TvUndergroundDownloaderLib
{
    internal class GeoIP
    {
        public string City;
        public string ClientIP;
        public string Country;
        public string CountryCode;
        public string Isp;
        public string Lat;
        public string Lon;
        public string Region;
        public string RegionName;
        public string Status;
        public string Zip;

        public static GeoIP GetCountryFromWeb(string ipAddress = "")
        {
            GeoIP result = new GeoIP();
            string ipResponse;
            try
            {
                ipResponse = GetFromWeb("http://ip-api.com/xml/" + ipAddress);
            }
            catch
            {
                return null;
            }

            result.ClientIP = ipAddress;

            XmlDocument ipInfoXML = new XmlDocument();
            ipInfoXML.LoadXml(ipResponse);
            XmlNodeList responseXML = ipInfoXML.GetElementsByTagName("status");
            XmlNode statusXmlNode = responseXML[0];

            result.Status = statusXmlNode.InnerText;

            if (result.Status == "fail")
            {
                return result;
            }

            result.Country = ipInfoXML.GetElementsByTagName("country")[0].InnerText;
            result.CountryCode = ipInfoXML.GetElementsByTagName("countryCode")[0].InnerText;
            result.Region = ipInfoXML.GetElementsByTagName("region")[0].InnerText;
            result.RegionName = ipInfoXML.GetElementsByTagName("regionName")[0].InnerText;
            result.City = ipInfoXML.GetElementsByTagName("city")[0].InnerText;
            result.Zip = ipInfoXML.GetElementsByTagName("zip")[0].InnerText;
            result.Lat = ipInfoXML.GetElementsByTagName("lat")[0].InnerText;
            result.Lon = ipInfoXML.GetElementsByTagName("lon")[0].InnerText;
            result.Isp = ipInfoXML.GetElementsByTagName("isp")[0].InnerText;

            return result;
        }

        private static string GetFromWeb(string sURL)
        {
            //const string sURL = "http://tvudownloader.sourceforge.net/statistics/data.csv";

            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);

            using (Stream objStream = wrGETURL.GetResponse().GetResponseStream())
            {
                using (StreamReader objReader = new StreamReader(objStream))
                {
                    return objReader.ReadToEnd();
                }
            }
        }
    }
}