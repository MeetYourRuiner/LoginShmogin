using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using LoginShmogin.Application.Models;
using LoginShmogin.Application.Interfaces;

namespace LoginShmogin.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        
        public EmailSender(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public async Task SendEmailAsync(EmailRequest emailRequest)
        {
            var fromAddress = new MailAddress(_emailSettings.Email, _emailSettings.DisplayName);
            var toAddress = new MailAddress(emailRequest.To);

            var smtp = new SmtpClient
            {
                Host = _emailSettings.Host,
                Port = _emailSettings.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                //UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = emailRequest.Subject,
                Body = emailRequest.Body,
                IsBodyHtml = true
            })
            {
                try
                {
                    await smtp.SendMailAsync(message);
                }
                catch { }
            }
        }
    }
}
