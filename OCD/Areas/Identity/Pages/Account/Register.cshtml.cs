using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OCD.Data;
using OCD.Services;
using System.ComponentModel.DataAnnotations;

namespace OCD.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public InputRegisterModel Input { get; set; }
        public IEnumerable<ApplicationUser> Managers { get; set; }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly DataContext _context;

        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailService emailService, DataContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _context = context;
        }
        public async Task OnGetAsync()
        {
           
            Managers = await _userManager.Users.Where(u => u.IsManager == true).ToListAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(Input.Email);
                if (existingUser is not null)
                {
                    ModelState.AddModelError(string.Empty, $"User with this email {Input.Email} already exists");
                    return Page();
                }

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var user = new ApplicationUser
                        {
                            UserName = Input.Email,
                            Email = Input.Email,
                            IsActive = false,
                            Name = Input.Name,
                            Surname = Input.Surname,
                            EmployeeNumber = Input.EmployeeNumber,
                            MobileNumber = Input.MobileNumber,
                            IsAdmin = Input.IsAdmin ?? false,
                            IsManager = Input.IsManager ?? false,
                            IsOperations = Input.IsOperations ?? false,
                            ManagerId = Input.ManagerId,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };

                        var result = await _userManager.CreateAsync(user, Input.Password);

                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, Input.SelectedRole);
                            var message = $"{Input.Name} {Input.Surname} has requested access to OCD as a {Input.SelectedRole}. Please log on to your dashboard to review the request and grant the user access.</p>";
                            var subject = "Request for access on OCD";

                           // await _emailService.SendTestEmail(subject, message);

                            await transaction.CommitAsync();

                            return new JsonResult(new { success = true, message = "Request for access sent. You will receive an email as soon as access has been granted.", redirectUrl = Url.Page("/account/login") });
                        }
                        else
                        {
                            var errors = result.Errors.Select(e => e.Description).ToList();
                            return new JsonResult(new { success = false, message = string.Join(", ", errors) });
                        }
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        ModelState.AddModelError(string.Empty, "Failed to send email. Please check your network connection or contact IT support.");
                        return Page();
                    }
                }
            }
            return Page();
        }
    }
}
public class InputRegisterModel
{
    [Required(ErrorMessage = "This field is required")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 30 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Surname must be between 3 and 30 characters")]
    public string Surname { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [RegularExpression(@"^[0-9a-zA-Z]*@(tihsa|telesure|autogen|budgetins|businessins|dialdirect|firstforwomen|virsekker|1life|brokersupport|hippo|hippoadvice|isservices)\.co\.za$", ErrorMessage = "Please use your TIH email address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Your employee number should have a minimum of 6 characters")]
    //[RegularExpression(@"(([a-z]|[A-Z])([a-z]|[A-Z])\d\d\d\d)", ErrorMessage = "Please enter a valid employee number")]
    public string EmployeeNumber { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Please enter a valid 10 digit SA phone number")]
    [RegularExpression(@"0[1-9]\d\d\d\d\d\d\d\d", ErrorMessage = "Please enter a valid 10 digit SA phone number")]
    public string MobileNumber { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [Compare("Password", ErrorMessage = "Passwords must match")]
    public string ConfirmPassword { get; set; }
    public Guid? ManagerId { get; set; }

    public bool? IsAdmin { get; set; } = false;
    public bool? IsActive { get; set; } = false;
    public bool? IsManager { get; set; } = false;
    public bool? IsOperations { get; set; } = false;
    public string? SelectedRole { get; set; }

}

