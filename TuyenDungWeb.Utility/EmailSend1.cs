using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace TuyenDungWeb.Utility
{
    public class EmailSender1 : IEmailSender
    {
        public string SendGridSecret { get; set; }

        public EmailSender1(IConfiguration _config)
        {
            SendGridSecret = _config.GetValue<string>("SendGrid:SecretKey");
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //logic to send email

            var client = new SendGridClient(SendGridSecret);

            var from = new EmailAddress("hello@dotnetmastery.com", "Bulky Book");
            var to = new EmailAddress(email);
            var message = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

            return client.SendEmailAsync(message);


        }
    }
}
