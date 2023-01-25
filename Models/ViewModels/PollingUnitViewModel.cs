using System.ComponentModel.DataAnnotations;

namespace ElectionWeb.Models.ViewModels
{
    public class PollingUnitCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public long WardId { get; set; }
    }
}
