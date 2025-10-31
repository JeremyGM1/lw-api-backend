namespace Application.Features.Identity.Models
{
    public class TokenRefreshRequest
    {
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
    }
}
