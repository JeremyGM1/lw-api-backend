using Application.Exceptions;
using Application.Features.Identity.Models;
using Application.Interfaces.Identity;
using Domain.Entities.Users;
using FluentValidation;
using Identity.Configurations;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Identity.Services;

public class UserService(UserManager<ApplicationUser> userManager) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<RegistrationResponse> Register(RegistrationRequest request, string requestingUserId)
    {
        ValidateRequiredField(request.FirstName, nameof(request.FirstName));
        ValidateRequiredField(request.LastName, nameof(request.LastName));
        ValidateRequiredField(request.Email, nameof(request.Email));
        ValidateRequiredField(request.Password, nameof(request.Password));

        var (_, requesterRoles) = await GetUserWithRolesAsync(requestingUserId);

        var roleToAssign = string.IsNullOrWhiteSpace(request.Role)
                            ? SystemRoles.RegularUser
                            : request.Role.Trim();

        var roleExists = await _roleManager.RoleExistsAsync(roleToAssign);
        if (!roleExists)
        {
            throw new BadRequestException($"The role '{roleToAssign}' does not exist.");
        }

        if (roleToAssign == SystemRoles.SuperAdministrator && !requesterRoles.Contains(SystemRoles.SuperAdministrator))
            throw new UnauthorizedAccessException("Only Super Admins can create other Super Admins.");

        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            StringBuilder str = new StringBuilder();
            foreach (var err in result.Errors)
            {
                str.AppendFormat("•{0}\n", err.Description);
            }
            throw new BadRequestException($"{str}");
        }

        var addRoleResult = await _userManager.AddToRoleAsync(user, roleToAssign);
        if (!addRoleResult.Succeeded)
        {
            var errors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
            throw new BadRequestException($"Unable to assign role: {errors}");
        }

        return new RegistrationResponse() { UserId = user.Id };
    }

    public async Task<IEnumerable<UserProfileResponse>> GetAllUsersByRoleAsync(string requestingUserId)
    {
        var (_, requesterRoles) = await GetUserWithRolesAsync(requestingUserId);

        bool isSuperAdmin = requesterRoles.Contains(SystemRoles.SuperAdministrator);
        bool isAdmin = requesterRoles.Contains(SystemRoles.Administrator);

        if (!isSuperAdmin && !isAdmin)
            throw new UnauthorizedAccessException("You do not have permission to view users.");

        var allUsers = await _userManager.Users
                            .Where(u => u.Id != requestingUserId)
                            .ToListAsync();

        var visibleUsers = new List<ApplicationUser>();

        if (isSuperAdmin)
        {
            visibleUsers = allUsers;
        }
        else if (isAdmin)
        {
            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains(SystemRoles.SuperAdministrator))
                    visibleUsers.Add(user);
            }
        }

        var result = new List<UserProfileResponse>();
        foreach (var user in visibleUsers)
        {
            var roles = await _userManager.GetRolesAsync(user);
            result.Add(new UserProfileResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = roles.FirstOrDefault()
            });
        }

        return result;
    }


    public async Task<UpdateResponse> Update(UpdateRequest request, string requestingUserId)
    {
        var user = await _userManager.FindByIdAsync(request.Id) ?? throw new NotFoundException($"User with {request.Id} not found.", request.Id);

        var (_, requestingUserRoles) = await GetUserWithRolesAsync(requestingUserId);

        bool isSuperAdmin = requestingUserRoles.Contains(SystemRoles.SuperAdministrator);
        bool isAdmin = requestingUserRoles.Contains(SystemRoles.Administrator);
        bool isSelf = user.Id == requestingUserId;

        var targetRoles = await _userManager.GetRolesAsync(user);
        bool targetIsSuperAdmin = targetRoles.Contains(SystemRoles.SuperAdministrator);

        if (isAdmin && targetIsSuperAdmin)
            throw new ValidationException("Administrators cannot modify Super Administrators.");

        if (!isSuperAdmin && !isAdmin && !isSelf)
        {
            throw new UnauthorizedAccessException("You are not allowed to update other users.");
        }

        if (!string.IsNullOrEmpty(request.Email))
        {
            user.Email = request.Email;
            user.UserName = request.Email;
        }

        if (!string.IsNullOrEmpty(request.FirstName))
            user.FirstName = request.FirstName;

        if (!string.IsNullOrEmpty(request.LastName))
            user.LastName = request.LastName;

        if (!string.IsNullOrEmpty(request.Role))
        {
            if (!isSuperAdmin && !isAdmin)
                throw new UnauthorizedAccessException("You are not allowed to change user roles.");

            if (!isSuperAdmin && request.Role == SystemRoles.SuperAdministrator)
                throw new ValidationException("Administrators cannot assign the Super Administrator role.");

            var roleExists = await _roleManager.RoleExistsAsync(request.Role);
            if (!roleExists)
                throw new ValidationException($"The role '{request.Role}' does not exist.");

            var userRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, userRoles);
            var roleResult = await _userManager.AddToRoleAsync(user, request.Role);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new ValidationException($"Role update failed: {errors}");
            }
        }

        if (!string.IsNullOrEmpty(request.NewPassword) && !string.IsNullOrEmpty(request.CurrentPassword))
        {
            var result = await _userManager.ResetPasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                throw new ValidationException($"Password change failed. {errors}");
            }
        }

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
            throw new ValidationException($"User update failed: {errors}");
        }

        return new UpdateResponse() { UserId = user.Id };
    }

    public async Task<UpdateResponse> AdminUpdateRequest(AdminUpdateRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.Id) ?? throw new NotFoundException($"User with {request.Id} not found.", request.Id);
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        if (!string.IsNullOrEmpty(request.Email))
        {
            user.Email = request.Email;
            user.UserName = request.Email;
        }

        if (!string.IsNullOrEmpty(request.FirstName))
            user.FirstName = request.FirstName;

        if (!string.IsNullOrEmpty(request.LastName))
            user.LastName = request.LastName;

        if (!string.IsNullOrEmpty(request.NewPassword))
        {
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                throw new ValidationException($"Password change failed. {errors}");
            }
        }

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
            throw new ValidationException($"User update failed: {errors}");
        }

        return new UpdateResponse() { UserId = user.Id };
    }

    public async Task<bool> DeleteUserAsync(string userId, string requestingUserId)
    {
        var user = await _userManager.FindByIdAsync(userId)
        ?? throw new NotFoundException("User not found", userId);
        var userRoles = await _userManager.GetRolesAsync(user);

        var (_, requestingUserRoles) = await GetUserWithRolesAsync(requestingUserId);
       
        bool isSuperAdmin = requestingUserRoles.Contains(SystemRoles.SuperAdministrator);
        bool isAdmin = requestingUserRoles.Contains(SystemRoles.Administrator);
        bool isSelf = user.Id == requestingUserId;
        bool targetIsSuperAdmin = userRoles.Contains(SystemRoles.SuperAdministrator);

        if (!isAdmin && !isSuperAdmin && !isSelf)
        {
            throw new UnauthorizedAccessException("You are not allowed to delete this user.");
        }

        if (isAdmin && targetIsSuperAdmin)
            throw new ValidationException("Administrators cannot delete Super Administrators.");

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new BadRequestException($"Unable to delete user: {errors}");
        }

        return true;
    }

    public async Task<UserProfileResponse> GetCurrentUserProfileAsync(string userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            throw new NotFoundException("User", null);

        var roles = await _userManager.GetRolesAsync(user);

        return new UserProfileResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = roles.FirstOrDefault()
        };
    }

    private static void ValidateRequiredField(string value, string fieldName)
    {
        if (string.IsNullOrEmpty(value))
            throw new BadRequestException($"The {fieldName} field is required");
    }

    private async Task<(ApplicationUser User, IList<string> Roles)> GetUserWithRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException($"User with ID '{userId}' not found.", userId);

        var roles = await _userManager.GetRolesAsync(user);
        return (user, roles);
    }

}