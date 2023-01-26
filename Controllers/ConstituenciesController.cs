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
    public class ConstituenciesController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public ConstituenciesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Constituencies
        public async Task<IActionResult> Index()
        {
              return View(await _context.Constituencies.ToListAsync());
        }

        // GET: Constituencies/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Constituencies == null)
            {
                return NotFound();
            }

            var constituency = await _context.Constituencies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (constituency == null)
            {
                return NotFound();
            }

            return View(constituency);
        }

        // GET: Constituencies/Create
        public IActionResult Create()
        {
            ViewBag.StateRegions = new SelectList(_context.StateRegions, "Id", "Name");
            return View();
        }

        // POST: Constituencies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,StateRegionId")] ConstituencyCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var state = _context.StateRegions.Find(model.StateRegionId);
                var nameExist = _context.Constituencies.Include(x => x.StateRegion).Any(x => x.Name.ToLower().Trim() == model.Name.ToLower().Trim() && x.StateRegion.Id == model.StateRegionId);
                if (nameExist)
                {
                    DisplayError("Name Already In Use");
                    return View(model);
                }
                var constituency = new Constituency()
                {
                    Description = model.Description,
                    Name = model.Name,
                    StateRegion = state
                };
                _context.Add(constituency);
                await _context.SaveChangesAsync();
                DisplayMessage("Successful Operation");
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Constituencies/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Constituencies == null)
            {
                return NotFound();
            }

            var constituency = await _context.Constituencies.FindAsync(id);
            if (constituency == null)
            {
                return NotFound();
            }
            return View(constituency);
        }

        // POST: Constituencies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Name,Description,Id,CodeId,Active,Deleted,CreatedAt,UpdatedAt")] Constituency constituency)
        {
            if (id != constituency.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(constituency);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConstituencyExists(constituency.Id))
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
            return View(constituency);
        }

        // GET: Constituencies/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Constituencies == null)
            {
                return NotFound();
            }

            var constituency = await _context.Constituencies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (constituency == null)
            {
                return NotFound();
            }

            return View(constituency);
        }

        // POST: Constituencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Constituencies == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Constituencies'  is null.");
            }
            var constituency = await _context.Constituencies.FindAsync(id);
            if (constituency != null)
            {
                _context.Constituencies.Remove(constituency);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConstituencyExists(long id)
        {
          return _context.Constituencies.Any(e => e.Id == id);
        }
    }
}
