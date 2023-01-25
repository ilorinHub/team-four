using System.ComponentModel.DataAnnotations;

namespace ElectionWeb.Models
{
    public class PollingUnit : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public Ward Ward { get; set; }
    }
}
