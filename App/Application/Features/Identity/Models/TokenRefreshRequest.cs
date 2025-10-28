using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Models
{
    public class TokenRefreshRequest
    {
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
    }
}
