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
    public class ElectionsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public ElectionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Elections
        public async Task<IActionResult> Index()
        {
              return View(await _context.Elections.ToListAsync());
        }

        // GET: Elections/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Elections == null)
            {
                return NotFound();
            }

            var election = await _context.Elections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (election == null)
            {
                return NotFound();
            }

            return View(election);
        }

        // GET: Elections/Create
        public IActionResult Create()
        {
			ViewBag.ElectionTypes = new SelectList(_context.ElectionTypes, "Id", "Name");
			ViewBag.Countries = new SelectList(_context.Countries, "Id", "Name");
			return View();
        }

        // POST: Elections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,EventDate,ElectionTypeId, CountryId")] ElectionCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var electionType = await _context.ElectionTypes.FindAsync(model.ElectionTypeId);
                if(electionType == null)
                {
                    DisplayError("Invalid Election Type Selected");
                    return View(model);
                }
                var country = _context.Countries.Find(model.CountryId);
                if(country == null)
                {
                    DisplayError("Invalid Country Passed");
                    return View(model);
                }
                var election = new Election()
                {
                     Country = country,
                     ElectionType = electionType,
                     EventDate = model.EventDate,
                     Name = model.Name,
                     Description = model.Description
                };
                _context.Add(election);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Elections/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Elections == null)
            {
                return NotFound();
            }

            var election = await _context.Elections.FindAsync(id);
            if (election == null)
            {
                return NotFound();
            }
            return View(election);
        }

        // POST: Elections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Name,Description,EventDate,Id,CodeId,Active,Deleted,CreatedAt,UpdatedAt")] Election election)
        {
            if (id != election.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(election);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElectionExists(election.Id))
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
            return View(election);
        }

        // GET: Elections/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Elections == null)
            {
                return NotFound();
            }

            var election = await _context.Elections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (election == null)
            {
                return NotFound();
            }

            return View(election);
        }

        // POST: Elections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Elections == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Election'  is null.");
            }
            var election = await _context.Elections.FindAsync(id);
            if (election != null)
            {
                _context.Elections.Remove(election);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElectionExists(long id)
        {
          return _context.Elections.Any(e => e.Id == id);
        }
    }
}
