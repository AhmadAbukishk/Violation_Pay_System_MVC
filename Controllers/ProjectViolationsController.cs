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
    public class ProjectViolationsController : Controller
    {
        private readonly ModelContext _context;

        public ProjectViolationsController(ModelContext context)
        {
            _context = context;
        }

        // GET: ProjectViolations
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.ProjectViolations.Include(p => p.Car).Include(p => p.State).Include(p => p.Violationtype);
            return View(await modelContext.ToListAsync());
        }

        // GET: ProjectViolations/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.ProjectViolations == null)
            {
                return NotFound();
            }

            var projectViolation = await _context.ProjectViolations
                .Include(p => p.Car)
                .Include(p => p.State)
                .Include(p => p.Violationtype)
                .FirstOrDefaultAsync(m => m.Violationid == id);
            if (projectViolation == null)
            {
                return NotFound();
            }

            return View(projectViolation);
        }

        // GET: ProjectViolations/Create
        public IActionResult Create()
        {
            ViewData["Carid"] = new SelectList(_context.ProjectCars, "Carid", "Carid");
            ViewData["Stateid"] = new SelectList(_context.ProjectViolationStates, "Stateid", "Stateid");
            ViewData["Violationtypeid"] = new SelectList(_context.ProjectViolationTypes, "Violationtypeid", "Violationtypeid");
            return View();
        }

        // POST: ProjectViolations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Violationid,Violationdate,Location,Violationtypeid,Carid,Stateid")] ProjectViolation projectViolation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectViolation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Carid"] = new SelectList(_context.ProjectCars, "Carid", "Carid", projectViolation.Carid);
            ViewData["Stateid"] = new SelectList(_context.ProjectViolationStates, "Stateid", "Stateid", projectViolation.Stateid);
            ViewData["Violationtypeid"] = new SelectList(_context.ProjectViolationTypes, "Violationtypeid", "Violationtypeid", projectViolation.Violationtypeid);
            return View(projectViolation);
        }

        // GET: ProjectViolations/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.ProjectViolations == null)
            {
                return NotFound();
            }

            var projectViolation = await _context.ProjectViolations.FindAsync(id);
            if (projectViolation == null)
            {
                return NotFound();
            }
            ViewData["Carid"] = new SelectList(_context.ProjectCars, "Carid", "Carid", projectViolation.Carid);
            ViewData["Stateid"] = new SelectList(_context.ProjectViolationStates, "Stateid", "Stateid", projectViolation.Stateid);
            ViewData["Violationtypeid"] = new SelectList(_context.ProjectViolationTypes, "Violationtypeid", "Violationtypeid", projectViolation.Violationtypeid);
            return View(projectViolation);
        }

        // POST: ProjectViolations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Violationid,Violationdate,Location,Violationtypeid,Carid,Stateid")] ProjectViolation projectViolation)
        {
            if (id != projectViolation.Violationid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectViolation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectViolationExists(projectViolation.Violationid))
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
            ViewData["Carid"] = new SelectList(_context.ProjectCars, "Carid", "Carid", projectViolation.Carid);
            ViewData["Stateid"] = new SelectList(_context.ProjectViolationStates, "Stateid", "Stateid", projectViolation.Stateid);
            ViewData["Violationtypeid"] = new SelectList(_context.ProjectViolationTypes, "Violationtypeid", "Violationtypeid", projectViolation.Violationtypeid);
            return View(projectViolation);
        }

        // GET: ProjectViolations/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.ProjectViolations == null)
            {
                return NotFound();
            }

            var projectViolation = await _context.ProjectViolations
                .Include(p => p.Car)
                .Include(p => p.State)
                .Include(p => p.Violationtype)
                .FirstOrDefaultAsync(m => m.Violationid == id);
            if (projectViolation == null)
            {
                return NotFound();
            }

            return View(projectViolation);
        }

        // POST: ProjectViolations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.ProjectViolations == null)
            {
                return Problem("Entity set 'ModelContext.ProjectViolations'  is null.");
            }
            var projectViolation = await _context.ProjectViolations.FindAsync(id);
            if (projectViolation != null)
            {
                _context.ProjectViolations.Remove(projectViolation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectViolationExists(decimal id)
        {
          return (_context.ProjectViolations?.Any(e => e.Violationid == id)).GetValueOrDefault();
        }
    }
}
