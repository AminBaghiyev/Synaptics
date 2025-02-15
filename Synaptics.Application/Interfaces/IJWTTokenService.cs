using System.Security.Claims;

namespace Synaptics.Application.Interfaces;

public interface IJWTTokenService
{
    string GenerateToken(IEnumerable<Claim> claims);
    string GetSecretKey();
    string GetIssuer();
    string GetAudience();
}
