using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Net;

namespace IsoTreatmentProcessSupportAPI.Services
{
    public  interface IMailkitService
    {
        void Send(string userEmail, string emailConfirmationToken);
    }
    public class MailkitService : IMailkitService
    {
        private readonly IConfiguration _configuration;
        public MailkitService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Send(string userEmail, string emailConfirmationToken)
        {
            //emailConfirmationToken = WebUtility.UrlEncode(emailConfirmationToken);
            var email = new MimeMessage();
            Uri confirmationLink = new Uri("http://localhost:5173/activateAccount?token=" + emailConfirmationToken);
            string body = "<html><body><h1>Witamy w IsoSupport!</h1>" +
                      $"<p>Kliknij <a href='{confirmationLink}'>tutaj</a> aby potwierdzić swój adres email.</p></body></html>";

            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailUsername").Value)); 
            email.To.Add(MailboxAddress.Parse(userEmail));
            email.Subject = "Potwierdź swój adres email";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            using var smtp = new SmtpClient();
            smtp.Connect(_configuration.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration.GetSection("EmailUsername").Value, _configuration.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
