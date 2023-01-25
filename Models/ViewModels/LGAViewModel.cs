namespace ElectionWeb.Models.ViewModels
{
    public class LGAViewModel
    {
    }

    public class LGACreateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long ConstituencyId { get; set; }
    }
}
