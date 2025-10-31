using Application.Interfaces;
using Domain.Entities.Users;
using Identity.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Identity.Repositories;

public class PermissionRepository : IPermissionRepository
{
    private readonly TalentSuiteIdentityDbContext _context;

    public PermissionRepository(TalentSuiteIdentityDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Permission>> GetAsync()
        => await _context.Permissions.AsNoTracking().ToListAsync();

    public async Task<Permission?> GetByIdAsync(Guid id)
        => await _context.Permissions.FirstOrDefaultAsync(p => p.Id == id);

    public async Task CreateAsync(Permission entity)
    {
        await _context.Permissions.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Permission entity)
    {
        _context.Permissions.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Permission entity)
    {
        _context.Permissions.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Permission>> GetPermissionsByRoleNameAsync(string roleName)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        if (role == null) return new List<Permission>();

        return await _context.RolePermissions
            .Where(rp => rp.RoleId == role.Id)
            .Include(rp => rp.Permission)
            .Select(rp => rp.Permission)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Permission>> GetPermissionsByUserIdAsync(string userId)
    {
        // Get role IDs assigned to the user
        var userRoleIds = await _context.UserRoles
         .Where(ur => ur.UserId == userId)
         .Select(ur => ur.RoleId)
         .ToListAsync();

        // Permissions inherited from roles
        var inheritedPermissions = await _context.RolePermissions
            .Where(rp => userRoleIds.Contains(rp.RoleId))
            .Select(rp => rp.Permission)
            .Distinct()
            .ToListAsync();

        // Explicit user overrides (grants or revokes)
        var overrides = await _context.UserPermissions
            .Where(up => up.UserId == userId)
            .Select(up => new { up.Permission, up.IsGranted, up.PermissionId })
            .ToListAsync();

        // Start from inherited permissions and exclude those explicitly revoked
        var effective = inheritedPermissions
            .Where(p => !overrides.Any(o => o.PermissionId == p.Id && o.IsGranted == false))
            .ToList();

        // Add explicit grants not inherited from roles
        effective.AddRange(overrides
            .Where(o => o.IsGranted && inheritedPermissions.All(p => p.Id != o.PermissionId))
            .Select(o => o.Permission));

        // Return the final distinct set
        return effective
            .DistinctBy(p => p.Id)
            .ToList();
    }

    public async Task AssignPermissionsToRoleAsync(string roleId, IEnumerable<Guid> permissionIds, string updatedBy)
    {
        // Replace all existing role-permission links
        var existing = await _context.RolePermissions.Where(rp => rp.RoleId == roleId).ToListAsync();
        _context.RolePermissions.RemoveRange(existing);

        var now = DateTime.UtcNow;
        var newPermissions = permissionIds.Select(pid => new RolePermission
        {
            Id = Guid.NewGuid(),
            RoleId = roleId,
            PermissionId = pid,
            CreatedAt = now,
            CreatedBy = updatedBy
        });

        await _context.RolePermissions.AddRangeAsync(newPermissions);
        await _context.SaveChangesAsync();

        // Auto-clean user overrides that became redundant
        await CleanUserPermissionInconsistenciesAsync(roleId);
    }

    public async Task AssignPermissionsToUserAsync(string userId, IEnumerable<Guid> newPermissionIds, string updatedBy)
    {
        // Fetch user role and inherited permissions
        var userRoles = await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync();

        var rolePermissionIds = await _context.RolePermissions
            .Where(rp => userRoles.Contains(rp.RoleId))
            .Select(rp => rp.PermissionId)
            .Distinct()
            .ToListAsync();

        var existingOverrides = await _context.UserPermissions
            .Where(up => up.UserId == userId)
            .ToListAsync();

        var now = DateTime.UtcNow;
        var toAdd = new List<UserPermission>();
        var toRemove = new List<UserPermission>();

        // Determine new overrides to add or remove
        foreach (var pid in rolePermissionIds.Union(newPermissionIds).Distinct())
        {
            bool inherited = rolePermissionIds.Contains(pid);
            bool shouldHave = newPermissionIds.Contains(pid);
            var existing = existingOverrides.FirstOrDefault(x => x.PermissionId == pid);

            if (inherited)
            {
                // Permission comes from a role
                if (shouldHave)
                {
                    // If user had a revoked override, remove it
                    if (existing != null && existing.IsGranted == false)
                        toRemove.Add(existing);
                }
                else
                {
                    // Explicitly revoke inherited permission
                    if (existing == null)
                    {
                        toAdd.Add(new UserPermission
                        {
                            Id = Guid.NewGuid(),
                            UserId = userId,
                            PermissionId = pid,
                            IsGranted = false,
                            CreatedAt = now,
                            CreatedBy = updatedBy
                        });
                    }
                }
            }
            else
            {
                // Permission not inherited (purely user-level)
                if (shouldHave)
                {
                    // Add explicit grant
                    if (existing == null)
                    {
                        toAdd.Add(new UserPermission
                        {
                            Id = Guid.NewGuid(),
                            UserId = userId,
                            PermissionId = pid,
                            IsGranted = true,
                            CreatedAt = now,
                            CreatedBy = updatedBy
                        });
                    }
                }
                else
                {
                    // Remove unnecessary override
                    if (existing != null)
                        toRemove.Add(existing);
                }
            }
        }

        _context.UserPermissions.RemoveRange(toRemove);
        await _context.UserPermissions.AddRangeAsync(toAdd);
        await _context.SaveChangesAsync();
    }

    private async Task CleanUserPermissionInconsistenciesAsync(string roleId)
    {
        // Fetch all permissions linked to the role
        var rolePermissionIds = await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .Select(rp => rp.PermissionId)
            .ToListAsync();
        // Get all users in this role
        var usersInRole = await _context.UserRoles
            .Where(ur => ur.RoleId == roleId)
            .Select(ur => ur.UserId)
            .ToListAsync();
        // Find user overrides related to those users
        var overrides = await _context.UserPermissions
            .Where(up => usersInRole.Contains(up.UserId))
            .ToListAsync();

        var toRemove = new List<UserPermission>();

        foreach (var up in overrides)
        {
            bool permissionInRole = rolePermissionIds.Contains(up.PermissionId);

            // Case 1: Role now grants the permission → explicit grant override is redundant
            if (permissionInRole && up.IsGranted)
                toRemove.Add(up);

            // Case 2: Role no longer grants it → explicit revoke override is redundant
            if (!permissionInRole && !up.IsGranted)
                toRemove.Add(up);
        }

        if (toRemove.Any())
        {
            _context.UserPermissions.RemoveRange(toRemove);
            await _context.SaveChangesAsync();
        }
    }
}