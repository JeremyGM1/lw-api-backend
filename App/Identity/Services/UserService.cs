using Application.Exceptions;
using Application.Features.Identity.Models.UserService;
using Application.Interfaces.Features.Identity;
using FluentValidation;
using Identity.Configurations;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Identity.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<RegistrationResponse> Register(RegistrationRequest request, string requestingUserId)
    {
        try
        {
            ValidateRequiredField(request.FirstName, nameof(request.FirstName));
            ValidateRequiredField(request.LastName, nameof(request.LastName));
            ValidateRequiredField(request.Email, nameof(request.Email));
            ValidateRequiredField(request.Password, nameof(request.Password));

            var (_, requesterRoles) = await GetUserWithRolesAsync(requestingUserId);
            var requesterRole = requesterRoles.FirstOrDefault() ?? SystemRoles.RegularUser;

            var roleToAssign = string.IsNullOrWhiteSpace(request.Role)
                                ? SystemRoles.RegularUser
                                : request.Role.Trim();

            var roleExists = await _roleManager.RoleExistsAsync(roleToAssign);
            if (!roleExists)
            {
                throw new BadRequestException($"The role '{roleToAssign}' does not exist.");
            }

            if (!RoleHierarchy.CanInteractWith(requesterRole, roleToAssign))
                throw new UnauthorizedAccessException($"You cannot assign the role '{roleToAssign}' because it is higher than your current role '{requesterRole}'.");

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
        }catch(Exception ex)
        {
            throw new BadRequestException($"Error {ex}");
        }
    }

    public async Task<IEnumerable<UserProfileResponse>> GetAllUsersByRoleAsync(string requestingUserId)
    {
        var (_, requesterRoles) = await GetUserWithRolesAsync(requestingUserId);
        var requesterRole = requesterRoles.FirstOrDefault() ?? SystemRoles.Viewer;

        var allUsers = await _userManager.Users
                        .Where(u => u.Id != requestingUserId)
                        .ToListAsync();

        var visibleUsers = new List<UserProfileResponse>();

        foreach (var user in allUsers)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var targetRole = roles.FirstOrDefault() ?? SystemRoles.Viewer;

            if (RoleHierarchy.CanInteractWith(requesterRole, targetRole))
            {
                visibleUsers.Add(new UserProfileResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = targetRole
                });
            }
        }

        return visibleUsers;
    }

    public async Task<UpdateResponse> Update(UpdateRequest request, string requestingUserId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.Id) ?? throw new NotFoundException($"User with {request.Id} not found.", request.Id);

            var (_, requesterRoles) = await GetUserWithRolesAsync(requestingUserId);
            var requesterRole = requesterRoles.FirstOrDefault() ?? SystemRoles.Viewer;

            var targetRoles = await _userManager.GetRolesAsync(user);
            var targetRole = targetRoles.FirstOrDefault() ?? SystemRoles.Viewer;

            bool isSelf = user.Id == requestingUserId;

            if (!isSelf && !RoleHierarchy.CanInteractWith(requesterRole, targetRole))
                throw new UnauthorizedAccessException($"Your role '{requesterRole}' cannot update a user with role '{targetRole}'.");

            if (!string.IsNullOrEmpty(request.Role))
            {
                var roleExists = await _roleManager.RoleExistsAsync(request.Role);
                if (!roleExists)
                    throw new ValidationException($"The role '{request.Role}' does not exist.");

                if (!RoleHierarchy.CanInteractWith(requesterRole, request.Role))
                    throw new UnauthorizedAccessException($"You cannot assign a higher-level role ('{request.Role}').");

                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);

                var roleResult = await _userManager.AddToRoleAsync(user, request.Role);
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    throw new ValidationException($"Role update failed: {errors}");
                }
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

            if (!string.IsNullOrEmpty(request.NewPassword) && !string.IsNullOrEmpty(request.CurrentPassword))
            {
                var passwordResult = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
                if (!passwordResult.Succeeded)
                {
                    var errors = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
                    throw new ValidationException($"Password change failed: {errors}");
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                throw new ValidationException($"User update failed: {errors}");
            }

            return new UpdateResponse { UserId = user.Id };
        }
        catch (Exception ex)
        {
            throw new BadRequestException($"Error: {ex.Message}");
        }
    }

    public async Task<UpdateResponse> AdminUpdateRequest(AdminUpdateRequest request, string requestingUserId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.Id)
                ?? throw new NotFoundException($"User with ID '{request.Id}' not found.", request.Id);

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var (_, requesterRoles) = await GetUserWithRolesAsync(requestingUserId);
            var requesterRole = requesterRoles.FirstOrDefault() ?? SystemRoles.RegularUser;

            var targetRoles = await _userManager.GetRolesAsync(user);
            var targetRole = targetRoles.FirstOrDefault() ?? SystemRoles.RegularUser;

            bool isSelf = user.Id == requestingUserId;

            if (!isSelf && !RoleHierarchy.CanInteractWith(requesterRole, targetRole))
                throw new UnauthorizedAccessException(
                    $"Your role '{requesterRole}' cannot modify a user with role '{targetRole}'.");

            if (!string.IsNullOrEmpty(request.Role))
            {
                var roleExists = await _roleManager.RoleExistsAsync(request.Role);
                if (!roleExists)
                    throw new ValidationException($"The role '{request.Role}' does not exist.");

                if (!RoleHierarchy.CanInteractWith(requesterRole, request.Role))
                    throw new UnauthorizedAccessException(
                        $"You cannot assign a higher-level role ('{request.Role}').");

                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);

                var roleResult = await _userManager.AddToRoleAsync(user, request.Role);
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    throw new ValidationException($"Role update failed: {errors}");
                }
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

            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                var result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new ValidationException($"Password reset failed: {errors}");
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                throw new ValidationException($"User update failed: {errors}");
            }

            return new UpdateResponse { UserId = user.Id };
        }
        catch (Exception ex)
        {
            throw new BadRequestException($"Error: {ex.Message}");
        }
    }

    public async Task<bool> DeleteUserAsync(string userId, string requestingUserId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new NotFoundException("User not found", userId);

            var userRoles = await _userManager.GetRolesAsync(user);
            var targetRole = userRoles.FirstOrDefault() ?? SystemRoles.RegularUser;

            var (_, requesterRoles) = await GetUserWithRolesAsync(requestingUserId);
            var requesterRole = requesterRoles.FirstOrDefault() ?? SystemRoles.RegularUser;

            bool isSelf = user.Id == requestingUserId;

            if (!isSelf && !RoleHierarchy.CanInteractWith(requesterRole, targetRole))
            {
                throw new UnauthorizedAccessException(
                    $"Your role '{requesterRole}' does not have permission to delete users with role '{targetRole}'.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new BadRequestException($"Unable to delete user: {errors}");
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new BadRequestException($"Error deleting user: {ex.Message}");
        }
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