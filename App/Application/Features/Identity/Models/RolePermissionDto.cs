using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Models
{
    public class RolePermissionsDto
    {
        public string RoleName { get; set; } = default!;
        public List<PermissionDto> Permissions { get; set; } = new();
    }
}
