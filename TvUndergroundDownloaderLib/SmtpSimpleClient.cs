using System.Net.Mail;

namespace TvUndergroundDownloaderLib
{
    public class SmtpSimpleClient
    {
        public string MailReceiver { get; set; } = string.Empty;

        public string MailSender { get; set; } = string.Empty;

        public string SmtpClientHost { get; set; } = string.Empty;

        public int SmtpClientPort { get; set; } = 25;

        public string SmtpServerPassword { get; set; } = string.Empty;

        public string SmtpServerUserName { get; set; } = string.Empty;

        public bool SmtpServerEnableSsl { get; set; } = false;

        public bool SmtpServerEnableCredential { get; set; } = false;

        public SmtpSimpleClient()
        {

        }

        public SmtpSimpleClient(Config config)
        {
            MailReceiver = config.MailReceiver;

            MailSender = config.MailSender;

            SmtpClientHost = config.SmtpServerAddress;

            SmtpClientPort = config.SmtpServerPort;

            SmtpServerPassword = config.SmtpServerPassword;

            SmtpServerUserName = config.SmtpServerUserName;

            SmtpServerEnableSsl = config.SmtpServerEnableSsl;

            SmtpServerEnableCredential = config.SmtpServerEnableAuthentication;
        }

        public void SendEmail(string subject, string message)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(MailSender);

            mailMessage.To.Add(MailReceiver);

            mailMessage.Subject = subject;
            //mailMessage.IsBodyHtml = true;
            mailMessage.Body = message;

            SmtpClient smtpSimpleClient = new SmtpClient();
            smtpSimpleClient.UseDefaultCredentials = !SmtpServerEnableCredential;

            if (SmtpServerEnableCredential)
            {
                smtpSimpleClient.Credentials = new System.Net.NetworkCredential(SmtpServerUserName, SmtpServerPassword);
            }

            smtpSimpleClient.EnableSsl = SmtpServerEnableSsl;

            smtpSimpleClient.Host = SmtpClientHost;
            smtpSimpleClient.Port = SmtpClientPort;
            smtpSimpleClient.Send(mailMessage);

        }
    }
}