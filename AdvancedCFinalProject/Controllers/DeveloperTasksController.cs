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

namespace AdvancedCFinalProject.Controllers
{
    public class DeveloperTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeveloperTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DeveloperTasks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tasks.Include(d => d.Developer);
            ViewBag.NumOfNotifications = _context.Notification.Where(n => n.TaskId != null).Count();
            return View(await applicationDbContext.ToListAsync());            
        }
        public async Task<IActionResult> ShowingNotification()
        {
            var AllNotifications = _context.Notification.Where(n =>n.TaskId != null);
            return View(AllNotifications);
            
        }

        // GET: DeveloperTasks/Details/5
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

        // GET: DeveloperTasks/Create
        public IActionResult MakingNotification()
        {
            return View();
        }
        [HttpPost]
        public IActionResult MakingNotification(int count)
        {
            var Alltask = _context.Tasks;
            foreach (var task in Alltask)
            {
                var notificationWithtask = _context.Notification.Where(x => x.TaskId == task.TaskId);
                var today = DateTime.Today;
                var deadline = task.Deadline;
                if ((today - deadline).Days <= 1)
                { 
                    if(!notificationWithtask.Any(n => n.Content == $"{task.Title} deadline remained under 1 day"))
                    {
                        var newNotification = new Notification();
                        newNotification.Content = $"{task.Title} deadline remained under 1 day";
                        newNotification.TaskId = task.TaskId;
                        _context.Notification.Add(newNotification);                        

                    }                   
                   
                }
                
            }
            _context.SaveChanges();
            return RedirectToAction("Index");            
        }
        public IActionResult Create()
        {
            ViewData["DeveloperId"] = new SelectList(_context.Developer, "DeveloperId", "DeveloperId");
            return View();
        }
        

        // POST: DeveloperTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskId,Title,CompletionRate,IsComplete,Priority,Deadline,DeveloperId")] DeveloperTask developerTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(developerTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeveloperId"] = new SelectList(_context.Developer, "DeveloperId", "DeveloperId", developerTask.DeveloperId);
            return View(developerTask);
        }

        // GET: DeveloperTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developerTask = await _context.Tasks.FindAsync(id);
            if (developerTask == null)
            {
                return NotFound();
            }
            ViewData["DeveloperId"] = new SelectList(_context.Developer, "DeveloperId", "DeveloperId", developerTask.DeveloperId);
            return View(developerTask);
        }

        // POST: DeveloperTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskId,Title,CompletionRate,IsComplete,Priority,Deadline,DeveloperId")] DeveloperTask developerTask)
        {
            if (id != developerTask.TaskId)
            {
                return NotFound();
            }

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

        // GET: DeveloperTasks/Delete/5
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

        // POST: DeveloperTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var developerTask = await _context.Tasks.FindAsync(id);
            _context.Tasks.Remove(developerTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeveloperTaskExists(int id)
        {
            return _context.Tasks.Any(e => e.TaskId == id);
        }
    }
}
