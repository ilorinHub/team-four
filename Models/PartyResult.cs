namespace ElectionWeb.Models
{
    public class PartyResult : BaseModel
    {
        public Party Party { get; set; }
        public long ResultCount { get; set; }
        public long ResultId { get; set; }
    }
}
