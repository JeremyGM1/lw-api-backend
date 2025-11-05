namespace Identity.Configurations
{
    public static class SystemRoles
    {
        public const string SuperAdministrator = "Super Administrator";
        public const string Administrator = "Administrator";
        public const string TechnicalAccount = "Technical Account";
        public const string RegularUser = "Regular User";
        public const string Viewer = "Viewer";

        public static readonly string[] AllRoles =
        {
        SuperAdministrator,
        Administrator,
        TechnicalAccount,
        RegularUser,
        Viewer
        };

        public static readonly Dictionary<string, string> RoleDescriptions = new()
        {
            [SuperAdministrator] = "Full system access with all permissions.",
            [Administrator] = "Manages users, roles, and core configurations.",
            [TechnicalAccount] = "Handles system integrations and technical configurations.",
            [RegularUser] = "Standard user with access to operational functionalities.",
            [Viewer] = "Read-only access to view content and reports."
        };
    }
}
