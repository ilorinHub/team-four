using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectionWeb.Data;
using ElectionWeb.Models;
using ElectionWeb.Models.ViewModels;

namespace ElectionWeb.Controllers
{
    public class PollingUnitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PollingUnitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PollingUnits
        public async Task<IActionResult> Index()
        {
              return View(await _context.PollingUnits.ToListAsync());
        }

        // GET: PollingUnits/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.PollingUnits == null)
            {
                return NotFound();
            }

            var pollingUnit = await _context.PollingUnits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pollingUnit == null)
            {
                return NotFound();
            }

            return View(pollingUnit);
        }

        // GET: PollingUnits/Create
        public IActionResult Create()
        {
            ViewBag.Wards = new SelectList(_context.Wards, "Id", "Name");
            return View();
        }

        // POST: PollingUnits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,WardId")] PollingUnitCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ward = _context.Wards.Find(model.WardId);
                var pollingUnit = new PollingUnit
                {
                    Name = model.Name,
                    Description = model.Description,
                    Ward = ward
                };
                _context.Add(pollingUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: PollingUnits/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.PollingUnits == null)
            {
                return NotFound();
            }

            var pollingUnit = await _context.PollingUnits.FindAsync(id);
            if (pollingUnit == null)
            {
                return NotFound();
            }
            return View(pollingUnit);
        }

        // POST: PollingUnits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Name,Description,Id,CodeId,Active,Deleted,CreatedAt,UpdatedAt")] PollingUnit pollingUnit)
        {
            if (id != pollingUnit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pollingUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PollingUnitExists(pollingUnit.Id))
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
            return View(pollingUnit);
        }

        // GET: PollingUnits/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.PollingUnits == null)
            {
                return NotFound();
            }

            var pollingUnit = await _context.PollingUnits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pollingUnit == null)
            {
                return NotFound();
            }

            return View(pollingUnit);
        }

        // POST: PollingUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.PollingUnits == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PollingUnits'  is null.");
            }
            var pollingUnit = await _context.PollingUnits.FindAsync(id);
            if (pollingUnit != null)
            {
                _context.PollingUnits.Remove(pollingUnit);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PollingUnitExists(long id)
        {
          return _context.PollingUnits.Any(e => e.Id == id);
        }
    }
}
