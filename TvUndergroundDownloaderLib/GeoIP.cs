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
            var result = new GeoIP();
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

            var ipInfoXML = new XmlDocument();
            ipInfoXML.LoadXml(ipResponse);
            var responseXML = ipInfoXML.GetElementsByTagName("status");
            var statusXmlNode = responseXML[0];

            result.Status = statusXmlNode.InnerText;

            if (result.Status == "fail")
                return result;

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
            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);

            string buffer;

            using (var objStream = wrGETURL.GetResponse().GetResponseStream())
            {
                using (var objReader = new StreamReader(objStream))
                {
                    buffer = objReader.ReadToEnd();
                    objReader.Close();
                }
                objStream.Close();
            }

            return buffer;
        }
    }
}