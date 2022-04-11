#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdvancedCFinalProject.Data;
using AdvancedCFinalProject.Models;
using AdvancedCFinalProject.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace AdvancedCFinalProject.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public CompanyController(ILogger<HomeController> logger, ApplicationDbContext _db, RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            roleManager = _roleManager;
            userManager = _userManager;
        }

        // GET: Company
        public async Task<IActionResult> Index()
        {
            return View(await db.Company.ToListAsync());
        }

        // GET: Company/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = db.Company.Include(x => x.Projects)
                .FirstOrDefault(m => m.CompanyId == id);
            var projects = db.Project.Include(t => t.Tasks).Where(p => p.CompanyId == company.CompanyId);
            var results = from p in company.Projects
                                             group p.Tasks by p.ProjectId into g
                                             select new { PersonId = g.Key, ProjectTasks = g.ToList() };
            //foreach(var project in projectsForCompany)
            //{
            //    var h = project.Tasks.Where(x => x.projectId == project.ProjectId);

            //}
            ViewBag.companyProject = projects.ToList();
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Company/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Company/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,Title")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Company.Add(company);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Company/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await db.Company.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Company/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,Title")] Company company)
        {
            if (id != company.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(company);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.CompanyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Company/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await db.Company
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await db.Company.FindAsync(id);
            db.Company.Remove(company);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return db.Company.Any(e => e.CompanyId == id);
        }

        public async Task<IActionResult> ViewDeveloperTasks(int? DevId)
        {
            if (DevId == null)
            {
                return NotFound();
            }
            var developer = db.Developer.Include(t => t.Tasks).FirstOrDefault(dev => dev.DeveloperId == DevId);
            var devTasks = db.Tasks.FirstOrDefault(t => t.DeveloperId == developer.DeveloperId);

              
            return View(developer);
        }

        [HttpPost]
        public async Task<IActionResult> ViewDeveloperTasks(int? devId, int taskId, int? newRate, string? comment, bool? IsChecked = false)
        {
            if (devId == null)
            {
                return NotFound();
            }
            var developer = db.Developer.Include(t => t.Tasks).FirstOrDefault(dev => dev.DeveloperId == devId);
            var devTask = db.Tasks.FirstOrDefault(t => t.TaskId == taskId);
            if(IsChecked == true)
            {
                devTask.IsComplete = true;

            } else
            {
                devTask.CompletionRate = newRate;
            }
            if(comment != null)
            {
                Comment newComment = new Comment
                {
                    Description = comment,
                    DeveloperId = (int)devId,
                    DeveloperTaskId = taskId,
                };
                db.Comments.Add(newComment);
                developer.Comments.Add(newComment);
                await db.SaveChangesAsync();
            }

            return View(devTask);
        }

        [Authorize(Roles = "Project Manager")]
        public async Task<IActionResult> TaskManager(string? id, int? addId, int? deleteId, int? editId, int? assignId)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            string role = "Project Manager";
            bool userIsInRole = await userManager.IsInRoleAsync(user, role);
            // get method
            // post method of task manager
            if (userIsInRole)
            {
                return RedirectToAction("ActionOrViewName", "TaskController");
            }
            return View();
        }


        [Authorize(Roles = "Project Manager")]
        [HttpGet]
        public IActionResult ProjDetails(int projId)
        {
            Project project = db.Project.Where(u => u.ProjectId == projId).Include(t => t.Tasks.OrderByDescending(p => p.CompletionRate)).First();
            if (project == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }
        [Authorize(Roles = "Project Manager")]
        public IActionResult ProjManagerDashboard()
        {
            string UserName = User.Identity.Name;
            Developer DevUser = db.Developer.First(u => u.Title == UserName);
            List<Notification> notifications = db.Notification.Where(u => u.DeveloperId == DevUser.DeveloperId).ToList();
            ViewBag.NotiCount = notifications.Count;
            List<Project> projects = db.Project.Include(p => p.Tasks.OrderByDescending(p => p.Priority)).ThenInclude(j => j.Developer).ToList();
            return View(projects);
        }

        public IActionResult DeveloperNotifications()
        {
            string UserName = User.Identity.Name;
            Developer DevUser = db.Developer.Where(u => u.Title == UserName).FirstOrDefault();
            List<Notification> notifications = db.Notification.Where(u => u.DeveloperId == DevUser.DeveloperId).ToList();


            return View(notifications);
        }

        public IActionResult HideCompleted()
        {
            foreach (var project in db.Project)
            {
                foreach(var task in project.Tasks)
                {
                    task.Hidden = true;
                }
            }
            return RedirectToAction("ProjManagerDashboard");
        }

        public IActionResult ShowCompleted()
        {
            foreach (var project in db.Project)
            {
                foreach(var task in project.Tasks)
                {
                    if(task.IsComplete == true)
                    {
                        task.Hidden = false;
                    }
                }
            }
            return RedirectToAction("ProjManagerDashboard");
        }

    }
}
