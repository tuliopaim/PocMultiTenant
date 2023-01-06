using Microsoft.IdentityModel.Tokens;
using PocMultiTenant.Api.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PocMultiTenant.Api.Infrastructure.Auth;

public static class TokenGenerator
{
    public static string GenerateToken(GenerateTokenDto dto)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new List<Claim>()
        {
            new Claim(PocClaims.Id, dto.User.Id.ToString()),
            new Claim(PocClaims.Tenant, dto.User.Tenant.ToString()),
        };

        var key = Encoding.ASCII.GetBytes(dto.Secret);
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(dto.ExpirationTime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        return tokenHandler.WriteToken(token);
    }
}
