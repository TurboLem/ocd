using OCD.Data;
using System.Net.Mail;
using System.Net;
using static MudBlazor.CategoryTypes;

namespace OCD.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendCampaignActivationEmail(string subject, string message, string requesterEmailAdrress, string managerEmailAddress)
        {
            var additionalEmailAddresses = new List<string> { managerEmailAddress };
            await SendEmailAsync(subject, message, requesterEmailAdrress, additionalEmailAddresses);
        }

        public Task SendEmailToSuperUser(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailToUser(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public async Task SendTestEmail(string subject, string messageBody)
        {
            var smtpHost = _configuration["Smtp:Host"];
            var smtpUsername = _configuration["GMAIL_SMTP_USER"] ?? _configuration["Smtp:Username"];
            var smtpPassword = _configuration["GMAIL_SMTP_PASS"] ?? _configuration["Smtp:Password"];

            var smtpClient = new SmtpClient(smtpHost)
            {
                Port = int.Parse(_configuration["Smtp:Port"]!),
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true,
            };
            var message = new MailMessage
            {
                From = new MailAddress(smtpUsername ?? _configuration["Smtp:From"]!),
                Subject = subject,
                Body = messageBody,
                IsBodyHtml = true,
            };
            message.To.Add("thegreatanubis179@gmail.com");
            await smtpClient.SendMailAsync(message);
        }
        private async Task SendEmailAsync(string subject, string messageBody, string emailAddress, IEnumerable<string>? additionalEmailAddresses = null)
        {
            var smtpHost = _configuration["Smtp:Host"];
            var smtpUsername = _configuration["GMAIL_SMTP_USER"] ?? _configuration["Smtp:Username"];
            var smtpPassword = _configuration["GMAIL_SMTP_PASS"] ?? _configuration["Smtp:Password"];

            if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword))
            {
                throw new InvalidOperationException("SMTP configuration is missing or invalid.");
            }

            using var smtpClient = new SmtpClient(smtpHost)
            {
                Port = int.Parse(_configuration["Smtp:Port"]!),
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true,
            };

            using var message = new MailMessage
            {
                From = new MailAddress(smtpUsername ?? _configuration["Smtp:From"]!),
                Subject = subject,
                Body = messageBody,
                IsBodyHtml = true,
            };

            message.To.Add(emailAddress);
            if (additionalEmailAddresses is not null)
            {
                foreach (var additionalEmail in additionalEmailAddresses)
                {
                    message.To.Add(additionalEmail);
                }
            }

            try
            {
                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("Failed to send email.", ex);
            }
        }

    }
}
