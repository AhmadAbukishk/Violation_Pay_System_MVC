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
    public class ProjectFactSectionsController : Controller
    {
        private readonly ModelContext _context;

        public ProjectFactSectionsController(ModelContext context)
        {
            _context = context;
        }

        // GET: ProjectFactSections
        public async Task<IActionResult> Index()
        {
              return _context.ProjectFactSections != null ? 
                          View(await _context.ProjectFactSections.ToListAsync()) :
                          Problem("Entity set 'ModelContext.ProjectFactSections'  is null.");
        }

        // GET: ProjectFactSections/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.ProjectFactSections == null)
            {
                return NotFound();
            }

            var projectFactSection = await _context.ProjectFactSections
                .FirstOrDefaultAsync(m => m.Factid == id);
            if (projectFactSection == null)
            {
                return NotFound();
            }

            return View(projectFactSection);
        }

        // GET: ProjectFactSections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProjectFactSections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Factid,Title,Content")] ProjectFactSection projectFactSection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectFactSection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectFactSection);
        }

        // GET: ProjectFactSections/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.ProjectFactSections == null)
            {
                return NotFound();
            }

            var projectFactSection = await _context.ProjectFactSections.FindAsync(id);
            if (projectFactSection == null)
            {
                return NotFound();
            }
            return View(projectFactSection);
        }

        // POST: ProjectFactSections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Factid,Title,Content")] ProjectFactSection projectFactSection)
        {
            if (id != projectFactSection.Factid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectFactSection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectFactSectionExists(projectFactSection.Factid))
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
            return View(projectFactSection);
        }

        // GET: ProjectFactSections/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.ProjectFactSections == null)
            {
                return NotFound();
            }

            var projectFactSection = await _context.ProjectFactSections
                .FirstOrDefaultAsync(m => m.Factid == id);
            if (projectFactSection == null)
            {
                return NotFound();
            }

            return View(projectFactSection);
        }

        // POST: ProjectFactSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.ProjectFactSections == null)
            {
                return Problem("Entity set 'ModelContext.ProjectFactSections'  is null.");
            }
            var projectFactSection = await _context.ProjectFactSections.FindAsync(id);
            if (projectFactSection != null)
            {
                _context.ProjectFactSections.Remove(projectFactSection);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectFactSectionExists(decimal id)
        {
          return (_context.ProjectFactSections?.Any(e => e.Factid == id)).GetValueOrDefault();
        }
    }
}
