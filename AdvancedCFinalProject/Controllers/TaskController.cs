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
        public int ProjId;

        public TaskController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            _context = context;
            userManager = _userManager;
            roleManager = _roleManager;
        }

        // GET: Task
        public async Task<IActionResult> Index()
        {
            string userMail = User.Identity.Name;
            ApplicationUser user = await userManager.FindByEmailAsync(userMail);
            ViewBag.developer = user.Email;
            var applicationDbContext = _context.Tasks.Include(d => d.Developer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Task/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string userMail = User.Identity.Name;
            ApplicationUser user = await userManager.FindByEmailAsync(userMail);
            if (id == null)
            {
                return NotFound();
            }

            var developerTask = await _context.Tasks
                .Include(d => d.Developer)
                .Include(c => c.Comment)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (developerTask == null)
            {
                return NotFound();
            }

            if (developerTask.IsComplete && developerTask.Developer.Title == user.Email)
            {
                ViewBag.developer = user.Email;
            }
            if(developerTask.Comment != null)
            {
                ViewBag.comment = developerTask.Comment.Description;
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
            
            if (ModelState.IsValid)
            {
                if (Pid != null)
                {
                    developerTask.Project = _context.Project.FirstOrDefault(p => p.ProjectId == Pid);
                    developerTask.ProjectId = (int)Pid;
                    var project = developerTask.Project;
                    project.Tasks.Add(developerTask);
                }
                ViewBag.Developers = developers;
                _context.Tasks.Add(developerTask);
                await _context.SaveChangesAsync();
                return RedirectToRoute(new { controller = "Project", action = "Details", id = Pid });
            }
            return View(developerTask);
        }

        // GET: Task/Edit/5
        public async Task<IActionResult> Edit(int? id, string? mail)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.YourEnums = new SelectList(Enum.GetValues(typeof(Priority)), Priority.None);
            var developerTask = _context.Tasks.Include(d => d.Developer).First(x => x.TaskId == id);
            string userMail = User.Identity.Name;
            ApplicationUser user = await userManager.FindByEmailAsync(userMail);
            if (user.Email == developerTask.Developer.Title)
            {
                    ViewBag.mail = user.Email.ToString();

            }

            ProjId = (int)developerTask.ProjectId;

            if (developerTask == null)
            {
                return NotFound();
            }
            ViewData["DeveloperId"] = new SelectList(_context.Developer, "DeveloperId", "DeveloperId", developerTask.DeveloperId);
            return View(developerTask);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskId,Title,CompletionRate,IsComplete,Priority,DeveloperId")] DeveloperTask developerTask, string? stringComment, string? UrgentComment)
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
                    if (stringComment != null)
                    {
                        Comment newComment = new Comment
                        {
                            Developer = developerTask.Developer,
                            Description = stringComment,
                        };
                        developerTask.Comment = newComment;
                    }
                    if (UrgentComment != null && !developerTask.IsComplete)
                    {
                        Urgent newNote = new Urgent
                        {
                            Developer = developerTask.Developer,
                            Description = UrgentComment,
                            IsURgent = true,
                            UrgentNote = UrgentComment
                        };
                        developerTask.ProjectId = ProjId;
                        developerTask.UrgentNote = newNote;
                    }

                    DeveloperTask taskToEdit = _context.Tasks.FirstOrDefault(t => t.TaskId == developerTask.TaskId);
                    taskToEdit.Title = developerTask.Title;
                    taskToEdit.CompletionRate = developerTask.CompletionRate;
                    taskToEdit.IsComplete = developerTask.IsComplete;
                    taskToEdit.Priority = developerTask.Priority;
                    taskToEdit.DeveloperId = developerTask.DeveloperId;
                    if (taskToEdit.IsComplete)
                    {
                        Notification TaskCompleteNotification = new Notification
                        {
                            DeveloperTask = taskToEdit,
                            Content = $"{taskToEdit.Title} is Complete",
                            projectId = taskToEdit.ProjectId,
                            Project = _context.Project.FirstOrDefault(x => x.ProjectId == taskToEdit.ProjectId),
                        };
                        Project ProjectToSendNotificion = _context.Project.FirstOrDefault(x => x.ProjectId == taskToEdit.ProjectId);
                        _context.Notification.Add(TaskCompleteNotification);
                        ProjectToSendNotificion.Notifications.Add(TaskCompleteNotification);
                    }
                    _context.Update(taskToEdit);
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
                return RedirectToRoute(new { controller = "Project", action = "Details", id = id });
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
