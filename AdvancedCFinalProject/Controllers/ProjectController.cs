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

        public IActionResult CreateProject(int? Cid)
        {
            var comp = db.Company.FirstOrDefault(c => c.CompanyId == Cid);
            ViewBag.Company = comp.CompanyId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(int? Cid, [Bind("ProjectId,Title")] Project project)
        {
            var comp = db.Company.FirstOrDefault(c => c.CompanyId == Cid);
            ViewBag.Company = comp;
            if (ModelState.IsValid)
            {
                comp.Projects.Add(project);
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
