using System.Net;
using System.Net.Mail;

namespace TuyenDungWeb.Utility
{
    public class SendMail
    {
        public static async Task SendMailTo(string email, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress("huynhpro@gmail.com");
                mail.Subject = subject;
                mail.Body = body;
                // create a new SMTP client
                SmtpClient client = new SmtpClient("smtp.example.com"); // replace with your SMTP server address
                client.Credentials = new NetworkCredential("username", "password"); // replace with your SMTP username and password

                // send the message
                await client.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
