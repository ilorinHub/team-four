namespace ElectionWeb.Models.ViewModels
{
	public class ElectionViewModel
	{
	}

	public class ElectionCreateViewModel
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime EventDate { get; set; }
		public long ElectionTypeId { get; set; }
		public long CountryId { get; set; }
	}

}
