using Microsoft.AspNetCore.Identity;

namespace AdvancedCFinalProject.Models
{
    public class ApplicationUser: IdentityUser
    {
        public int Reputation { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<DeveloperTask> Tasks { get; set; }


        public static ApplicationUser? MapFromIdentityUser(IdentityUser user)
        {
            if (user == null) return null;

            return new ApplicationUser()
            {
                AccessFailedCount = user.AccessFailedCount,
                ConcurrencyStamp = user.ConcurrencyStamp,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Id = user.Id,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd,
                NormalizedEmail = user.NormalizedEmail,
                NormalizedUserName = user.NormalizedUserName,
                PasswordHash = user.PasswordHash,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Reputation = 0,
                SecurityStamp = user.SecurityStamp,
                TwoFactorEnabled = user.TwoFactorEnabled,
                UserName = user.UserName,
            };
        }
    }
}
