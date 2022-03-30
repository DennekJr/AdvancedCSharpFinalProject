using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AdvancedCFinalProject.Data;
using AdvancedCFinalProject.Models;

namespace AdvancedCFinalProject.Models
{
    public class SeedData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        }
    }
}
