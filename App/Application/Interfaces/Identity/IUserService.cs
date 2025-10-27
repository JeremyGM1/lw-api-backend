using Application.Features.Identity.Models;

namespace Application.Interfaces.Identity;

public interface IUserService
{
    Task<RegistrationResponse> Register(RegistrationRequest request, string requestingUserId);

    Task<UpdateResponse> Update(UpdateRequest request, string requestingUserId);

    Task<bool> DeleteUserAsync(string userId, string requestingUserId);

    Task<UserProfileResponse> GetCurrentUserProfileAsync(string userId);

    Task<IEnumerable<UserProfileResponse>> GetAllUsersByRoleAsync(string requestingUserId);

    public Task<UpdateResponse> AdminUpdateRequest(AdminUpdateRequest request)
}