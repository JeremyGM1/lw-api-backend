namespace Identity.Configurations
{
    public static class SystemPermissions
    {
        public static readonly string ManageUsers_Create = "ManageUsers.Create";
        public static readonly string ManageUsers_Edit = "ManageUsers.Edit";
        public static readonly string ManageUsers_Delete = "ManageUsers.Delete";
        public static readonly string ManageUsers_View = "ManageUsers.View";

        public static readonly string ManageRoles_View = "ManageRoles.View";
        public static readonly string ManageRoles_Edit = "ManageRoles.Edit";

        public static readonly string ViewAnalytics = "View.Analytics";
        public static readonly string ConfigureAPI = "Configure.API";
        public static readonly string ManageBackgroundJobs = "Manage.BackgroundJobs";
        public static readonly string ManageContent = "Manage.Content";
        public static readonly string ViewContent = "View.Content";
        public static readonly string CommentPost = "Comment.Post";
        public static readonly string PurchaseTransact = "Purchase.Transact";

        public static readonly (string Name, string Description)[] AllPermissions =
        {
            (ManageUsers_Create, "Create users in the system"),
            (ManageUsers_Edit, "Edit existing users"),
            (ManageUsers_Delete, "Delete users"),
            (ManageUsers_View, "View user list"),

            (ManageRoles_View, "View roles and permissions"),
            (ManageRoles_Edit, "Edit or assign permissions to roles"),

            (ViewAnalytics, "Access analytics and reports"),
            (ConfigureAPI, "Configure API integrations"),
            (ManageBackgroundJobs, "Enable or disable background processes"),
            (ManageContent, "Create or edit content"),
            (ViewContent, "Read or view content"),
            (CommentPost, "Comment or post content"),
            (PurchaseTransact, "Perform purchases or transactions"),
        };

        public static readonly Dictionary<string, string[]> RolePermissions = new()
        {
            [SystemRoles.SuperAdministrator] = AllPermissions.Select(p => p.Name).ToArray(),

            [SystemRoles.Administrator] = new[]
            {
                ManageUsers_Create, ManageUsers_Edit, ManageUsers_View,
                ManageRoles_View,
                ViewAnalytics,
                ConfigureAPI,
                ManageContent, ViewContent, CommentPost, PurchaseTransact
            },

            [SystemRoles.TechnicalAccount] = new[]
            {
                ConfigureAPI, ManageBackgroundJobs, ViewAnalytics
            },

            [SystemRoles.RegularUser] = new[]
            {
                ManageUsers_View, ViewContent, ManageContent, CommentPost, PurchaseTransact
            },

            [SystemRoles.Viewer] = new[]
            {
                ViewContent
            }
        };
    }
}
