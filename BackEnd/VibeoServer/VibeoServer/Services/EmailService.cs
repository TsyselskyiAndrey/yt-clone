
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using VibeoServer.Helpers;
using VibeoServer.Services.Interfaces;

namespace VibeoServer.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true
            };
            mail.To.Add(toEmail);

            using (var smtp = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port) { 
                Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            })
            {
                await smtp.SendMailAsync(mail);
            }
        }
    }
}
