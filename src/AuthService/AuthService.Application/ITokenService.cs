using NC.AuthService.Domain;
using System.Security.Claims;

namespace NC.AuthService.Contracts;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    SecurityToken GenerateRefreshToken(Guid userId, string ipAddress);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}