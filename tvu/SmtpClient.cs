using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;


namespace tvu
{
    class SmtpClient
    {
        public static bool SendEmail(string SmtpServer, string EmailReceiver, string EmailSender, string Subject, string Message)
        {

            //Notifica eMule: Viene terminato un Download
            //
            //Scaricato:
            //Band.Of.Brothers.10.Il.Nido.Delle.Aquile.ITA.DVDRip.DivX.[tvu.org.ru].avi

            try
            {
                // Create a socket connection with the specified server and port.
                Socket s = ConnectSocket(SmtpServer, 25);

                if (s == null)
                    return false;

                int pos = EmailSender.IndexOf('@');
                string p = EmailSender.Substring(pos);
                p = "HELO " + p + "\r\n";
                send(s, p);


                Recive(s);

                p = "MAIL FROM: <" + EmailSender + ">\r\n";
                send(s, p);

                Recive(s);

                p = "RCPT TO: <" + EmailReceiver + ">\r\n";
                send(s, p);

                Recive(s);

                p = "DATA" + "\r\n";
                send(s, p);

                p = Recive(s);

                p = "Subject: " + Subject + "\r\n";
                p += "From: " + EmailSender + "\r\n";
                p += "To: " + EmailReceiver + "\r\n";
                p += Message + "\r\n";
                p += "\r\n.\r\n";
                send(s, p);

                Recive(s);
                p = "QUIT\r\n";
                send(s, p);
                Recive(s);

                s.Close();


                return true;
            }

            catch
            {
                return false;
                // fallito
            }
        }

        // from msdn http://msdn.microsoft.com/it-it/library/system.net.sockets.socket%28v=vs.80%29.aspx#Y1377
        private static Socket ConnectSocket(string server, int port)
        {
            Socket s = null;
            IPHostEntry hostEntry = null;

            // Get host related information.
            hostEntry = Dns.GetHostEntry(server);

            // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
            // an exception that occurs when the host IP Address is not compatible with the address family
            // (typical in the IPv6 case).
            foreach (IPAddress address in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address, port);
                Socket tempSocket =
                    new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                tempSocket.Connect(ipe);

                if (tempSocket.Connected)
                {
                    s = tempSocket;
                    return s; 
                }
                
                
            }
            return s;
        }

        private static void send(Socket s, String msg)
        {
            Byte[] bytesSent = Encoding.ASCII.GetBytes(msg);
            
            // Send request to the server.
            s.Send(bytesSent, bytesSent.Length, 0);
        }

        private static string Recive(Socket s)
        {
            int bytes = 0;
            Byte[] bytesReceived = new Byte[256];
            string page = "";
            bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);
            //while ((bytes = s.Receive(bytesReceived, bytesReceived.Length, 0)) > 0) 
            {
                
                page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
            }
           
            
            return page;
        }
    
    }
}
