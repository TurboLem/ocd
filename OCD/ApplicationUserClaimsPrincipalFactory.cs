using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OCD.Data;
using System.Security.Claims;

namespace OCD
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> 
            userManager, IOptions<IdentityOptions> optionsAccessor) 
            : base(userManager, optionsAccessor)
        {
        }

        //protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        //{
        //    var identity = await base.GenerateClaimsAsync(user);
        //    identity.AddClaim(new Claim(ClaimTypes.GivenName, user.Name ?? string.Empty));
        //    identity.AddClaim(new Claim(ClaimTypes.Surname, user.Surname ?? string.Empty));
        //    return identity;
        //}
    }
}
