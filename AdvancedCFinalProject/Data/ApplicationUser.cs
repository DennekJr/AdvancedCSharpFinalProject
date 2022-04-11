using Microsoft.AspNetCore.Identity;

namespace AdvancedCFinalProject.Data
{
    public class ApplicationUser : IdentityUser
    {
        public int? Salary { get; set; }
    }
}
