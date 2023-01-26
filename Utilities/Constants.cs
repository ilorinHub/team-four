namespace ElectionWeb.Utilities
{
    public class Constants
    {
    }

    public static class SD
    {
        public static string Success = "Success";
        public static string Error = "Error";
        public static string Warning = "Warning";
        public static string Info = "Info";
    }

	public static class Roles
	{

		public static List<string> AllRoles = new List<string>
		{
			INEC, FieldOfficers, SuperAdmin, User, NewsOutlets
        };

		public const string INEC = "INEC";
		public const string User = "User";
		public const string FieldOfficers = "Field Officers";
		public const string SuperAdmin = "Super Admin";
		public const string NewsOutlets = "News Outlets";

	}
}
