using System.Net;
using System.Net.Mail;

namespace TURNERO.Helpers
{
    public class HelperMail
    {
        private IConfiguration configuration;
        public HelperMail(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        private MailMessage ConfigureMail(string to, string subject, string message)
        {
            string from = configuration.GetValue<string>("MailSettings:user");
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = true;
            return mail;
        }
        private void ConfigureSmtp(MailMessage mail)
        {
            string user = configuration.GetValue<string>("MailSettings:user");
            string password = configuration.GetValue<string>("MailSettings:password");
            string host = configuration.GetValue<string>("MailSettings:host");

            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Host = host;
            NetworkCredential credentials = new NetworkCredential(user, password);
            client.Credentials = credentials;
            client.Send(mail);
        }
        public void SendMail(string to, string subject, string message)
        {
            MailMessage mail = this.ConfigureMail(to, subject, message);
            ConfigureSmtp(mail);
        }
    }
}
