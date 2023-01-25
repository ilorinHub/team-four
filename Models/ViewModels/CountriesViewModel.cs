using System.ComponentModel.DataAnnotations;

namespace ElectionWeb.Models.ViewModels
{
    public class CountriesViewModel
    {
    }

    public class CountriesCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
