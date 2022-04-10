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
using Microsoft.AspNetCore.Identity;

namespace AdvancedCFinalProject.Controllers
{
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public TaskController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            _context = context;
            userManager = _userManager;
            roleManager = _roleManager;
        }

        // GET: Task
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tasks.Include(d => d.Developer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Task/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developerTask = await _context.Tasks
                .Include(d => d.Developer)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (developerTask == null)
            {
                return NotFound();
            }

            return View(developerTask);
        }

        // GET: Task/Create
        public IActionResult Create(string? selectedDev,int? Pid)
        {
            Project project = _context.Project.FirstOrDefault(x => x.ProjectId == Pid);
            ViewBag.YourEnums = new SelectList(Enum.GetValues(typeof(Priority)), Priority.None);
            //var developers = new List<string>();
            List<SelectListItem> developers = new List<SelectListItem>();
            foreach (var name in _context.Users)
            {
                developers.Add(new SelectListItem
                {
                    Text = name.Email.ToString(),
                    Value = name.Id,
                });
            }

            ViewBag.Developers = developers;
            ViewBag.ProjectId = Pid;
            return View();
        }

        // POST: Task/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string? selectedDev, int? Pid,[Bind("TaskId,Title,CompletionRate,IsComplete,Priority,DeveloperId")] DeveloperTask developerTask)
        {
            ViewBag.ProjectId = Pid;
            ViewBag.YourEnums = new SelectList(Enum.GetValues(typeof(Priority)), Priority.None);

            List<SelectListItem> developers = new List<SelectListItem>();
            foreach (var name in _context.Users)
            {
                developers.Add(new SelectListItem
                {
                    Text = name.Email.ToString(),
                    Value = name.Id,
                });
            }
            ApplicationUser user = _context.Users.FirstOrDefault(x => x.Id == selectedDev);
            Developer newDev = new Developer
            {
                Title = user.Email,
            };
            developerTask.Developer = newDev;
            ViewBag.Developers = developers;
            Project project = _context.Project.FirstOrDefault(x => x.ProjectId == Pid);

            if (ModelState.IsValid)
            {
                _context.Add(developerTask);
                project.Tasks.Add(developerTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(developerTask);
        }

        // GET: Task/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.YourEnums = new SelectList(Enum.GetValues(typeof(Priority)), Priority.None);
            var developerTask = await _context.Tasks.FindAsync(id);
            if (developerTask == null)
            {
                return NotFound();
            }
            ViewData["DeveloperId"] = new SelectList(_context.Developer, "DeveloperId", "DeveloperId", developerTask.DeveloperId);
            return View(developerTask);
        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskId,Title,CompletionRate,IsComplete,Priority,DeveloperId")] DeveloperTask developerTask)
        {
            if (id != developerTask.TaskId)
            {
                return NotFound();
            }
            ViewBag.YourEnums = new SelectList(Enum.GetValues(typeof(Priority)), Priority.None);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(developerTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeveloperTaskExists(developerTask.TaskId))
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
            ViewData["DeveloperId"] = new SelectList(_context.Developer, "DeveloperId", "DeveloperId", developerTask.DeveloperId);
            return View(developerTask);
        }

        // GET: Task/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developerTask = await _context.Tasks
                .Include(d => d.Developer)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (developerTask == null)
            {
                return NotFound();
            }

            return View(developerTask);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var developerTask = await _context.Tasks.FindAsync(id);
            _context.Tasks.Remove(developerTask);
            // new save
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeveloperTaskExists(int id)
        {
            return _context.Tasks.Any(e => e.TaskId == id);
        }
    }
}
