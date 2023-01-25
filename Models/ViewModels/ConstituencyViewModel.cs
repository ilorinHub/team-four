namespace ElectionWeb.Models.ViewModels
{
    public class ConstituencyViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long StateRegionId { get; set; }
    }

    public class ConstituencyCreateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long StateRegionId { get; set; }
    }
}
