namespace OCD.Services
{
    public interface IEmailService
    {
        Task SendTestEmail(string subject, string message); // for testing purposes
        Task SendCampaignActivationEmail(string subject, string message, string requesterEmailAdrress, string managerEmailAddress);
        Task SendEmail(string subject, string message, string emailAddress);
    }
}
