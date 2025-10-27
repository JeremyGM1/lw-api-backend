using Application.Features.Identity.Models;

namespace Application.Interfaces.Identity;

public interface IUserService
{
    Task<RegistrationResponse> Register(RegistrationRequest request);

    Task<UpdateResponse> Update(UpdateRequest request);

    Task<bool> DeleteUserAsync(string userId);

    Task<UserProfileResponse> GetCurrentUserProfileAsync(string userId);

    Task<UpdateResponse> AdminUpdateRequest(AdminUpdateRequest request);
}