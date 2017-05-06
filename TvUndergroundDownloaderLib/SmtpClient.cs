using System;
using System.Net.Mail;
using NLog;

namespace TvUndergroundDownloaderLib
{
    public class SmtpClient
    {
        public static bool SendEmail(string SmtpServer, string EmailReceiver, string EmailSender, string Subject,
            string Message)
        {
            var message = new MailMessage();
            message.To.Add(EmailReceiver);
            message.Subject = Subject;
            message.From = new MailAddress(EmailSender);
            message.Body = Message;
            var smtp = new System.Net.Mail.SmtpClient(SmtpServer);
            try
            {
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                var logger = LogManager.GetCurrentClassLogger();
                logger.Error(ex, "Unable to send email");
                return false;
            }
            return true;
        }
    }
}