using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Synaptics.Application.Interfaces.Services;
using Synaptics.Infrastructure.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Synaptics.Infrastructure.Services;

public class JWTTokenService : IJWTTokenService
{
    readonly IConfiguration _configuration;

    public JWTTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(IEnumerable<Claim> claims)
    {
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(GetSecretKey()));
        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(
            issuer: GetIssuer(),
            audience: GetAudience(),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GetAudience() => _configuration["JWT:Audience"] ?? throw new AudienceNotFoundException();

    public string GetIssuer() => _configuration["JWT:Issuer"] ?? throw new IssuerNotFoundException();

    public string GetSecretKey() => _configuration["JWT:SecretKey"] ?? throw new SecretKeyNotFoundException();
}