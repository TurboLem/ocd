using Microsoft.JSInterop;

namespace OCD.Services
{
    public interface IRoleRedirectService
    {
        Task RedirectBasedOnRoleAsync(string userName);
    }
}
