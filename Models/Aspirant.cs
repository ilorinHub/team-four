namespace ElectionWeb.Models
{
    public class Aspirant : BaseModel
    {
        public Party Party { get; set; }
        public Election Election { get; set; }
    }
}
