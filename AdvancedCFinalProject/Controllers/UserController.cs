using AdvancedCFinalProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdvancedCFinalProject.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            db = context;
            userManager = _userManager;
            roleManager = _roleManager;
        }

        public IActionResult SelectRole()
        {
            var selectList = new SelectList(roleManager.Roles);
            return View(selectList);
        }

        public async Task<IActionResult> AllUsersInRole(string? role)
        {
            var users = new List<ApplicationUser>();
            var allUsers = db.Users.ToList();

            try
            {
                if (role != null)
                {
                    foreach (var user in allUsers)
                    {
                        if (user != null)
                        {
                            bool userIsInRole = await userManager.IsInRoleAsync(user, role);
                            if (userIsInRole)
                            {
                                users.Add(user);
                            }
                        }
                    }
                }
                return View(users);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        public IActionResult AddUserToRole()
        {
            ViewBag.Roles = new SelectList(roleManager.Roles);
            var allUsers = db.Users.ToList();
            ViewBag.Users = new SelectList(allUsers, "Id", "Email");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(string? userId, string? role, int? salary)
        {
            try
            {
                if (userId != null && role != null)
                {
                    ApplicationUser user = (ApplicationUser)await userManager.FindByIdAsync(userId);
                    bool roleExists = await roleManager.RoleExistsAsync(role);

                    if (roleExists && user != null)
                    {
                        bool userIsAlreadyInRole = await userManager.IsInRoleAsync(user, role);
                        if (!userIsAlreadyInRole)
                        {
                            user.Salary = (int)salary;
                            await userManager.AddToRoleAsync(user, role);
                            db.SaveChanges();
                        }
                    }
                }
                return RedirectToAction(nameof(AddUserToRole));
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        public IActionResult UserIsInRole()
        {
            ViewBag.Roles = new SelectList(roleManager.Roles);
            var allUsers = db.Users.ToList();
            ViewBag.Users = new SelectList(allUsers, "Id", "Email");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserIsInRole(string? userId, string? role)
        {
            string message;
            if (userId != null && role != null)
            {
                ApplicationUser user = await userManager.FindByIdAsync(userId);
                bool userIsInRole = await userManager.IsInRoleAsync(user, role);
                ViewBag.Roles = new SelectList(roleManager.Roles);
                var allUsers = db.Users.ToList();
                ViewBag.Users = new SelectList(allUsers, "Id", "Email");

                if (userIsInRole)
                {
                    message = "True";
                    ViewBag.Message = message;
                    return View("UserIsInRole");
                }
                else
                {
                    message = "False";
                    ViewBag.Message = message;
                    return View("UserIsInRole");
                }
            }
            else
            {
                message = "False";
                ViewBag.Message = message;
                return View("UserIsInRole");
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
