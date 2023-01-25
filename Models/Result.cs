

namespace ElectionWeb.Models
{
    public class Result : BaseModel
    {
        public PollingUnit PollingUnit { get; set; }
        public Ward Ward { get; set; }
        public Election Election { get; set; }
        public DateTime Date { get; set; }
        public List<PartyResult> PartyResults { get; set; }
    }
}
