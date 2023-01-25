using CsvHelper;
using ElectionWeb.Data;
using ElectionWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ElectionWeb.Controllers
{
	public class DataController : Controller
	{

        private readonly ApplicationDbContext _context;

        public DataController(ApplicationDbContext context)
        {
            _context = context;
        }
		public IActionResult Index()
		{
            return View();
		}

        [HttpPost]
        public IActionResult Create(DataUpload upload)
        {
           
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
                            _context.Countries.AddRange(records);
                            _context.SaveChanges();
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
                                if (countryName == null) break;
                                var country = countries.Where(x => x.Name.ToLower() == countryName.ToLower()).FirstOrDefault();
                                if (country == null) break;
                                record.Country = country;
                                records.Add(record);
                            }
                            _context.StateRegions.AddRange(records);
                            _context.SaveChanges();
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
                            _context.Constituencies.AddRange(records);
                            _context.SaveChanges();
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
                            _context.lGAs.AddRange(records);
                            _context.SaveChanges();
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
                            _context.Wards.AddRange(records);
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
                            _context.PollingUnits.AddRange(records);
                            _context.SaveChanges();
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

        public class Foo
        {
            public int Id { get; set; }
            public int Prev { get; set; }
        }
    }
}
