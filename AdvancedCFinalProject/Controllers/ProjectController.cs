using AdvancedCFinalProject.Data;
using AdvancedCFinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AdvancedCFinalProject.Controllers
{
    
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext db;

        public ProjectController(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = db.Project;
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult CreateProject(int? Cid)
        {
            ViewBag.YourEnums = new SelectList(Enum.GetValues(typeof(Priority)), Priority.None);
            var comp = db.Company.FirstOrDefault(c => c.CompanyId == Cid);
            ViewBag.Company = comp.CompanyId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(int? Cid, [Bind("ProjectId,Title,Content,IsComplete,Priority")] Project project)
        {
            var comp = db.Company.FirstOrDefault(c => c.CompanyId == Cid);
            ViewBag.Company = comp.CompanyId;
            ViewBag.YourEnums = new SelectList(Enum.GetValues(typeof(Priority)), Priority.None);
            if (ModelState.IsValid)
            {
                project.CompanyId = comp.CompanyId;
                comp.Projects.Add(project);
                db.Project.Add(project);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateProject(int? id)
        {
            {
                ViewBag.Id = id;
                if (id == null)
                {
                    return NotFound();
                }

                var project = await db.Project.FindAsync(id);
                if (project == null)
                {
                    return NotFound();
                }
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProject(int id, [Bind("ProjectId,Title,IsComplete,Tasks")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(project);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await db.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            else
            {
                db.Project.Remove(project);
                await db.SaveChangesAsync();
            }
            return View(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            Project project = db.Project.Find(id);
            return View(project);
        }
    }
}
