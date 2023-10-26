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
    public class ProjectCarsController : Controller
    {
        private readonly ModelContext _context;

        public ProjectCarsController(ModelContext context)
        {
            _context = context;
        }

        // GET: ProjectCars
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.ProjectCars.Include(p => p.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: ProjectCars/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.ProjectCars == null)
            {
                return NotFound();
            }

            var projectCar = await _context.ProjectCars
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Carid == id);
            if (projectCar == null)
            {
                return NotFound();
            }

            return View(projectCar);
        }

        // GET: ProjectCars/Create
        public IActionResult Create()
        {
            ViewData["Userid"] = new SelectList(_context.ProjectUsers, "Userid", "Userid");
            return View();
        }

        // POST: ProjectCars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Carid,Brand,Model,Color,Platenumber,Userid")] ProjectCar projectCar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectCar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Userid"] = new SelectList(_context.ProjectUsers, "Userid", "Userid", projectCar.Userid);
            return View(projectCar);
        }

        // GET: ProjectCars/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.ProjectCars == null)
            {
                return NotFound();
            }

            var projectCar = await _context.ProjectCars.FindAsync(id);
            if (projectCar == null)
            {
                return NotFound();
            }
            ViewData["Userid"] = new SelectList(_context.ProjectUsers, "Userid", "Userid", projectCar.Userid);
            return View(projectCar);
        }

        // POST: ProjectCars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Carid,Brand,Model,Color,Platenumber,Userid")] ProjectCar projectCar)
        {
            if (id != projectCar.Carid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectCar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectCarExists(projectCar.Carid))
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
            ViewData["Userid"] = new SelectList(_context.ProjectUsers, "Userid", "Userid", projectCar.Userid);
            return View(projectCar);
        }

        // GET: ProjectCars/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.ProjectCars == null)
            {
                return NotFound();
            }

            var projectCar = await _context.ProjectCars
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Carid == id);
            if (projectCar == null)
            {
                return NotFound();
            }

            return View(projectCar);
        }

        // POST: ProjectCars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.ProjectCars == null)
            {
                return Problem("Entity set 'ModelContext.ProjectCars'  is null.");
            }
            var projectCar = await _context.ProjectCars.FindAsync(id);
            if (projectCar != null)
            {
                _context.ProjectCars.Remove(projectCar);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectCarExists(decimal id)
        {
          return (_context.ProjectCars?.Any(e => e.Carid == id)).GetValueOrDefault();
        }
    }
}
