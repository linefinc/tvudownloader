using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;


namespace tvu
{
    class SmtpClient
    {
        bool SendEmail(string SmtpServer, string EmailReceiver, string EmailSender, string Message)
        {
            Byte[] bytesReceived = new Byte[256];

            // Create a socket connection with the specified server and port.
            Socket s = ConnectSocket(SmtpServer, 25);

            if (s == null)
                return false;

            this.send(s, "HELLO");
            

            // Receive the server home page content.
            int bytes = 0;

            //send(null);
            //send("HELO " + hostName);
            //send("MAIL FROM: " + "my gmail email");
            //send("RCPT TO: " + "my gmail email");
            //send("DATA");
            //send("Happy SMTP Programming!!");
            //send("Happy SMTP Programming!!");
            //send(".");
            //send("QUIT");


            // The following will block until te page is transmitted.
            string page ="";
            do
            {
                bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);
                page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
            }
            while (bytes > 0);

            return true;
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
                    break;
                }
                else
                {
                    continue;
                }
            }
            return s;
        }

        private void send(Socket s, String msg)
        {
            Byte[] bytesSent = Encoding.ASCII.GetBytes(msg);
            
            // Send request to the server.
            s.Send(bytesSent, bytesSent.Length, 0);
        }

    
    }
}
