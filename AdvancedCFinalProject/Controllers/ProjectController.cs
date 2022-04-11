using AdvancedCFinalProject.Data;
using AdvancedCFinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AdvancedCFinalProject.Controllers
{
    
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;


        public ProjectController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
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
            return RedirectToRoute(new { controller = "Company", action = "Index" });
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
            ApplicationUser user = await userManager.FindByEmailAsync(userMail);
            var applicationDbContext = db.Project;
            ViewBag.NumOfNotifications = db.Notification.Count();
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "Project Manager")]
        public async Task<IActionResult> CreateProject(int? Cid)
        {
            string userMail = User.Identity.Name;
            ApplicationUser user = await userManager.FindByEmailAsync(userMail);
            ViewBag.YourEnums = new SelectList(Enum.GetValues(typeof(Priority)), Priority.None);
            var comp = db.Company.FirstOrDefault(c => c.CompanyId == Cid);
            ViewBag.Company = comp.CompanyId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(int? Cid, [Bind("ProjectId,Title,Content,IsComplete,Budget,Priority")] Project project)
        {
            string userMail = User.Identity.Name;
            ApplicationUser user = await userManager.FindByEmailAsync(userMail);
            var comp = db.Company.FirstOrDefault(c => c.CompanyId == Cid);
            ViewBag.Company = comp.CompanyId;
            ViewBag.YourEnums = new SelectList(Enum.GetValues(typeof(Priority)), Priority.None);
            if (ModelState.IsValid)
            {
                project.CompanyId = comp.CompanyId;
                project.Manager = user;
                comp.Projects.Add(project);
                db.Project.Add(project);
                await db.SaveChangesAsync();
                return RedirectToRoute(new {controller = "Company", action = "Details", id = Cid});
            }
            return RedirectToRoute(new { controller = "Company", action = "Details", id = Cid });
        }

        [Authorize(Roles = "Project Manager")]
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
        public async Task<IActionResult> UpdateProject(int id, [Bind("ProjectId,Title,Content,Budget,IsComplete,Priority")] Project project)
        {
            string userMail = User.Identity.Name;
            ViewBag.YourEnums = new SelectList(Enum.GetValues(typeof(Priority)), Priority.None);
            ApplicationUser user = await userManager.FindByEmailAsync(userMail);
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
                return RedirectToRoute(new { controller = "Company", action = "Details", id = id });
            }
            return RedirectToRoute(new { controller = "Company", action = "Details", id = id });
        }

        [Authorize(Roles = "Project Manager")]
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
            return RedirectToRoute(new { controller = "Company", action = "Details", id = id });
        }

        public IActionResult Details(int? id)
        {
            Project project = db.Project.Include("Manager").Include("Tasks").Include("Tasks.Developer").First(p => p.ProjectId == id);
            int amountSpent = 0;
            if(project.Manager.Salary != null)
            {
                int managerSalary = (int)project.Manager.Salary;
                int developerSalary = 0;
                foreach (var task in project.Tasks)
                {
                    int currentDate = DateTime.Now.Day;
                    int startDate = task.CreatedTime.Day;
                    int numberOfDays = currentDate - startDate;
                    ApplicationUser user = db.Users.First(x => x.Email == task.Developer.Title);
                    if (user.Salary != null)
                    {
                        int salary = numberOfDays * (int)user.Salary;
                        developerSalary += salary;
                    }
                    
                }
                amountSpent = developerSalary + managerSalary;
            }
            int totalBudget = project.Budget;

            ViewBag.CompanyId = project.CompanyId;
            ViewBag.TotalAmountSpent = amountSpent;
            ViewBag.TotalBudget = totalBudget;
            return View(project);
        }

        [Authorize(Roles = "Project Manager")]
        public async Task<IActionResult> ExceededBudget()
        {
            List<Project> projects = new List<Project>();
            var allProjects = await db.Project.Include("Manager").Include("Tasks").Include("Tasks.Developer").ToListAsync();
            foreach (var project in allProjects)
            {
                int amountSpent = 0;
                int managerSalary = (int)project.Manager.Salary;
                int developerSalary = 0;
                foreach (var task in project.Tasks)
                {
                    int currentDate = DateTime.Now.Day;
                    int startDate = task.CreatedTime.Day;
                    int numberOfDays = currentDate - startDate;
                    ApplicationUser user = db.Users.First(x => x.Email == task.Developer.Title);
                    int salary = numberOfDays * (int)user.Salary;
                    developerSalary += salary;
                }

                amountSpent = developerSalary + managerSalary;

                int totalBudget = project.Budget;
                if (amountSpent > totalBudget)
                {
                    projects.Add(project);
                }
                ViewBag.TotalAmountSpent = amountSpent;
            }
            return View(projects);
        }
    }
}
