using System;
using System.IO;
using System.Net;
using System.Text;


/// <summary>
/// Fetches a Web Page
/// </summary>
class WebFetch
{

    
    const int myDelay = 1000; //max 3 connection per sec

    public static string Fetch(string page, bool clean)
    {


        System.Threading.Thread.Sleep(myDelay);

        try
        {
            // used to build entire input
            StringBuilder sb = new StringBuilder();

            // used on each read operation
            byte[] buf = new byte[8192];

            CookieContainer cookieContainer = new CookieContainer();
            Uri uriTvunderground = new Uri("http://tvunderground.org.ru/");
            cookieContainer.Add(uriTvunderground, new Cookie("h", "91ace5b7bb757ae8fb8b5075369147d0"));
            cookieContainer.Add(uriTvunderground, new Cookie("i", "138793"));
            cookieContainer.Add(uriTvunderground, new Cookie("t", "db5b508b4da45e41ed6d51a460815e78"));

            // prepare the web page we will be asking for
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(page);
            request.CookieContainer = cookieContainer;

            // execute the request
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            

            // we will read data via the response stream
            Stream resStream = response.GetResponseStream();

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
                    tempString = Encoding.ASCII.GetString(buf, 0, count);

                    // continue building the string
                    sb.Append(tempString);
                }
            }
            while (count > 0); // any more data to read?

            if (clean == true)
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
