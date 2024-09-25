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

        public async Task SendEmail(string subject, string message, string emailAddress)
        {
            await SendEmailAsync(subject, message, emailAddress);
        }
        public async Task SendTestEmail(string subject, string messageBody)
        {
            var smtpHost = _configuration["Smtp:Host"];
            var smtpUsername =  _configuration["Smtp:Username"];
            var smtpPassword = Environment.GetEnvironmentVariable("OCD_SMTP_PASS") ?? _configuration["Smtp:Password"];
            var from = _configuration["Smtp:From"];

            var smtpClient = new SmtpClient(smtpHost)
            {
                Port = int.Parse(_configuration["Smtp:Port"]!),
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = false,
            };
            var message = new MailMessage
            {
                From = new MailAddress(from!),
                Subject = subject,
                Body = messageBody,
                IsBodyHtml = true,
            };
            message.To.Add("");
            await smtpClient.SendMailAsync(message);
        }
        private async Task SendEmailAsync(string subject, string messageBody, string emailAddress, IEnumerable<string>? additionalEmailAddresses = null)
        {
            var smtpHost = _configuration["Smtp:Host"];
            var smtpUsername = _configuration["Smtp:Username"];
            var smtpPassword = Environment.GetEnvironmentVariable("OCD_SMTP_PASS") ?? _configuration["Smtp:Password"];
            var from = _configuration["Smtp:From"];

            if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword))
            {
                throw new InvalidOperationException("SMTP configuration is missing or invalid.");
            }

            using var smtpClient = new SmtpClient(smtpHost)
            {
                Port = int.Parse(_configuration["Smtp:Port"]!),
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = false,
            };

            using var message = new MailMessage
            {
                From = new MailAddress(from!),
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
