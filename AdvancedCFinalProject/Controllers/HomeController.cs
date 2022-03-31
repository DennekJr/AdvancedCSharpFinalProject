using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AdvancedCFinalProject.Models;
using Microsoft.AspNetCore.Identity;
using AdvancedCFinalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdvancedCFinalProject.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private RoleManager<IdentityRole> _roleManager;
    private UserManager<ApplicationUser> _userManager;
    public ApplicationDbContext _db;

    public HomeController(ILogger<HomeController> logger,
                              RoleManager<IdentityRole> r,
                              UserManager<ApplicationUser> u,
                              ApplicationDbContext d)
    {
        _logger = logger;
        _roleManager = r;
        _userManager = u;
        _db = d;
    }
    [AllowAnonymous]
    public IActionResult AllUsers()
    {
        string userName = User.Identity.Name;
        ViewBag.userName = userName;
        List<ApplicationUser> users = _db.Users.Select(u => (ApplicationUser)u).ToList();
        return View(users);
    }
    public IActionResult CreateRole()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> CreateRole(string RoleName)
    {
        if (RoleName == null)
        {
            return NotFound();
        }
        try
        {
            await _roleManager.CreateAsync(new IdentityRole(RoleName));
            _db.SaveChanges();
            return View();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    public IActionResult SetUserRole()
    {
        var UserSelectList = new SelectList(_db.Users, "Id", "UserName");
        var RoleSelectList = new SelectList(_db.Roles, "Name", "Name");
        ViewBag.UserRole = UserSelectList;
        ViewBag.RoleName = RoleSelectList;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> SetUserRole(string Id, string RoleName)
    {
        if (Id == null || RoleName == null)
        {
            return BadRequest();
        }
        var user = _db.Users.FirstOrDefault(u => u.Id == Id);
        if (await _userManager.FindByIdAsync(Id) == null || await _roleManager.RoleExistsAsync(RoleName) == null)
        {
            return NotFound();
        }
        await _userManager.AddToRoleAsync((ApplicationUser)user, RoleName);
        _db.SaveChanges();
        return View();
    }
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        return View();
    }
    

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

