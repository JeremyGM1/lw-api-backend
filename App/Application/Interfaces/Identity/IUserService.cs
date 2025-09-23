using Application.Features.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Identity
{
    public interface IUserService
    {
        Task<RegistrationResponse> Register(RegistrationRequest request);
        Task<UpdateResponse> Update(UpdateRequest request);
        Task<bool> DeleteUserAsync(string userId);
        Task<UserProfileResponse> GetCurrentUserProfileAsync(string userId);

    }
}
