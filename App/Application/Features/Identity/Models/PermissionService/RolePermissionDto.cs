using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Models.PermissionService
{
    public class RolePermissionsDto
    {
        public string RoleName { get; set; } = default!;
        public string? RoleDescription { get; set; }
        public List<PermissionDto> Permissions { get; set; } = new();
    }
}
