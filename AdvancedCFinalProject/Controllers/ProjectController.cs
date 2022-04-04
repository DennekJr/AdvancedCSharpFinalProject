using Microsoft.AspNetCore.Mvc;

namespace AdvancedCFinalProject.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
