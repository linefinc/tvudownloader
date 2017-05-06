using System.Net;
using System.Text;
using System.Threading;

/// <summary>
///     Fetches a Web Page
/// </summary>
internal class WebFetch
{
    public static string Fetch(string page, bool clean, CookieContainer cookieContainer, int myDelay = 0)
    {
        Thread.Sleep(myDelay);

        try
        {
            // used to build entire input
            var sb = new StringBuilder();

            // used on each read operation
            var buf = new byte[8192];

            // prepare the web page we will be asking for
            var request = (HttpWebRequest) WebRequest.Create(page);
            request.CookieContainer = cookieContainer;

            // execute the request
            var response = (HttpWebResponse) request.GetResponse();

            // we will read data via the response stream
            var resStream = response.GetResponseStream();

            string tempString = null;
            int count = 0;

            do
            {
                // fill the buffer with data
                count = resStream.Read(buf, 0, buf.Length);

                // make sure we read some data
                if (count != 0)
                {
                    // translate from bytes to ASCII text
                    tempString = Encoding.UTF8.GetString(buf, 0, count);

                    // continue building the string
                    sb.Append(tempString);
                }
            } while (count > 0); // any more data to read?

            if (clean)
            {
                sb.Replace("&quot;", "\""); //&quot;
                sb.Replace("&apos;", "'"); //&quot;
                sb.Replace("&amp;", "&"); //&amp;
                sb.Replace("&lt;", "<"); //&quot;
                sb.Replace("&gt;", ">"); //&quot;
                sb.Replace("%5B", "["); // %5D -> [
                sb.Replace("%5b", "["); // %5D -> [
                sb.Replace("%5d", "]"); // %5D -> ]
                sb.Replace("%5D", "]"); // %5D -> ]
            }

            return sb.ToString();
        }
        catch
        {
            return null;
        }
    }
}