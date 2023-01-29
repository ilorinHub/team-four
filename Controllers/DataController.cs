using CsvHelper;
using ElectionWeb.Data;
using ElectionWeb.Models;
using ElectionWeb.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ElectionWeb.Controllers
{
	public class DataController : BaseController
	{

        private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _appEnvironment;

		public DataController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _appEnvironment = webHostEnvironment;
        }
		public IActionResult Index()
		{
            return View();
		}

        public IActionResult DownloadTemplate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DataUpload upload)
        {
            var extension = Path.GetExtension(upload.File.FileName).ToLower();
            if (extension != ".csv")
            {
                DisplayError("Only Files in CSV Formats are allowed");
                return View("Index", upload);
            }
            using (var reader = new StreamReader(upload.File.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
               
                csv.Read();
                csv.ReadHeader();
                switch (upload.FileType)
                {
                    case "Country":
                        {
                            var records = new List<Country>();
                            while (csv.Read())
                            {
                                var record = new Country
                                {
                                    Name = csv.GetField<string>("Name"),
                                    Description = csv.GetField<string>("Description")
                                };
                                records.Add(record);
                            }
                            if (!records.Any())
                            {
                                DisplayError("File Cannot Be Empty, Try Again");
                                break;
                            }
                            var listOfNewCountries = records.Select(x => x.Name.ToLower().Replace(" ", "")).ToList();
                            var nameExist = _context.Countries.Any(x => listOfNewCountries.Contains(x.Name.ToLower().Replace(" ", "")));
                            if (nameExist)
                            {
                                DisplayError("Upload Failed");
                                DisplayError("One or More Countries in list Already Exist");
                                break;
                            }
                            try
                            {
                                _context.Countries.AddRange(records);
                                _context.SaveChanges();
                                DisplayMessage("Upload Successful");
                               
                            }
                            catch(Exception ex)
                            {
                                DisplayMessage("Something Went wrong");
                                break;
                            }


                        }
                        break;
                    case "State":
                        {
                            var countries = _context.Countries.ToList();
                            var records = new List<StateRegion>();
                            while (csv.Read())
                            {
                                var record = new StateRegion
                                {
                                    Name = csv.GetField<string>("Name"),
                                    Description = csv.GetField<string>("Description")
                                };
                                var countryName = csv.GetField<string>("Country");
                                if (countryName == null)
                                {
                                    DisplayError("Country Field Cannot be Empty");
                                    break;
                                };
                                var country = countries.Where(x => x.Name.ToLower() == countryName.ToLower()).FirstOrDefault();
                                if (country == null) break;
                                record.Country = country;
                                records.Add(record);
                            }
                            if (!records.Any())
                            {
                                DisplayError("File Cannot Be Empty, Try Again");
                                break;
                            }
                            var listOfNewStates = records.Select(x => x.Name.ToLower().Replace(" ", "")).ToList();
                            var nameExist = _context.StateRegions.Any(x => listOfNewStates.Contains(x.Name.ToLower().Replace(" ", "")));
                            if (nameExist)
                            {
                                DisplayError("Upload Failed");
                                DisplayError("One or More States in list, Already Exist");
                                break;
                            }
                            try
                            {
                                _context.StateRegions.AddRange(records);
                                _context.SaveChanges();
                                DisplayMessage("Upload Successful");

                            }
                            catch (Exception ex)
                            {
                                DisplayMessage("Something Went wrong");
                                break;
                            }
                            
                        }
                        break;
                    case "Constituency":
                        {
                            var states = _context.StateRegions.ToList();
                            var records = new List<Constituency>();
                            while (csv.Read())
                            {
                                var record = new Constituency
                                {
                                    Name = csv.GetField<string>("Name"),
                                    Description = csv.GetField<string>("Description")
                                };
                                var stateName = csv.GetField<string>("State");
                                if (stateName == null) break;
                                var state = states.Where(x => x.Name.Trim().ToLower() == stateName.Trim().ToLower()).FirstOrDefault();
                                if (state == null) break;
                                record.StateRegion = state;
                                records.Add(record);
                            }
                            if (!records.Any())
                            {
                                DisplayError("File Cannot Be Empty, Try Again");
                                break;
                            }
                            var listOfNewConstituencies = records.Select(x => x.Name.ToLower().Replace(" ", "")).ToList();
                            var nameExist = _context.Constituencies.Any(x => listOfNewConstituencies.Contains(x.Name.ToLower().Replace(" ", "")));
                            if (nameExist)
                            {
                                DisplayError("Upload Failed");
                                DisplayError("One or More Constituencies in list, Already Exist");
                                break;
                            }
                            try
                            {
                                _context.Constituencies.AddRange(records);
                                _context.SaveChanges();
                                DisplayMessage("Upload Successful");
                            }
                            catch (Exception ex)
                            {
                                DisplayMessage("Something Went wrong");
                                break;
                            }
                        }
                        break;
                    case "LGA":
                        {
                            var constituencies = _context.Constituencies.ToList();
                            var records = new List<LGA>();
                            while (csv.Read())
                            {
                                var record = new LGA
                                {
                                    Name = csv.GetField<string>("Name"),
                                    Description = csv.GetField<string>("Description")
                                };
                                var constituencyName = csv.GetField<string>("Constituency");
                                if (constituencyName == null) break;
                                var constituency = constituencies.Where(x => x.Name.Trim().ToLower() == constituencyName.Trim().ToLower()).FirstOrDefault();
                                if (constituency == null) break;
                                record.Constituency = constituency;
                                records.Add(record);
                            }
                            if (!records.Any())
                            {
                                DisplayError("File Cannot Be Empty, Try Again");
                                break;
                            }
                            var listOfNewlgas = records.Select(x => x.Name.ToLower().Replace(" ", "")).ToList();
                            var nameExist = _context.lGAs.Any(x => listOfNewlgas.Contains(x.Name.ToLower().Replace(" ", "")));
                            if (nameExist)
                            {
                                DisplayError("Upload Failed");
                                DisplayError("One or More LGAs in list, Already Exist");
                                break;
                            }
                            _context.lGAs.AddRange(records);
                            _context.SaveChanges();
                            DisplayMessage("Upload Successful");
                        }
                        break;
                    case "Ward":
                        {
                            var lgas = _context.lGAs.ToList();
                            var records = new List<Ward>();
                            while (csv.Read())
                            {
                                var record = new Ward
                                {
                                    Name = csv.GetField<string>("Name"),
                                    Description = csv.GetField<string>("Description")
                                };
                                var lgaName = csv.GetField<string>("LGA");
                                if (lgaName == null) break;
                                var lga = lgas.Where(x => x.Name.Trim().ToLower() == lgaName.Trim().ToLower()).FirstOrDefault();
                                if (lga == null) break;
                                record.LGA = lga;
                                records.Add(record);
                            }
                            if (!records.Any())
                            {
                                DisplayError("File Cannot Be Empty, Try Again");
                                break;
                            }
                            var listOfNewWards = records.Select(x => x.Name.ToLower().Replace(" ", "")).ToList();
                            var nameExist = _context.Wards.Any(x => listOfNewWards.Contains(x.Name.ToLower().Replace(" ", "")));
                            if (nameExist)
                            {
                                DisplayError("Upload Failed");
                                DisplayError("One or More States in list, Already Exist");
                                break;
                            }
                            _context.Wards.AddRange(records);
                            DisplayMessage("Upload Successful");
                            _context.SaveChanges();
                        }
                        break;
                    case "Polling Unit":
                        {
                            var wards = _context.Wards.ToList();
                            var records = new List<PollingUnit>();
                            while (csv.Read())
                            {
                                var record = new PollingUnit
                                {
                                    Name = csv.GetField<string>("Name"),
                                    Description = csv.GetField<string>("Description")
                                };
                                var wardName = csv.GetField<string>("Ward");
                                if (wardName == null) break;
                                var ward = wards.Where(x => x.Name.Trim().ToLower() == wardName.Trim().ToLower()).FirstOrDefault();
                                if (ward == null) break;
                                record.Ward = ward;
                                records.Add(record);
                            }
                            if (!records.Any())
                            {
                                DisplayError("File Cannot Be Empty, Try Again");
                                break;
                            }
                            _context.PollingUnits.AddRange(records);
                            _context.SaveChanges();
                            DisplayMessage("Upload Successful");
                        }
                        break;
                    default:
                        {
                            break;
                        }
                        break;
                }
                
            }
            
            return View("Index");
        }
        public IActionResult DownloadFile(string fileType)
        {
            
            switch (fileType)
            {
                case "Country":
                    {
						var path = _appEnvironment.WebRootPath + "\\Documents\\countryTemplate.csv";
						return File(System.IO.File.ReadAllBytes(path), "text/plain", "countryTemplate.csv");
					}
                case "State":
                    {
						var path = _appEnvironment.WebRootPath + "\\Documents\\stateTemplate.csv";
						return File(System.IO.File.ReadAllBytes(path), "text/plain", "stateTemplate.csv");
					}
                case "Constituency":
                    {
						var path = _appEnvironment.WebRootPath + "\\Documents\\constituencyTemplate.csv";
						return File(System.IO.File.ReadAllBytes(path), "text/plain", "constituencyTemplate.csv");

					}
                case "LGA":
                    {
						var path = _appEnvironment.WebRootPath + "\\Documents\\lgaTemplate.csv";
						return File(System.IO.File.ReadAllBytes(path), "text/plain", "lgaTemplate.csv");
					}
                case "Ward":
                    {
						var path = _appEnvironment.WebRootPath + "\\Documents\\wardTemplate.csv";
						return File(System.IO.File.ReadAllBytes(path), "text/plain", "wardTemplate.csv");
					}
                case "Polling Unit":
                    {
						var path = _appEnvironment.WebRootPath + "\\Documents\\pollingunitTemplate.csv";
						return File(System.IO.File.ReadAllBytes(path), "text/plain", "PollingUnitTemplate.csv");
					}

				default:
                    return NotFound();
            }
        }
    }
}
