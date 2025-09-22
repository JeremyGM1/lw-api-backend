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
        Task<List<UserDto>> GetEmployees();
        Task<UserDto> GetEmployee(string userId);
        
    }
}
