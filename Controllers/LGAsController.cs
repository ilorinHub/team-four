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
    public class LGAsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public LGAsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LGAs
        public async Task<IActionResult> Index()
        {
              return View(await _context.lGAs.ToListAsync());
        }

        // GET: LGAs/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.lGAs == null)
            {
                return NotFound();
            }

            var lGA = await _context.lGAs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lGA == null)
            {
                return NotFound();
            }

            return View(lGA);
        }

        // GET: LGAs/Create
        public IActionResult Create()
        {
            ViewBag.Constituencies = new SelectList(_context.Constituencies, "Id", "Name");
            return View();
        }

        // POST: LGAs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,ConstituencyId")] LGACreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var constituency = _context.Constituencies.Find(model.ConstituencyId);
                var nameExist = _context.lGAs.Include(x => x.Constituency).Any(x => x.Name.ToLower().Trim() == model.Name.ToLower().Trim() && x.Constituency.Id == model.ConstituencyId);
                if (nameExist)
                {
                    DisplayError("Name Already In Use");
                    return View(model);
                }
                var lga = new LGA
                {
                     Constituency = constituency,
                      Description = model.Description,
                      Name = model.Name
                };
                _context.Add(lga);
                await _context.SaveChangesAsync();
                DisplayMessage("Saved Successfully");
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: LGAs/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.lGAs == null)
            {
                return NotFound();
            }

            var lGA = await _context.lGAs.FindAsync(id);
            if (lGA == null)
            {
                return NotFound();
            }
            return View(lGA);
        }

        // POST: LGAs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Name,Description,Id,CodeId,Active,Deleted,CreatedAt,UpdatedAt")] LGA lGA)
        {
            if (id != lGA.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lGA);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LGAExists(lGA.Id))
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
            return View(lGA);
        }

        // GET: LGAs/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.lGAs == null)
            {
                return NotFound();
            }

            var lGA = await _context.lGAs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lGA == null)
            {
                return NotFound();
            }

            return View(lGA);
        }

        // POST: LGAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.lGAs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.lGAs'  is null.");
            }
            var lGA = await _context.lGAs.FindAsync(id);
            if (lGA != null)
            {
                _context.lGAs.Remove(lGA);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LGAExists(long id)
        {
          return _context.lGAs.Any(e => e.Id == id);
        }
    }
}
