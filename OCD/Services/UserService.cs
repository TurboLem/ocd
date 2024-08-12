using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OCD.Data;
using System.Security.Claims;

namespace OCD.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly NavigationManager _navigationManager;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UserService(UserManager<ApplicationUser> userManager, NavigationManager navigationManager, IServiceScopeFactory serviceScopeFactory)
        {
            _userManager = userManager;
            _navigationManager = navigationManager;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<IEnumerable<ApplicationUser>> GetInactiveUsersAsync()
        {

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                return await userManager.Users.Where(x => x.IsActive == false).ToListAsync();
            }

        }

        public Task RedirectBasedOnUserRoleAsync(string userName)
        {
            throw new NotImplementedException();
        }
        public async Task<ApplicationUser> GetLoggedInUserAsync(ClaimsPrincipal userPrincipal)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                if (userPrincipal.Identity.IsAuthenticated)
                {
                    var user = await userManager.GetUserAsync(userPrincipal);
                    return user;
                }
                return null;
            }
        }

        public async Task ActivateAccountAsync(ApplicationUser user)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                user.IsActive = true;
                await userManager.UpdateAsync(user);
            }
        }

        public async Task<bool> UpdateUserRoleAsync(ApplicationUser user, string newRole)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var currentRoles = await userManager.GetRolesAsync(user);
                var removeResult = await userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    return false;
                }

                var addResult = await userManager.AddToRoleAsync(user, newRole);
                return addResult.Succeeded;
            }
        }
    }
}
