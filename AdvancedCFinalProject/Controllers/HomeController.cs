using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AdvancedCFinalProject.Models;
using AdvancedCFinalProject.Helpers;
using Microsoft.AspNetCore.Identity;
using AdvancedCFinalProject.Data;

namespace AdvancedCFinalProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext db;
    User_Manager user_Manager;
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext _db)
    {
        user_Manager = new User_Manager();
        _logger = logger;
        db = _db;
    }

    public IActionResult Index()
    {
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

