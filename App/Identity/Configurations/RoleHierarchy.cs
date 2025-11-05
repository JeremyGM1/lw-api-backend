namespace Identity.Configurations
{
    public static class RoleHierarchy
    {
        public static readonly Dictionary<string, int> Levels = new()
        {
            [SystemRoles.SuperAdministrator] = 5,
            [SystemRoles.Administrator] = 4,
            [SystemRoles.TechnicalAccount] = 3,
            [SystemRoles.RegularUser] = 2,
            [SystemRoles.Viewer] = 1
        };

        public static bool CanInteractWith(string requesterRole, string targetRole)
        {
            if (!Levels.ContainsKey(requesterRole) || !Levels.ContainsKey(targetRole))
                return false;

            return Levels[requesterRole] >= Levels[targetRole];
        }
    }
}
