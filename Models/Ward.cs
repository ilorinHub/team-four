namespace ElectionWeb.Models
{
    public class Ward : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public LGA LGA { get; set; }
    }
}
