namespace ElectionWeb.Models.ViewModels
{

    public class AspirantCreateViewModel
    {
        public long ElectionId { get; set; }
        public long PartyId { get; set; }
        public string Email { get; set; }
    }

    public class AspirantIndexViewModel
    {
        public List<AspirantViewModel> Aspirants { get; set; }
    }
    public class AspirantViewModel
    {
        public long Id { get; set; }
        public long ElectionId { get; set; }
        public long PartyId { get; set; }
        public string Election { get; set; }
        public string Party { get; set; }
        public string User { get; set; }
    }
}
