using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Kanban.Domain.Entities;
using Kanban.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace Kanban.Infrastructure.Security.Tokens;

public class JwtTokenGenerator(uint expirationTimeMinutes, string signingKey) : IAccessTokenGenerator
{
    public string Generate(User user)
    {
        List<Claim> claims = [
            new(type: ClaimTypes.Name, value: user.Name),
            new(type: ClaimTypes.Sid, value: user.Id.ToString()),
        ];
        
        SecurityTokenDescriptor tokenDescription = new()
        {
            Expires = DateTime.UtcNow.AddMinutes(value: expirationTimeMinutes),
            SigningCredentials = new SigningCredentials(key: SecurityKey(), algorithm: SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(claims: claims)
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken? securityToken = tokenHandler.CreateToken(tokenDescriptor: tokenDescription);

        return tokenHandler.WriteToken(token: securityToken);
    }

    private SymmetricSecurityKey SecurityKey()
    {
        byte[] key = Encoding.UTF8.GetBytes(s: signingKey);
        return new SymmetricSecurityKey(key: key);
    }
}