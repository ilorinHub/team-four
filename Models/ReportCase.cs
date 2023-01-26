using ElectionWeb.Models.Enums;

namespace ElectionWeb.Models
{
    public class ReportCase : BaseModel
    {
        public Case Case { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Note { get; set; }
        public DateTime EventDate { get; set; }
        public string Photo { get; set; }
    }
}
