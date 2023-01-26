using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectionWeb.Data;
using ElectionWeb.Models;

namespace ElectionWeb.Controllers
{
    public class ReportCasesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportCasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ReportCases
        public async Task<IActionResult> Index()
        {
              return View(await _context.ReportCase.ToListAsync());
        }

        // GET: ReportCases/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.ReportCase == null)
            {
                return NotFound();
            }

            var reportCase = await _context.ReportCase
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reportCase == null)
            {
                return NotFound();
            }

            return View(reportCase);
        }

        // GET: ReportCases/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ReportCases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Case,Latitude,Longitude,Note,EventDate,Photo,Id,CodeId,Active,Deleted,CreatedAt,UpdatedAt")] ReportCase reportCase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reportCase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reportCase);
        }

        // GET: ReportCases/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.ReportCase == null)
            {
                return NotFound();
            }

            var reportCase = await _context.ReportCase.FindAsync(id);
            if (reportCase == null)
            {
                return NotFound();
            }
            return View(reportCase);
        }

        // POST: ReportCases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Case,Latitude,Longitude,Note,EventDate,Photo,Id,CodeId,Active,Deleted,CreatedAt,UpdatedAt")] ReportCase reportCase)
        {
            if (id != reportCase.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reportCase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportCaseExists(reportCase.Id))
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
            return View(reportCase);
        }

        // GET: ReportCases/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.ReportCase == null)
            {
                return NotFound();
            }

            var reportCase = await _context.ReportCase
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reportCase == null)
            {
                return NotFound();
            }

            return View(reportCase);
        }

        // POST: ReportCases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.ReportCase == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ReportCase'  is null.");
            }
            var reportCase = await _context.ReportCase.FindAsync(id);
            if (reportCase != null)
            {
                _context.ReportCase.Remove(reportCase);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportCaseExists(long id)
        {
          return _context.ReportCase.Any(e => e.Id == id);
        }
    }
}
