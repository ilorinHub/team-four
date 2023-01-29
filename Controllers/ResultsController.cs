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
using Microsoft.AspNetCore.Identity;

namespace ElectionWeb.Controllers
{
    public class ResultsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly UserManager<ApplicationUsers> _userManager;

        public ResultsController(ApplicationDbContext context, UserManager<ApplicationUsers> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _appEnvironment = webHostEnvironment;
        }

        // GET: Results
        public async Task<IActionResult> Index(long? wardId)
        {
            var users = await _context.ApplicationUsers.ToListAsync();
            var result = await _context.Results.Include(x => x.PollingUnit).Include(x => x.Ward).Include(x => x.PartyResults).ToListAsync();
            if(wardId != null)
            {
                result = result.Where(x => x.Ward.Id == wardId).ToList();
            }
            var resultMap = result.Select(x =>
            {
                var partyResults = x.PartyResults;
                var user = users.Where(u => u.Id == x.CreatedBy).FirstOrDefault();
                //   var role =  _userManager.GetRolesAsync(user);

                return new ResultViewModel
                {
                    Id = x.Id,
                    PollingUnitName = x.PollingUnit.Name,
                    PollingUnitId = x.PollingUnit.Id,
                    WardName = x.Ward.Name,
                    WardId = x.Ward.Id,
                    TotalVoteCount = partyResults.Select(x => x.ResultCount).Sum(),
                    TotalParties = partyResults.Count(),
                    User = user.Email,
                    Role = "Unassigned"

                };
            }).ToList();
            return View(resultMap);
        }




        // GET: Results
        public async Task<IActionResult> WardResult()
        {
            var users = await _context.ApplicationUsers.ToListAsync();
            var result = await _context.Results.Include(x => x.PollingUnit).Include(x => x.Ward).ThenInclude(x=>x.LGA).Include(x => x.PartyResults).ThenInclude(x=>x.Party).ToListAsync();

            var groupByWards = result.GroupBy(result => result.Ward.Id);
            //var dat = groupByWards.Select(x => x.Select(j =>
            //{
            //    //var partyResults = j.PartyResults.ToList();
            //    return new WardResultViewModel
            //    {
            //        Ward = j.Ward.Name,
            //        WardId = j.Ward.Id,
            //        LgaName = j.Ward.LGA.Name,
            //        LgaId = j.Ward.LGA.Id,
            //        PollingUnitCounts = x.Count(),
            //        TotalVote = partyResults.Select(x => x.ResultCount).Sum()

            //    };
            //}
            //)
            //).ToList();
            var resultListOfWards = new List<WardResultViewModel>();
            foreach (var item in groupByWards)
            {
                var data = item.Select(x =>
                {
                    var pResults = x.PartyResults;
                    return new WardResultViewModel
                    {
                        Ward = x.Ward.Name,
                        WardId = x.Ward.Id,
                        LgaId = x.Ward.LGA.Id,
                        LgaName = x.Ward.LGA.Name,
                        TotalVote = pResults.Sum(x => x.ResultCount),
                        TotalParties = pResults.Count(),
                        PartyResult = pResults.Select(x =>
                        new PartyResultViewModel {
                              Id = x.Id,
                              PartyCount = x.ResultCount,
                              PartyName = x.Party.Name
                        }).ToList()
                    };
                }).ToList();
                var totalpartyResult = new List<PartyResultViewModel>();
                foreach (var ite in data)
                {
                    totalpartyResult.AddRange(ite.PartyResult);
                }

                var something = totalpartyResult.GroupBy(tp => tp.PartyName).Select(tpx =>
                new PartyResultViewModel
                {
                    PartyName = tpx.Select(x => x.PartyName).First(),
                    Id = tpx.Select(x => x.Id).First(),
                    PartyCount = tpx.Sum(x => x.PartyCount)
                }).ToList();
                
                var res = new WardResultViewModel
                {
                    LgaName = data.First().LgaName,
                    LgaId = data.First().LgaId,
                    Ward = data.First().Ward,
                    WardId = data.First().WardId,
                    TotalVote = data.Select(x => x.TotalVote).Sum(),
                    PollingUnitCounts = data.Select(x=>x.PartyResult).Count(),
                    PartyResult = something,
                    TotalParties = something.Count()
                };
                resultListOfWards.Add(res);
            }

            return View(resultListOfWards);
        }


