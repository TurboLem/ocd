using OCD.Data;
using System.Security.Claims;

namespace OCD.Services
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetInactiveUsersAsync();
        Task RedirectBasedOnUserRoleAsync(string userName);
        Task<ApplicationUser> GetLoggedInUserAsync(ClaimsPrincipal userPrincipal);
        Task ActivateAccountAsync(ApplicationUser user);
        Task<bool> UpdateUserRoleAsync(ApplicationUser user, string newRole);
    }
}
