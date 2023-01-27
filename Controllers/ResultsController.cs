using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectionWeb.Data;
using ElectionWeb.Models;
using static ElectionWeb.Controllers.DataController;
using CsvHelper;
using System.Globalization;
using ElectionWeb.Models.ViewModels;
using static System.Collections.Specialized.BitVector32;

namespace ElectionWeb.Controllers
{
    public class ResultsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public ResultsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _appEnvironment = webHostEnvironment;
        }

        // GET: Results
        public async Task<IActionResult> Index()
        {
            return View(await _context.Results.ToListAsync());
        }

        // GET: Results/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Results == null)
            {
                return NotFound();
            }

            var result = await _context.Results
                .FirstOrDefaultAsync(m => m.Id == id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // GET: Results/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Results/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResultUploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                var extension = Path.GetExtension(model.File.FileName).ToLower();
                if (extension != ".csv")
                {
                    DisplayError("Only Files in CSV Formats are allowed");
                    return View(model);
                }
                var elections = _context.Elections.ToList();
                var parties = _context.Parties.ToList();
                var pollingUnits = _context.PollingUnits.Include(x=>x.Ward).ToList();
                using (var reader = new StreamReader(model.File.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {

                    csv.Read();
                    csv.ReadHeader();
                    var records = new List<ResultPicker>();
                    var results = new List<PartyResult>();

                    while (csv.Read())
                    {
                        try
                        {
                            var record = new ResultPicker
                            {
                                Election = csv.GetField<string>("Election"),
                                PollingUnit = csv.GetField<string>("PollingUnit"),
                                Count = csv.GetField<long>("Count"),
                                Party = csv.GetField<string>("Party")
                            };
                            records.Add(record);
                        }
                        catch(Exception ex)
                        {
                            DisplayError("Something Went Wrong");
                            DisplayError(ex.Message.ToString());
                            return View(model);
                        }
                        
                    }
                    var election = elections.FirstOrDefault(e => e.Name.ToLower().Replace(" ", "") == records.First().Election.ToLower().Replace(" ", ""));
                    if (election == null)
                    {
                        DisplayError("Invalid Election in list of records");
                        return View(model);
                    }    
                    var pollingUnit = pollingUnits.FirstOrDefault(e => e.Name.ToLower().Replace(" ", "") == records.First().PollingUnit.ToLower().Replace(" ", ""));
                    var r = new Result
                    {
                        Election = election,
                        PollingUnit = pollingUnit,
                        Ward = pollingUnit.Ward,
                        Date = DateTime.Now,
                    };
                    foreach (var item in records)
                    {
                        var party = parties.FirstOrDefault(e => e.Name.ToLower().Replace(" ", "") == item.Party.ToLower().Replace(" ", ""));
                        if (party == null)
                        {
                            DisplayError("Invalid Party in list of records");
                            return View(model);
                        }
                        var t = new PartyResult
                        {
                            Party = party,
                            ResultCount = item.Count
                        };
                        results.Add(t);
                    }
                    r.PartyResults = results;
                  
                    _context.Results.Add(r);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                    
                }
                
            }
            DisplayError("File Not Found");
            return View(model);
        }

        // GET: Results/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Results == null)
            {
                return NotFound();
            }

            var result = await _context.Results.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Results/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Date,Id,CodeId,Active,Deleted,CreatedAt,UpdatedAt")] Result result)
        {
            if (id != result.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(result);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResultExists(result.Id))
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
            return View(result);
        }

        // GET: Results/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Results == null)
            {
                return NotFound();
            }

            var result = await _context.Results
                .FirstOrDefaultAsync(m => m.Id == id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // POST: Results/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Results == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Results'  is null.");
            }
            var result = await _context.Results.FindAsync(id);
            if (result != null)
            {
                _context.Results.Remove(result);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResultExists(long id)
        {
            return _context.Results.Any(e => e.Id == id);
        }


        //public bool DownloadTemplate()
        //{
        //    var records = new List<Foo>
        //    {
        //        new Foo { Id = 1, Name = "one" },
        //        new Foo { Id = 2, Name = "two" },
        //    };
        //    var path = _appEnvironment.WebRootPath + "\\Documents\\filer.csv";      
        //    using (var writer = new StreamWriter(path))
        //    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        //    {
        //        csv.WriteRecords(records);
        //    }
        //    return true;
        //}

        public IActionResult DownloadTemplateNow()
        {
            var path = _appEnvironment.WebRootPath + "\\Documents\\ResultUploadTemplate.csv";
            return File(System.IO.File.ReadAllBytes(path), "text/plain", "ResultUploadTemplate.csv");
        }




    }

    public class ResultPicker
    {
        public string Election { get; set; }
        public string PollingUnit { get; set; }
        public string Party { get; set; }
        public long Count { get; set; }
    }

}
