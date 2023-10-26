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
    public class ProjectUsersController : Controller
    {
        private readonly ModelContext _context;

        public ProjectUsersController(ModelContext context)
        {
            _context = context;
        }

        // GET: ProjectUsers
        public async Task<IActionResult> Index()
        {
              return _context.ProjectUsers != null ? 
                          View(await _context.ProjectUsers.ToListAsync()) :
                          Problem("Entity set 'ModelContext.ProjectUsers'  is null.");
        }

        // GET: ProjectUsers/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.ProjectUsers == null)
            {
                return NotFound();
            }

            var projectUser = await _context.ProjectUsers
                .FirstOrDefaultAsync(m => m.Userid == id);
            if (projectUser == null)
            {
                return NotFound();
            }

            return View(projectUser);
        }

        // GET: ProjectUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProjectUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Userid,Fname,Lname,Birthdate,Phonenumber,Email,Imgpath,Address")] ProjectUser projectUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectUser);
        }

        // GET: ProjectUsers/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.ProjectUsers == null)
            {
                return NotFound();
            }

            var projectUser = await _context.ProjectUsers.FindAsync(id);
            if (projectUser == null)
            {
                return NotFound();
            }
            return View(projectUser);
        }

        // POST: ProjectUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Userid,Fname,Lname,Birthdate,Phonenumber,Email,Imgpath,Address")] ProjectUser projectUser)
        {
            if (id != projectUser.Userid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectUserExists(projectUser.Userid))
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
            return View(projectUser);
        }

        // GET: ProjectUsers/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.ProjectUsers == null)
            {
                return NotFound();
            }

            var projectUser = await _context.ProjectUsers
                .FirstOrDefaultAsync(m => m.Userid == id);
            if (projectUser == null)
            {
                return NotFound();
            }

            return View(projectUser);
        }

        // POST: ProjectUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.ProjectUsers == null)
            {
                return Problem("Entity set 'ModelContext.ProjectUsers'  is null.");
            }
            var projectUser = await _context.ProjectUsers.FindAsync(id);
            if (projectUser != null)
            {
                _context.ProjectUsers.Remove(projectUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectUserExists(decimal id)
        {
          return (_context.ProjectUsers?.Any(e => e.Userid == id)).GetValueOrDefault();
        }
    }
}
