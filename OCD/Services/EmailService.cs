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
            var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
            {
                Port = int.Parse(_configuration["Smtp:Port"]),
                Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                EnableSsl = true,
            };
            var message = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:From"]),
                Subject = subject,
                Body = messageBody,
                IsBodyHtml = true,
            };
            message.To.Add("thegreatanubis179@gmail.com");
            await smtpClient.SendMailAsync(message);
        }
    }
}
