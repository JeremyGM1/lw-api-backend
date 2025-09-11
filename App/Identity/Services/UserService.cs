using Application.Features.Identity.Models;
using Application.Interfaces.Identity;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> user)
        {
            
        }
        public Task<UserDto> GetEmployee(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserDto>> GetEmployees()
        {
            throw new NotImplementedException();
        }
    }
}
