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

            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string newRole = new string("Project Manager");
            await roleManager.CreateAsync(new IdentityRole(newRole));

            string newRole2 = new string("Developer");
            await roleManager.CreateAsync(new IdentityRole(newRole2));

            var passwordHasher = new PasswordHasher<IdentityUser>();
            IdentityUser firstProjectManager = new IdentityUser
            {
                Email = "projectManager@company.ca",
                NormalizedEmail = "PROJECTMANAGER@COMPANY.CA",
                UserName = "projectManager@company.ca",
                EmailConfirmed = true
            };

            var hashedPassword = passwordHasher.HashPassword(firstProjectManager, "P@ssword1");
            firstProjectManager.PasswordHash = hashedPassword;
            await userManager.CreateAsync(firstProjectManager);
            await userManager.AddToRoleAsync(firstProjectManager, "Project Manager");

            IdentityUser user1 = new IdentityUser { Email = "sam@gmail.com", NormalizedEmail = "SAM@GMAIL.COM", UserName = "sam@gmail.com", EmailConfirmed = true };
            IdentityUser user2 = new IdentityUser { Email = "cat@gmail.com", NormalizedEmail = "CAT@GMAIL.COM", UserName = "cat@gmail.com", EmailConfirmed = true };
            IdentityUser user3 = new IdentityUser { Email = "andre@gmail.com", NormalizedEmail = "ANDRE@GMAIL.COM", UserName = "andre@gmail.com", EmailConfirmed = true };
            IdentityUser user4 = new IdentityUser { Email = "manager@gmail.com", NormalizedEmail = "MANAGER@GMAIL.COM", UserName = "manager@gmail.com", EmailConfirmed = true };

            var hashedPassword2 = passwordHasher.HashPassword(user1, "P@ssword1");
            user1.PasswordHash = hashedPassword2;
            await userManager.CreateAsync(user1);
            await userManager.AddToRoleAsync(user1, "Developer");

            var hashedPassword3 = passwordHasher.HashPassword(user2, "P@ssword2");
            user2.PasswordHash = hashedPassword3;
            await userManager.CreateAsync(user2);
            await userManager.AddToRoleAsync(user2, "Developer");

            var hashedPassword4 = passwordHasher.HashPassword(user3, "P@ssword3");
            user3.PasswordHash = hashedPassword4;
            await userManager.CreateAsync(user3);
            await userManager.AddToRoleAsync(user3, "Developer");

            var hashedPassword5 = passwordHasher.HashPassword(user4, "P@ssword3");
            user4.PasswordHash = hashedPassword5;
            await userManager.CreateAsync(user4);
            await userManager.AddToRoleAsync(user4, "Project Manager");

            context.SaveChanges();
        }
    }
}
