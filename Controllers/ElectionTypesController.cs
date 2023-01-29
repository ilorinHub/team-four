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
using Microsoft.AspNetCore.Authorization;

namespace ElectionWeb.Controllers
{
	public class ElectionTypesController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public ElectionTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ElectionTypes
        public async Task<IActionResult> Index()
        {
              return View(await _context.ElectionTypes.ToListAsync());
        }

        // GET: ElectionTypes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.ElectionTypes == null)
            {
                return NotFound();
            }

            var electionType = await _context.ElectionTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (electionType == null)
            {
                return NotFound();
            }

            return View(electionType);
        }

        // GET: ElectionTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ElectionTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] ElectionTypeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var electionType = new ElectionType
                {
                    Name = model.Name,
                    Description = model.Description
                };
                var nameExist = _context.ElectionTypes.Any(m => m.Name == model.Name);
                if (nameExist)
                {
                    DisplayError("Name Already In Use");
                    return View(model);
                }
                _context.Add(electionType);
                await _context.SaveChangesAsync();
                DisplayMessage("Election Type Created Successfully");
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: ElectionTypes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.ElectionTypes == null)
            {
                return NotFound();
            }

            var electionType = await _context.ElectionTypes.FindAsync(id);
            if (electionType == null)
            {
                return NotFound();
            }
            return View(electionType);
        }

        // POST: ElectionTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Name,Description,Id,CodeId,Active,Deleted,CreatedAt,UpdatedAt")] ElectionType electionType)
        {
            if (id != electionType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(electionType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElectionTypeExists(electionType.Id))
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
            return View(electionType);
        }

        // GET: ElectionTypes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.ElectionTypes == null)
            {
                return NotFound();
            }

            var electionType = await _context.ElectionTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (electionType == null)
            {
                return NotFound();
            }

            return View(electionType);
        }

        // POST: ElectionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.ElectionTypes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ElectionType'  is null.");
            }
            var electionType = await _context.ElectionTypes.FindAsync(id);
            if (electionType != null)
            {
                _context.ElectionTypes.Remove(electionType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElectionTypeExists(long id)
        {
          return _context.ElectionTypes.Any(e => e.Id == id);
        }
    }
}
