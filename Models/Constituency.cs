namespace ElectionWeb.Models
{
    public class Constituency : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public StateRegion StateRegion { get; set; }
    }
}
