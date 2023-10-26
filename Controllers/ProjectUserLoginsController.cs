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
    public class ProjectUserLoginsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _IwebHostEnvironment;

        public ProjectUserLoginsController(ModelContext context, IWebHostEnvironment iwebHostEnvironment)
        {
            _context = context;
            _IwebHostEnvironment = iwebHostEnvironment;
        }

        // GET: ProjectUserLogins
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.ProjectUserLogins.Include(p => p.Role).Include(p => p.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: ProjectUserLogins/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.ProjectUserLogins == null)
            {
                return NotFound();
            }

            var projectUserLogin = await _context.ProjectUserLogins
                .Include(p => p.Role)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Loginid == id);
            if (projectUserLogin == null)
            {
                return NotFound();
            }

            return View(projectUserLogin);
        }

        // GET: ProjectUserLogins/Create
        public IActionResult Create()
        {
            ViewData["Roleid"] = new SelectList(_context.ProjectRoles, "Roleid", "Roleid");
            ViewData["Userid"] = new SelectList(_context.ProjectUsers, "Userid", "Userid");
            return View();
        }

        // POST: ProjectUserLogins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Loginid,Username,Password,Userid,Roleid")] ProjectUserLogin projectUserLogin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectUserLogin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Roleid"] = new SelectList(_context.ProjectRoles, "Roleid", "Roleid", projectUserLogin.Roleid);
            ViewData["Userid"] = new SelectList(_context.ProjectUsers, "Userid", "Userid", projectUserLogin.Userid);
            return View(projectUserLogin);
        }

        // GET: ProjectUserLogins/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.ProjectUserLogins == null)
            {
                return NotFound();
            }

            var projectUserLogin = await _context.ProjectUserLogins.FindAsync(id);
            if (projectUserLogin == null)
            {
                return NotFound();
            }
            ViewData["Roleid"] = new SelectList(_context.ProjectRoles, "Roleid", "Roleid", projectUserLogin.Roleid);
            ViewData["Userid"] = new SelectList(_context.ProjectUsers, "Userid", "Userid", projectUserLogin.Userid);
            return View(projectUserLogin);
        }

        // POST: ProjectUserLogins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Loginid,Username,Password,Userid,Roleid")] ProjectUserLogin projectUserLogin)
        {
            if (id != projectUserLogin.Loginid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectUserLogin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectUserLoginExists(projectUserLogin.Loginid))
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
            ViewData["Roleid"] = new SelectList(_context.ProjectRoles, "Roleid", "Roleid", projectUserLogin.Roleid);
            ViewData["Userid"] = new SelectList(_context.ProjectUsers, "Userid", "Userid", projectUserLogin.Userid);
            return View(projectUserLogin);
        }

        // GET: ProjectUserLogins/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.ProjectUserLogins == null)
            {
                return NotFound();
            }

            var projectUserLogin = await _context.ProjectUserLogins
                .Include(p => p.Role)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Loginid == id);
            if (projectUserLogin == null)
            {
                return NotFound();
            }

            return View(projectUserLogin);
        }

        // POST: ProjectUserLogins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.ProjectUserLogins == null)
            {
                return Problem("Entity set 'ModelContext.ProjectUserLogins'  is null.");
            }
            var projectUserLogin = await _context.ProjectUserLogins.FindAsync(id);
            if (projectUserLogin != null)
            {
                _context.ProjectUserLogins.Remove(projectUserLogin);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectUserLoginExists(decimal id)
        {
          return (_context.ProjectUserLogins?.Any(e => e.Loginid == id)).GetValueOrDefault();
        }
    }
}
