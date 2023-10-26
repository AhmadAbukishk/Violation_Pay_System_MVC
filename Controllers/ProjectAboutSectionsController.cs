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
    public class ProjectAboutSectionsController : Controller
    {
        private readonly ModelContext _context;

        public ProjectAboutSectionsController(ModelContext context)
        {
            _context = context;
        }

        // GET: ProjectAboutSections
        public async Task<IActionResult> Index()
        {
              return _context.ProjectAboutSections != null ? 
                          View(await _context.ProjectAboutSections.ToListAsync()) :
                          Problem("Entity set 'ModelContext.ProjectAboutSections'  is null.");
        }

        // GET: ProjectAboutSections/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.ProjectAboutSections == null)
            {
                return NotFound();
            }

            var projectAboutSection = await _context.ProjectAboutSections
                .FirstOrDefaultAsync(m => m.Aboutid == id);
            if (projectAboutSection == null)
            {
                return NotFound();
            }

            return View(projectAboutSection);
        }

        // GET: ProjectAboutSections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProjectAboutSections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Aboutid,Header,Subheader,Imgpath")] ProjectAboutSection projectAboutSection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectAboutSection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectAboutSection);
        }

        // GET: ProjectAboutSections/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.ProjectAboutSections == null)
            {
                return NotFound();
            }

            var projectAboutSection = await _context.ProjectAboutSections.FindAsync(id);
            if (projectAboutSection == null)
            {
                return NotFound();
            }
            return View(projectAboutSection);
        }

        // POST: ProjectAboutSections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Aboutid,Header,Subheader,Imgpath")] ProjectAboutSection projectAboutSection)
        {
            if (id != projectAboutSection.Aboutid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectAboutSection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectAboutSectionExists(projectAboutSection.Aboutid))
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
            return View(projectAboutSection);
        }

        // GET: ProjectAboutSections/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.ProjectAboutSections == null)
            {
                return NotFound();
            }

            var projectAboutSection = await _context.ProjectAboutSections
                .FirstOrDefaultAsync(m => m.Aboutid == id);
            if (projectAboutSection == null)
            {
                return NotFound();
            }

            return View(projectAboutSection);
        }

        // POST: ProjectAboutSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.ProjectAboutSections == null)
            {
                return Problem("Entity set 'ModelContext.ProjectAboutSections'  is null.");
            }
            var projectAboutSection = await _context.ProjectAboutSections.FindAsync(id);
            if (projectAboutSection != null)
            {
                _context.ProjectAboutSections.Remove(projectAboutSection);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectAboutSectionExists(decimal id)
        {
          return (_context.ProjectAboutSections?.Any(e => e.Aboutid == id)).GetValueOrDefault();
        }
    }
}
