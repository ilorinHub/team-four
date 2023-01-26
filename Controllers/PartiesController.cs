﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectionWeb.Data;
using ElectionWeb.Models;
using ElectionWeb.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace ElectionWeb.Controllers
{
    public class PartiesController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public PartiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Parties
        public async Task<IActionResult> Index()
        {
              return View(await _context.Parties.ToListAsync());
        }

        // GET: Parties/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Parties == null)
            {
                return NotFound();
            }

            var party = await _context.Parties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }

        // GET: Parties/Create
        public IActionResult Create()
        {
			ViewBag.Countries = new SelectList(_context.Countries, "Id", "Name");
			return View();
        }

        // POST: Parties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Logo,CountryId")] PartyCreateViewModel model)
        {
            var nameExist = _context.Parties.Include(x => x.Country).Any(x => x.Name.ToLower().Trim() == model.Name.ToLower().Trim() && x.Country.Id == model.CountryId);
            if (nameExist)
            {
                DisplayError("Name Already In Use");
                return View(model);
            }

            var logo = model.Logo;
            var logoName = model.Logo.FileName;
            
            var extension = Path.GetExtension(logoName);
            List<string> acceptedFormats = new List<string>() {".png", ".jpg", ".jpeg"};
            if (!acceptedFormats.Contains(extension))
            {
                var message = string.Format("{0}, not permitted on the method", extension);
                DisplayError(message);
                return View(model);
            }

			var fileName = model.Name + Path.GetFileName(logoName);
			string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
			var stream = new FileStream(uploadpath, FileMode.Create);
			await model.Logo.CopyToAsync(stream);
			var country = _context.Countries.Find(model.CountryId);
            if(country == null)
            {
                DisplayError("Country not valid");
            }
            var party = new Party
            {
               Name = model.Name,
               Description = model.Description,
               Logo = fileName,
               Country = country
            };
            if (ModelState.IsValid)
            {
                _context.Add(party);
                await _context.SaveChangesAsync();
                DisplayMessage("Party Created Successfully");
                return RedirectToAction(nameof(Index));
            }
            return View(party);
        }

        // GET: Parties/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Parties == null)
            {
                return NotFound();
            }

            var party = await _context.Parties.FindAsync(id);
            if (party == null)
            {
                return NotFound();
            }
            return View(party);
        }

        // POST: Parties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Name,Description,Logo,Id,CodeId,Active,Deleted,CreatedAt,UpdatedAt")] Party party)
        {
            if (id != party.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(party);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartyExists(party.Id))
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
            return View(party);
        }

        // GET: Parties/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Parties == null)
            {
                return NotFound();
            }

            var party = await _context.Parties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }

        // POST: Parties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Parties == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Party'  is null.");
            }
            var party = await _context.Parties.FindAsync(id);
            if (party != null)
            {
                _context.Parties.Remove(party);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartyExists(long id)
        {
          return _context.Parties.Any(e => e.Id == id);
        }
    }
}
