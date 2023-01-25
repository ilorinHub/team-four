namespace ElectionWeb.Models
{
    public class Party : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public Country Country { get; set; }
    }
}
