namespace ElectionWeb.Models
{
    public class Election : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public Country Country { get; set; }
        public ElectionType ElectionType { get; set; }

    }
}
