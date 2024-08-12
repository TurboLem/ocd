using Microsoft.AspNetCore.Identity;

namespace OCD.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string EmployeeNumber { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public bool? IsActive { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsManager { get; set; }
        public bool? IsOperations { get; set; }
        public Guid? ManagerId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        
    }
}
