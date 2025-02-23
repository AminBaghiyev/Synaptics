using System.Security.Claims;

namespace Synaptics.Application.Interfaces.Services;

public interface IJWTTokenService
{
    string GenerateToken(IEnumerable<Claim> claims, TimeSpan expiry);
    string GetSecretKey();
    string GetIssuer();
    string GetAudience();
    string GenerateRefreshToken();
}
