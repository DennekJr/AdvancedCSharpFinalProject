using AdvancedCFinalProject.Data;
using AdvancedCFinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedCFinalProject.Controllers
{
    
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext db;

        public ProjectController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult CreateProject()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([Bind("ProjectId,Title")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Project.Add(project);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            List<Project> allProjects = db.Project.ToList();
            return View(allProjects);
        }
    }
}
