namespace ElectionWeb.Models.ViewModels
{
	public class RoleClaimsViewModel
	{
		public RoleClaimsViewModel()
		{
			Claims = new List<RoleClaim>();
		}
		public List<RoleClaim> Claims { get; set; }
		public string RoleId { get; set; }
		public string Rolename { get; set; }
	}

	public class RoleClaim
	{
		public string ClaimName { get; set; }
		public bool IsSelected { get; set; }
	}
}