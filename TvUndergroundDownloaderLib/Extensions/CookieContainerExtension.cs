using System.Collections;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace TvUndergroundDownloaderLib.Extensions
{
    public static class CookieContainerExtension
    {
        /// <summary>
        /// Dump all coockies
        /// </summary>
        /// <param name="cookieContainer"></param>
        /// <returns></returns>
        public static string DumpToString(this CookieContainer cookieContainer)
        {
            var cookies = cookieContainer.GetAllCookies();
            StringBuilder sb = new StringBuilder();

            foreach (Cookie cookie in cookies)
            {
                sb.Append(cookie.Domain);
                sb.Append("\t");
                sb.Append(cookie.Name);
                sb.Append("\t");
                sb.Append(cookie.Value);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Extract all coocki from all site
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public static CookieCollection GetAllCookies(this CookieContainer container)
        {
            // https://stackoverflow.com/questions/15983166/how-can-i-get-all-cookies-of-a-cookiecontainer
            var allCookies = new CookieCollection();
            var domainTableField = container.GetType().GetRuntimeFields().FirstOrDefault(x => x.Name == "m_domainTable");
            var domains = (IDictionary)domainTableField.GetValue(container);

            foreach (var val in domains.Values)
            {
                var type = val.GetType().GetRuntimeFields().First(x => x.Name == "m_list");
                var values = (IDictionary)type.GetValue(val);
                foreach (CookieCollection cookies in values.Values)
                {
                    allCookies.Add(cookies);
                }
            }
            return allCookies;
        }
    }
}