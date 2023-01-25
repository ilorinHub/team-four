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
using static ElectionWeb.Models.ViewModels.StateRegionViewModel;

namespace ElectionWeb.Controllers
{
    public class StateRegionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StateRegionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StateRegions
        public async Task<IActionResult> Index()
        {
              return View(await _context.StateRegions.ToListAsync());
        }

        // GET: StateRegions/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.StateRegions == null)
            {
                return NotFound();
            }

            var stateRegion = await _context.StateRegions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stateRegion == null)
            {
                return NotFound();
            }

            return View(stateRegion);
        }

        // GET: StateRegions/Create
        public IActionResult Create()
        {
            ViewBag.Countries = new SelectList(_context.Countries, "Id", "Name");
            return View();
        }

        // POST: StateRegions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,CountryId")] StateRegionCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var country =  await _context.Countries.FindAsync(model.CountryId);
                var stateRegion = new StateRegion()
                {
                    Country = country,
                    Name = model.Name,
                    Description = model.Description
                };
                _context.Add(stateRegion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: StateRegions/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.StateRegions == null)
            {
                return NotFound();
            }

            var stateRegion = await _context.StateRegions.FindAsync(id);
            if (stateRegion == null)
            {
                return NotFound();
            }
            return View(stateRegion);
        }

        // POST: StateRegions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Name,Description,Id,CodeId,Active,Deleted,CreatedAt,UpdatedAt")] StateRegion stateRegion)
        {
            if (id != stateRegion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stateRegion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StateRegionExists(stateRegion.Id))
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
            return View(stateRegion);
        }

        // GET: StateRegions/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.StateRegions == null)
            {
                return NotFound();
            }

            var stateRegion = await _context.StateRegions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stateRegion == null)
            {
                return NotFound();
            }

            return View(stateRegion);
        }

        // POST: StateRegions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.StateRegions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.StateRegions'  is null.");
            }
            var stateRegion = await _context.StateRegions.FindAsync(id);
            if (stateRegion != null)
            {
                _context.StateRegions.Remove(stateRegion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StateRegionExists(long id)
        {
          return _context.StateRegions.Any(e => e.Id == id);
        }
    }
}
