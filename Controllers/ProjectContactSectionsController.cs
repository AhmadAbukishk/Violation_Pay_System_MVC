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
    public class ProjectContactSectionsController : Controller
    {
        private readonly ModelContext _context;

        public ProjectContactSectionsController(ModelContext context)
        {
            _context = context;
        }

        // GET: ProjectContactSections
        public async Task<IActionResult> Index()
        {
              return _context.ProjectContactSections != null ? 
                          View(await _context.ProjectContactSections.ToListAsync()) :
                          Problem("Entity set 'ModelContext.ProjectContactSections'  is null.");
        }

        // GET: ProjectContactSections/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.ProjectContactSections == null)
            {
                return NotFound();
            }

            var projectContactSection = await _context.ProjectContactSections
                .FirstOrDefaultAsync(m => m.Contactid == id);
            if (projectContactSection == null)
            {
                return NotFound();
            }

            return View(projectContactSection);
        }

        // GET: ProjectContactSections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProjectContactSections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Contactid,Email,Phone,Address,Title,Content")] ProjectContactSection projectContactSection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectContactSection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectContactSection);
        }

        // GET: ProjectContactSections/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.ProjectContactSections == null)
            {
                return NotFound();
            }

            var projectContactSection = await _context.ProjectContactSections.FindAsync(id);
            if (projectContactSection == null)
            {
                return NotFound();
            }
            return View(projectContactSection);
        }

        // POST: ProjectContactSections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Contactid,Email,Phone,Address,Title,Content")] ProjectContactSection projectContactSection)
        {
            if (id != projectContactSection.Contactid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectContactSection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectContactSectionExists(projectContactSection.Contactid))
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
            return View(projectContactSection);
        }

        // GET: ProjectContactSections/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.ProjectContactSections == null)
            {
                return NotFound();
            }

            var projectContactSection = await _context.ProjectContactSections
                .FirstOrDefaultAsync(m => m.Contactid == id);
            if (projectContactSection == null)
            {
                return NotFound();
            }

            return View(projectContactSection);
        }

        // POST: ProjectContactSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.ProjectContactSections == null)
            {
                return Problem("Entity set 'ModelContext.ProjectContactSections'  is null.");
            }
            var projectContactSection = await _context.ProjectContactSections.FindAsync(id);
            if (projectContactSection != null)
            {
                _context.ProjectContactSections.Remove(projectContactSection);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectContactSectionExists(decimal id)
        {
          return (_context.ProjectContactSections?.Any(e => e.Contactid == id)).GetValueOrDefault();
        }
    }
}
