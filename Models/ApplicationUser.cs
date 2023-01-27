using Microsoft.AspNetCore.Identity;

namespace ElectionWeb.Models
{
    public class ApplicationUsers :  IdentityUser
    {
        public string Image { get; set; }
        public string NIN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
