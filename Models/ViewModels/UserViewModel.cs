namespace ElectionWeb.Models.ViewModels
{
    public class UserViewModel
    {
    }

    public class UserView
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Id { get; set; }
    }

    public class UserEditViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string NIN { get; set; }
    }
}
