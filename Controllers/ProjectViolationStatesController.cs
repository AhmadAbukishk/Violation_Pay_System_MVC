using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Traffic_Violation.Models;

namespace Traffic_Violation.Controllers
{
    public class ProjectViolationStatesController : Controller
    {
        private readonly ModelContext _context;

        public ProjectViolationStatesController(ModelContext context)
        {
            _context = context;
        }

        // GET: ProjectViolationStates
        public async Task<IActionResult> Index()
        {
              return _context.ProjectViolationStates != null ? 
                          View(await _context.ProjectViolationStates.ToListAsync()) :
                          Problem("Entity set 'ModelContext.ProjectViolationStates'  is null.");
        }

        // GET: ProjectViolationStates/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.ProjectViolationStates == null)
            {
                return NotFound();
            }

            var projectViolationState = await _context.ProjectViolationStates
                .FirstOrDefaultAsync(m => m.Stateid == id);
            if (projectViolationState == null)
            {
                return NotFound();
            }

            return View(projectViolationState);
        }

        // GET: ProjectViolationStates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProjectViolationStates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Stateid,Name")] ProjectViolationState projectViolationState)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectViolationState);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectViolationState);
        }

        // GET: ProjectViolationStates/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.ProjectViolationStates == null)
            {
                return NotFound();
            }

            var projectViolationState = await _context.ProjectViolationStates.FindAsync(id);
            if (projectViolationState == null)
            {
                return NotFound();
            }
            return View(projectViolationState);
        }

        // POST: ProjectViolationStates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Stateid,Name")] ProjectViolationState projectViolationState)
        {
            if (id != projectViolationState.Stateid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectViolationState);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectViolationStateExists(projectViolationState.Stateid))
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
            return View(projectViolationState);
        }

        // GET: ProjectViolationStates/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.ProjectViolationStates == null)
            {
                return NotFound();
            }

            var projectViolationState = await _context.ProjectViolationStates
                .FirstOrDefaultAsync(m => m.Stateid == id);
            if (projectViolationState == null)
            {
                return NotFound();
            }

            return View(projectViolationState);
        }

        // POST: ProjectViolationStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.ProjectViolationStates == null)
            {
                return Problem("Entity set 'ModelContext.ProjectViolationStates'  is null.");
            }
            var projectViolationState = await _context.ProjectViolationStates.FindAsync(id);
            if (projectViolationState != null)
            {
                _context.ProjectViolationStates.Remove(projectViolationState);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectViolationStateExists(decimal id)
        {
          return (_context.ProjectViolationStates?.Any(e => e.Stateid == id)).GetValueOrDefault();
        }
    }
}