        public async Task<IActionResult> StateIndex()
        {
            var users = await _context.ApplicationUsers.ToListAsync();
            var state = _context.StateRegions.ToList();
            var constituency = _context.Constituencies.ToList();
            var lgas = _context.Constituencies.ToList();
            var wards = _context.lGAs.ToList();
            var pollingUnits = _context.PollingUnits.ToList();
            var result = await _context.Results.Include(x => x.PollingUnit).Include(x => x.Ward).ThenInclude(x => x.LGA).ThenInclude(x => x.Constituency).ThenInclude(x => x.StateRegion).Include(x => x.PartyResults).ToListAsync();
            //var wardResults = result.GroupBy(x=>)
            //var resultMap = result.Select(x =>
            //{
            //	var partyResults = x.PartyResults;
            //	var user = users.Where(u => u.Id == x.CreatedBy).FirstOrDefault();
            //	//   var role =  _userManager.GetRolesAsync(user);

            //	return new ResultViewModel
            //	{
            //		Id = x.Id,
            //		PollingUnitName = x.PollingUnit.Name,
            //		PollingUnitId = x.PollingUnit.Id,
            //		WardName = x.Ward.Name,
            //		WardId = x.Ward.Id,
            //		TotalVoteCount = partyResults.Select(x => x.ResultCount).Sum(),
            //		TotalParties = partyResults.Count(),
            //		User = user.Email,
            //		Role = "Unassigned"

            //	};
            //}).ToList();

            //var wardGrouped = result.GroupBy(x => x.Ward.Id).ToList();
            //var lgaGrouping = wardGrouped.Select(x => new
            //{
            //    LgaName = x.Select(x=>x.Ward.LGA.Name),
            //    LgaId = x.First().Ward.LGA.Id,
            //    Cons = x.First().Ward.LGA.Constituency.Name,
            //    WardsAvailable = 
            //});
            //var stateResult = wardGrouped.Select(x =>
            //new {

            //})
            return View();
        }




        // GET: Results/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Results == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                DisplayError("Invalid user");
                return RedirectToAction("Index");
            }
            var result = await _context.Results.Include(x => x.Ward).Include(x => x.PollingUnit).Include(x => x.PartyResults).ThenInclude(x => x.Party)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            var partyResults = result.PartyResults.Select(x =>
            new PartyResultViewModel
            {
                Id = x.Id,
                PartyCount = x.ResultCount,
                PartyName = x.Party.Name
            }
            ).ToList();
            var res = new ResultDetailViewModel
            {
                Id = result.Id,
                PollingUnitId = result.PollingUnit.Id,
                PollingUnitName = result.PollingUnit.Name,
                TotalParties = result.PartyResults.Count(),
                WardId = result.Ward.Id,
                WardName = result.Ward.Name,
                TotalVoteCount = result.PartyResults.Select(x => x.ResultCount).Sum(),
                PartyResult = partyResults
            };
            return View(res);
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
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    DisplayError("Invalid user");
                    return View(model);
                }
                var extension = Path.GetExtension(model.File.FileName).ToLower();
                if (extension != ".csv")
                {
                    DisplayError("Only Files in CSV Formats are allowed");
                    return View(model);
                }
                var elections = _context.Elections.ToList();
                var parties = _context.Parties.ToList();
                var pollingUnits = _context.PollingUnits.Include(x => x.Ward).ToList();
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
                        catch (Exception ex)
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
                        CreatedBy = user.Id
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
