using OCD.Data;
using System.Net.Mail;
using System.Net;

namespace OCD.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailToSuperUser(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailToUser(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public async Task SendTestEmail(string subject,string messageBody)
        {
            var smtpHost =  _configuration["Smtp:Host"];
            var smtpUsername = _configuration["GMAIL_SMTP_USER"] ?? _configuration["Smtp:Username"];
            var smtpPassword = _configuration["GMAIL_SMTP_PASS"] ?? _configuration["Smtp:Password"];

            var smtpClient = new SmtpClient(smtpHost)
            {
                Port = int.Parse(_configuration["Smtp:Port"]),
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true,
            };
            var message = new MailMessage
            {
                From = new MailAddress(smtpUsername ?? _configuration["Smtp:From"]),
                Subject = subject,
                Body = messageBody,
                IsBodyHtml = true,
            };
            message.To.Add("thegreatanubis179@gmail.com");
            await smtpClient.SendMailAsync(message);
        }
    }
}
