namespace ElectionWeb.Models
{
    public class BaseModel
    {
        public BaseModel()
        {
            CodeId= Guid.NewGuid();
            CreatedAt = DateTime.Now;
            UpdatedAt= DateTime.Now;
            Active = true;
            Deleted= false;  
        }
        public long Id { get; set; }
        public Guid CodeId { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
