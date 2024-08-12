using Microsoft.AspNetCore.Identity;
using OCD.Data;

namespace OCD.Services
{
    public interface IEmailService
    {
        Task SendEmailToSuperUser(ApplicationUser user);
        Task SendEmailToUser(ApplicationUser user);
        Task SendTestEmail(string subject, string message);
    }
}
