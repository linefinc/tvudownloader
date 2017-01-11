using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace TvUndergroundDownloader
{
    class GeoIP
    {
        public string ClientIP;
        public string status;
        public string country;
        public string countryCode;
        public string region;
        public string regionName;
        public string city;
        public string zip;
        public string lat;
        public string lon;
        public string timezone;
        public string isp;

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

            result.status = statusXmlNode.InnerText;

            if (result.status == "fail")
            {
                return result;
            }

            result.country = ipInfoXML.GetElementsByTagName("country")[0].InnerText;
            result.countryCode = ipInfoXML.GetElementsByTagName("countryCode")[0].InnerText;
            result.region = ipInfoXML.GetElementsByTagName("region")[0].InnerText;
            result.regionName = ipInfoXML.GetElementsByTagName("regionName")[0].InnerText;
            result.city = ipInfoXML.GetElementsByTagName("city")[0].InnerText;
            result.zip = ipInfoXML.GetElementsByTagName("zip")[0].InnerText;
            result.lat = ipInfoXML.GetElementsByTagName("lat")[0].InnerText;
            result.lon = ipInfoXML.GetElementsByTagName("lon")[0].InnerText;
            result.isp = ipInfoXML.GetElementsByTagName("isp")[0].InnerText;

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
