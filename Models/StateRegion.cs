namespace ElectionWeb.Models
{
    public class StateRegion : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Country Country { get; set; }
    }
}
