using Domain.AccessControl;

namespace Application.Interfaces;

public interface ITokenGenerator
{
    public string GenerateToken(User user);
}
public interface ITokenValidator
{
    public bool ValidateToken(string token);
}