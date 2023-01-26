using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectionWeb.Data;
using ElectionWeb.Models;
using Microsoft.AspNetCore.Identity;
using ElectionWeb.Models.ViewModels;

namespace ElectionWeb.Controllers
{
    public class AspirantsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AspirantsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Aspirants
        public IActionResult Index()
        {
            
            var aspirants = _context.Aspirants.Include(x=>x.Party).Include(x=>x.User).Include(x=>x.Election).ToList().Select(x => new AspirantViewModel
            {
                Id = x.Id,
                PartyId = x.Party.Id,
                Party = x.Party.Name,
                Election = x.Election.Name,
                ElectionId = x.Election.Id,
                User = x.User.UserName
            }).ToList();
            var viewModel = new AspirantIndexViewModel
            {
                 Aspirants = aspirants
            };
            return View(viewModel);
        }

        // GET: Aspirants/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Aspirants == null)
            {
                return NotFound();
            }
            var aspirant = await _context.Aspirants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspirant == null)
            {
                return NotFound();
            }
            return View(aspirant);
        }

        // GET: Aspirants/Create
        public IActionResult Create()
        {
            ViewBag.Elections = new SelectList(_context.Elections, "Id", "Name");
            ViewBag.Parties = new SelectList(_context.Parties, "Id", "Name");
            return View();
        }

        // POST: Aspirants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PartyId, ElectionId ,Email")] AspirantCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    DisplayError("User not Found");
                    return View(model);
                }
                var election = await _context.Elections.FindAsync(model.ElectionId);
                var party = await _context.Parties.FindAsync(model.PartyId);
                if (election == null || party == null)
                {
                    DisplayError("Invalid Party or Election Passed");
                    return View(model);
                }
                var contestantExist = _context.Aspirants.Include(x => x.Election).Include(x => x.Party).Any(x => x.Election.Id == model.ElectionId && x.Party.Id == model.PartyId);
                if (contestantExist)
                {
                    DisplayError("This Party already has a candidate Registered");
                    return View(model);
                }
                var aspirant = new Aspirant
                {
                    Election = election,
                    Party = party,
                    User = user
                };
                _context.Add(aspirant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Aspirants/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Aspirants == null)
            {
                return NotFound();
            }

            var aspirant = await _context.Aspirants.FindAsync(id);
            if (aspirant == null)
            {
                return NotFound();
            }
            return View(aspirant);
        }

        // POST: Aspirants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,CodeId,Active,Deleted,CreatedAt,UpdatedAt")] Aspirant aspirant)
        {
            if (id != aspirant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspirant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspirantExists(aspirant.Id))
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
            return View(aspirant);
        }

        // GET: Aspirants/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Aspirants == null)
            {
                return NotFound();
            }

            var aspirant = await _context.Aspirants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspirant == null)
            {
                return NotFound();
            }

            return View(aspirant);
        }

        // POST: Aspirants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Aspirants == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Aspirant'  is null.");
            }
            var aspirant = await _context.Aspirants.FindAsync(id);
            if (aspirant != null)
            {
                _context.Aspirants.Remove(aspirant);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AspirantExists(long id)
        {
            return _context.Aspirants.Any(e => e.Id == id);
        }
    }
}
