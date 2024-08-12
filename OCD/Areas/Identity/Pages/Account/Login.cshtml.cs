using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OCD.Data;
using System.ComponentModel.DataAnnotations;

namespace OCD.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                // Check if the user is not null and isActive is false
                if (user is null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
                else if (user.IsActive == false)
                {
                    ModelState.AddModelError(string.Empty, "Please contact your manager to grant you access");
                    return Page();
                }

                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    
                    if (roles.Contains("Superuser"))
                    {
                        return LocalRedirect("~/manager-dashboard");
                    }
                    else if (roles.Contains("Requester"))
                    {
                        return LocalRedirect("~/requester-dashboard");
                    }
                    else if (roles.Contains("Ops"))
                    {
                        return LocalRedirect("~/ops-dashboard");
                    }
                    else if (roles.Contains("Viewer"))
                    {
                        return LocalRedirect("~/viewer-dashboard");
                    }
                    
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return Page();
        }
    }
    public class InputModel
    {
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[0-9a-zA-Z]*@(tihsa|telesure|autogen|budgetinsurance|firstforwomen|virsekker|1life)\.co\.za$", ErrorMessage = "Please use your TIH email address")]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

    }
}
