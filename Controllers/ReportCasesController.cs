using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectionWeb.Data;
using ElectionWeb.Models;
using ElectionWeb.Models.Enums;
using ElectionWeb.Models.ViewModels;
using ElectionWeb.Services;

namespace ElectionWeb.Controllers
{
    public class ReportCasesController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public ReportCasesController(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
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
            var cases = new List<string>();
            foreach (var item in Enum.GetNames(typeof(Case)))
            {
                cases.Add(item);
            }
            cases.RemoveAt(0);
            ViewBag.Cases = new SelectList(cases);
            return View();
        }

        // POST: ReportCases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Case,Latitude,Longitude,CaseNote,EventDate,Photo")] ReportCaseCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var uploadImage = await _fileService.UploadFile(model.Photo, Guid.NewGuid().ToString());
                var reportCase = new ReportCase
                {
                     Case = model.Case,
                     EventDate = model.EventDate,
                     Note = model.CaseNote,
                     Latitude = model.Latitude,
                     Longitude = model.Longitude,
                     Photo = uploadImage.Item2
                };
                _context.Add(reportCase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var cases = new List<string>();
            foreach (var item in Enum.GetNames(typeof(Case)))
            {
                cases.Add(item);
            }
            cases.RemoveAt(0);
            ViewBag.Cases = new SelectList(cases);
            var errors = ModelState.Values.SelectMany(x => x.Errors);
            DisplayError(errors.FirstOrDefault().ErrorMessage);
            return View(model);
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
