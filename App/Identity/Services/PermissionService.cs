using Application.Exceptions;
using Application.Features.Identity.Models.PermissionService;
using Application.Interfaces.Features.Identity;
using Domain.Entities.Users;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Services;

public class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public PermissionService(IPermissionRepository permissionRepository,
                             RoleManager<ApplicationRole> roleManager,
                             UserManager<ApplicationUser> userManager)
    {
        _permissionRepository = permissionRepository;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<IReadOnlyList<Permission>> GetAllPermissionsAsync() =>
        await _permissionRepository.GetAsync();

    public async Task<IReadOnlyList<Permission>> GetPermissionsByRoleNameAsync(string roleName) =>
        await _permissionRepository.GetPermissionsByRoleNameAsync(roleName);

    public async Task<IReadOnlyList<Permission>> GetPermissionsByUserIdAsync(string userId) =>
        await _permissionRepository.GetPermissionsByUserIdAsync(userId);

    public async Task AssignPermissionsToRoleAsync(string roleName, IEnumerable<string> permissionNames, string updatedBy)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
            throw new NotFoundException("Role", roleName);

        var permissionIds = await ValidateAndMapPermissionNamesAsync(permissionNames);

        await _permissionRepository.AssignPermissionsToRoleAsync(role.Id, permissionIds, updatedBy);
    }

    public async Task AssignPermissionsToUserAsync(string userId, IEnumerable<string> permissionNames, string updatedBy)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User", userId);

        var permissionIds = await ValidateAndMapPermissionNamesAsync(permissionNames);

        await _permissionRepository.AssignPermissionsToUserAsync(userId, permissionIds, updatedBy);
    }

    public async Task<IReadOnlyList<RolePermissionsDto>> GetAllRolesWithPermissionsAsync()
    {
        var data = await _permissionRepository.GetAllRolesWithPermissionsAsync();

        var result = data.Select(r => new RolePermissionsDto
        {
            RoleName = r.RoleName,
            Permissions = r.Permissions.Select(p => new PermissionDto
            {
                Name = p.Name,
                Description = p.Description
            }).ToList()
        }).ToList();

        return result;
    }

    private async Task<List<Guid>> ValidateAndMapPermissionNamesAsync(IEnumerable<string> permissionNames)
    {
        var allPermissions = await _permissionRepository.GetAsync();

        var selected = allPermissions
            .Where(p => permissionNames.Contains(p.Name))
            .Select(p => p.Id)
            .ToList();

        if (selected.Count != permissionNames.Count())
            throw new BadRequestException("One or more provided permissions do not exist.");

        return selected;
    }
}