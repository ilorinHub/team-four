using System.Security.Claims;

namespace ElectionWeb.Utilities
{
    public static class ClaimsEngine
    {
        public static List<Claim> claimsList = new List<Claim>()
        {
            new Claim(UserManagementClaims.ManageUsers, UserManagementClaims.ManageUsers),
            new Claim(RolesManagementClaims.ManageRoles, RolesManagementClaims.ManageRoles),
        };
    }

	public static class UserManagementClaims
	{
		public const string ManageUsers = "Manage Users";
	}

	public static class RolesManagementClaims
	{
		public const string ManageRoles = "Manage Roles";
	}
}




