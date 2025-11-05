using Application.Features.Identity.Models.RoleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Features.Identity
{
    public interface IRoleService
    {
        Task<RoleDto> GetRoleByNameAsync(string roleName);
        Task<bool> UpdateRoleDescriptionAsync(string roleName, string description);
    }
}
