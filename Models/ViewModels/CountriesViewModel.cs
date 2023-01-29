using System.ComponentModel.DataAnnotations;

namespace ElectionWeb.Models.ViewModels
{
    public class CountriesViewModel
    {
        public Country Country { get; set; } 
        public List<StateRegion> StateRegions { get; set; }
    }

    public class CountriesCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
