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
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskController(ApplicationDbContext context)
        {
            _context = context;
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
        public IActionResult Create()
        {
            ViewData["DeveloperId"] = new SelectList(_context.Developer, "DeveloperId", "DeveloperId");
            return View();
        }

        // POST: Task/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskId,Title,CompletionRate,IsComplete,Priority,DeveloperId")] DeveloperTask developerTask)
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

        // GET: Task/Edit/5
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
