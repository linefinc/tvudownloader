namespace TvUndergroundDownloader
{
    internal class SmtpClient
    {
        public static bool SendEmail(string SmtpServer, string EmailReceiver, string EmailSender, string Subject, string Message)
        {
            //Notifica eMule: Viene terminato un Download
            //
            //Scaricato:
            //Band.Of.Brothers.10.Il.Nido.Delle.Aquile.ITA.DVDRip.DivX.[tvu.org.ru].avi

            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add(EmailReceiver);
            message.Subject = Subject;
            message.From = new System.Net.Mail.MailAddress(EmailSender);
            message.Body = Message;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(SmtpServer);
            try
            {
                smtp.Send(message);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}