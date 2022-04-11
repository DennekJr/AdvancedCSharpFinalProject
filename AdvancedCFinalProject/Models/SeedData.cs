using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AdvancedCFinalProject.Data;
namespace AdvancedCFinalProject.Models
{
    public class SeedData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string newRole = new string("Project Manager");
            await roleManager.CreateAsync(new IdentityRole(newRole));

            string newRole2 = new string("Developer");
            await roleManager.CreateAsync(new IdentityRole(newRole2));

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            ApplicationUser firstProjectManager = new ApplicationUser
            {
                Email = "projectManager@company.ca",
                NormalizedEmail = "PROJECTMANAGER@COMPANY.CA",
                UserName = "projectManager@company.ca",
                EmailConfirmed = true
            };

            var hashedPassword = passwordHasher.HashPassword(firstProjectManager, "P@ssword1");
            firstProjectManager.PasswordHash = hashedPassword;
            firstProjectManager.Salary = 7000;
            await userManager.CreateAsync(firstProjectManager);
            await userManager.AddToRoleAsync(firstProjectManager, "Project Manager");

            ApplicationUser user1 = new ApplicationUser { Email = "sam@gmail.com", NormalizedEmail = "SAM@GMAIL.COM", UserName = "sam@gmail.com", EmailConfirmed = true };
            ApplicationUser user2 = new ApplicationUser { Email = "cat@gmail.com", NormalizedEmail = "CAT@GMAIL.COM", UserName = "cat@gmail.com", EmailConfirmed = true };
            ApplicationUser user3 = new ApplicationUser { Email = "andre@gmail.com", NormalizedEmail = "ANDRE@GMAIL.COM", UserName = "andre@gmail.com", EmailConfirmed = true };
            ApplicationUser user4 = new ApplicationUser { Email = "manager@gmail.com", NormalizedEmail = "MANAGER@GMAIL.COM", UserName = "manager@gmail.com", EmailConfirmed = true };

            var hashedPassword2 = passwordHasher.HashPassword(user1, "P@ssword1");
            user1.PasswordHash = hashedPassword2;
            user1.Salary = 3000;
            await userManager.CreateAsync(user1);
            await userManager.AddToRoleAsync(user1, "Developer");

            var hashedPassword3 = passwordHasher.HashPassword(user2, "P@ssword2");
            user2.PasswordHash = hashedPassword3;
            user2.Salary = 4000;
            await userManager.CreateAsync(user2);
            await userManager.AddToRoleAsync(user2, "Developer");

            var hashedPassword4 = passwordHasher.HashPassword(user3, "P@ssword3");
            user3.PasswordHash = hashedPassword4;
            user3.Salary = 5000;
            await userManager.CreateAsync(user3);
            await userManager.AddToRoleAsync(user3, "Developer");

            var hashedPassword5 = passwordHasher.HashPassword(user4, "P@ssword3");
            user4.PasswordHash = hashedPassword5;
            user4.Salary = 6000;
            await userManager.CreateAsync(user4);
            await userManager.AddToRoleAsync(user4, "Project Manager");

            context.SaveChanges();
        }
    }
}
