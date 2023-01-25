namespace ElectionWeb.Models
{
    public class LGA : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Constituency Constituency { get; set; }
    }
}
