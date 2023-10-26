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
    public class ProjectViolationTypesController : Controller
    {
        private readonly ModelContext _context;

        public ProjectViolationTypesController(ModelContext context)
        {
            _context = context;
        }

        // GET: ProjectViolationTypes
        public async Task<IActionResult> Index()
        {
              return _context.ProjectViolationTypes != null ? 
                          View(await _context.ProjectViolationTypes.ToListAsync()) :
                          Problem("Entity set 'ModelContext.ProjectViolationTypes'  is null.");
        }

        // GET: ProjectViolationTypes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.ProjectViolationTypes == null)
            {
                return NotFound();
            }

            var projectViolationType = await _context.ProjectViolationTypes
                .FirstOrDefaultAsync(m => m.Violationtypeid == id);
            if (projectViolationType == null)
            {
                return NotFound();
            }

            return View(projectViolationType);
        }

        // GET: ProjectViolationTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProjectViolationTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Violationtypeid,Name,Fine")] ProjectViolationType projectViolationType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectViolationType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectViolationType);
        }

        // GET: ProjectViolationTypes/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.ProjectViolationTypes == null)
            {
                return NotFound();
            }

            var projectViolationType = await _context.ProjectViolationTypes.FindAsync(id);
            if (projectViolationType == null)
            {
                return NotFound();
            }
            return View(projectViolationType);
        }

        // POST: ProjectViolationTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Violationtypeid,Name,Fine")] ProjectViolationType projectViolationType)
        {
            if (id != projectViolationType.Violationtypeid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectViolationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectViolationTypeExists(projectViolationType.Violationtypeid))
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
            return View(projectViolationType);
        }

        // GET: ProjectViolationTypes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.ProjectViolationTypes == null)
            {
                return NotFound();
            }

            var projectViolationType = await _context.ProjectViolationTypes
                .FirstOrDefaultAsync(m => m.Violationtypeid == id);
            if (projectViolationType == null)
            {
                return NotFound();
            }

            return View(projectViolationType);
        }

        // POST: ProjectViolationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.ProjectViolationTypes == null)
            {
                return Problem("Entity set 'ModelContext.ProjectViolationTypes'  is null.");
            }
            var projectViolationType = await _context.ProjectViolationTypes.FindAsync(id);
            if (projectViolationType != null)
            {
                _context.ProjectViolationTypes.Remove(projectViolationType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectViolationTypeExists(decimal id)
        {
          return (_context.ProjectViolationTypes?.Any(e => e.Violationtypeid == id)).GetValueOrDefault();
        }
    }
}
