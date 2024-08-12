
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using OCD.Data;

namespace OCD.Services
{
    public class RoleRedirectService : IRoleRedirectService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly NavigationManager _navigationManager;

        public RoleRedirectService(UserManager<ApplicationUser> userManager, NavigationManager navigationManager)
        {
            _userManager = userManager;
            _navigationManager = navigationManager;
        }

        public async Task RedirectBasedOnRoleAsync(string userName)
        {
            var appUser = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(appUser);

            if (roles.Contains("Superuser"))
            {
                _navigationManager.NavigateTo("/manager-dashboard");
            }
            else if (roles.Contains("Requester"))
            {
                _navigationManager.NavigateTo("/requester-dashboard");
            }
            else if (roles.Contains("Ops"))
            {
                _navigationManager.NavigateTo("/ops-dashboard");
            }
            else if (roles.Contains("Viewer"))
            {
                _navigationManager.NavigateTo("/viewer-dashboard");
            }
        }
    }
}

