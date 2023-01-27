using ElectionWeb.Models.Enums;
using System.ComponentModel;

namespace ElectionWeb.Models.ViewModels
{
    public class ReportCaseCreateViewModel
    {
        [DisplayName("Case Type")]
        public Case Case { get; set; }
        [DisplayName("Note")]
        public string CaseNote { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public IFormFile Photo { get; set; }
        public DateTime EventDate { get; set; }
    }
}
