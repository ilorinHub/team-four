namespace ElectionWeb.Models.ViewModels
{
    public class PartyViewModel
    {
		public string Name { get; set; }
		public string Description { get; set; }
		public string Logo { get; set; }
		public long CountryId { get; set; }
		public string CountryName { get; set; }
	}

    public class PartyCreateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Logo { get; set; }
        public long CountryId { get; set; }
    }
}
