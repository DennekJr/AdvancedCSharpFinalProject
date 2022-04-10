using AdvancedCFinalProject.Data;
using AdvancedCFinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AdvancedCFinalProject.Controllers
{
    
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<IdentityUser> userManager;


        public ProjectController(ApplicationDbContext context, UserManager<IdentityUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }
        public async Task<IActionResult> ShowingNotification()
        {
            var AllNotifications = db.Notification.Where(n => n.projectId != null);
            return View(AllNotifications);

        }
        public IActionResult MakingNotification()
        {
            return View();
        }
        [HttpPost]
        public IActionResult MakingNotification(int count)
        {
            var AllProjects = db.Project.Include(Project=> Project.Tasks);
            foreach (var project in AllProjects)
            {
                var AllTasks = project.Tasks;
                var notificationWithProject = db.Notification.Where(x => x.projectId == project.ProjectId);
                var today = DateTime.Today;
                var deadline = project.Deadline;
                if ((today - deadline).Days > 0 && AllTasks.Any(task => task.IsComplete == false))
                {
                    if (!notificationWithProject.Any(n => n.Content == $"{project.Title} deadline passed with unfinshiedTask "))
                    {
                        var newNotification = new Notification();
                        var UnfinishedTask = new Notification();                       
                        newNotification.Content = $"{project.Title} deadline passed with unfinshiedTask ";
                        newNotification.projectId = project.ProjectId;
                        db.Notification.Add(newNotification);

                    }

                }

            }
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public IActionResult NotFinishedTAskList(int? id)
        {
            var project = db.Project.FirstOrDefault(x => x.ProjectId == id);
            var notFinishedTask = db.Tasks.Where(Tasks => Tasks.IsComplete == false).ToList();
            var FinsihedDeadlineTasks = new List<DeveloperTask>();
            foreach(var Task in notFinishedTask)
            {
                if (Task.Deadline.Day >= DateTime.Today.Day)
                {
                    FinsihedDeadlineTasks.Add(Task);
                }                

            }
            return View(FinsihedDeadlineTasks);


        }
        
        public async Task<IActionResult> Index()
        {
            string userMail = User.Identity.Name;
            IdentityUser user = await userManager.FindByEmailAsync(userMail);
            var applicationDbContext = db.Project;
            ViewBag.NumOfNotifications = db.Notification.Count();
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> CreateProjectAsync(int? Cid)
        {
            string userMail = User.Identity.Name;
            IdentityUser user = await userManager.FindByEmailAsync(userMail);
            ViewBag.YourEnums = new SelectList(Enum.GetValues(typeof(Priority)), Priority.None);
            var comp = db.Company.FirstOrDefault(c => c.CompanyId == Cid);
            ViewBag.Company = comp.CompanyId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(int? Cid, [Bind("ProjectId,Title,Content,IsComplete,Priority")] Project project)
        {
            string userMail = User.Identity.Name;
            IdentityUser user = await userManager.FindByEmailAsync(userMail);
            var comp = db.Company.FirstOrDefault(c => c.CompanyId == Cid);
            ViewBag.Company = comp.CompanyId;
            ViewBag.YourEnums = new SelectList(Enum.GetValues(typeof(Priority)), Priority.None);
            if (ModelState.IsValid)
            {
                project.CompanyId = comp.CompanyId;
                project.ProjectManager = user.Email.ToString();
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
                ViewBag.YourEnums = new SelectList(Enum.GetValues(typeof(Priority)), Priority.None);
                if (id == null)
                {
                    return NotFound();
                }

                var project = await db.Project.FindAsync(id);
                if (project == null)
                {
                    return NotFound();
                }
                return View(project);
                
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProject(int id, [Bind("ProjectId,Title,Content,IsComplete,Priority")] Project project)
        {
            string userMail = User.Identity.Name;
            ViewBag.YourEnums = new SelectList(Enum.GetValues(typeof(Priority)), Priority.None);
            IdentityUser user = await userManager.FindByEmailAsync(userMail);
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Project ProjectToEdit = db.Project.FirstOrDefault(t => t.ProjectId == project.ProjectId);
                    ProjectToEdit.Title = project.Title;
                    ProjectToEdit.IsComplete = project.IsComplete;
                    ProjectToEdit.Priority = project.Priority;
                    bool AllTasksComplete = false;
                    int count = 0;
                    if (ProjectToEdit.IsComplete)
                    {
                        foreach(var task in ProjectToEdit.Tasks)
                        {
                            if (task.IsComplete)
                            {
                                count++;
                                if(count == ProjectToEdit.Tasks.Count())
                                {
                                    AllTasksComplete = true;
                                }
                            }
                        }
                        
                            Notification ProjectCompleteNotification = new Notification
                            {
                                Project = ProjectToEdit,
                                Content = $"{ProjectToEdit.Title} is Complete",
                                projectId = ProjectToEdit.ProjectId,
                            };
                        Project ProjectToSendNotificion = db.Project.FirstOrDefault(x => x.ProjectId == ProjectToEdit.ProjectId);
                        ProjectToSendNotificion.Notifications.Add(ProjectCompleteNotification);
                        db.Notification.Add(ProjectCompleteNotification);
                    }
                    db.Update(ProjectToEdit);
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
