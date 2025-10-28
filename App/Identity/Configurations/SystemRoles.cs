using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
