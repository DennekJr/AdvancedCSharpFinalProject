using Microsoft.AspNetCore.Mvc;

namespace AdvancedCFinalProject.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
