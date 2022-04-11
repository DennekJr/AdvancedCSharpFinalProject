using AdvancedCFinalProject.Data;
using Microsoft.AspNetCore.Identity;

namespace AdvancedCFinalProject.Helpers
{
    public class User_Manager
    {
        private static ApplicationDbContext db;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public User_Manager() { }

        public User_Manager(ApplicationDbContext _db, RoleManager<IdentityRole> _roleManager, UserManager<IdentityUser> _userManager)
        {
            db = _db;
            roleManager = _roleManager;
            userManager = _userManager;
        }

        public async Task<List<IdentityUser>> AllUsersInRole(string? role)
        {
            try
            {
                List<IdentityUser> users = new List<IdentityUser>();
                if (role != null)
                {
                    foreach(var user in db.Users)
                    {
                        bool userIsInRole = await userManager.IsInRoleAsync(user, role);
                        if (userIsInRole)
                        {
                            users.Add(user);
                        }
                    }
                }
                return users;
            }
            catch (Exception ex)
            {
                throw new($"Users not found in the database.");
            }

        }

        public async void AddUserToRole(string? userId, string role)
        {
            try
            {
                if (userId != null && role != null)
                {
                    IdentityUser user = await userManager.FindByIdAsync(userId);
                    bool roleExists = await roleManager.RoleExistsAsync(role);

                    if (roleExists && user != null)
                    {
                        bool userIsAlreadyInRole = await userManager.IsInRoleAsync(user, role);
                        if (!userIsAlreadyInRole)
                        {
                            await userManager.AddToRoleAsync(user, role);
                            db.SaveChanges();
                        }
                    }
                }
            } 
            catch (Exception ex)
            {
            }
        }

        public async Task<bool> UserIsInRole(string? userId, string role)
        {
            if (userId != null && role != null)
            {
                IdentityUser user = await userManager.FindByIdAsync(userId);
                bool userIsInRole = await userManager.IsInRoleAsync(user, role);

                if (userIsInRole)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
