using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Kanban.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace Kanban.Infrastructure.Security.Tokens;

public class VerifyTokenResetCode(string signingKey) : IVerifyTokenResetCode
{
    public Guid? GetUserId(string token)
    {
        try
        {
            JwtSecurityTokenHandler tokenHandler = new();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token: token,
                validationParameters: new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(s: signingKey)),
                    ClockSkew = TimeSpan.Zero
                },
                validatedToken: out _);

            bool purposeIsRight = principal.Claims.Any(predicate: c => c.Type == "purpose" && c.Value == "password-reset");

            if (!purposeIsRight)
            {
                return null;   
            }

            string? userId = principal.Claims.FirstOrDefault(predicate: c => c.Type == ClaimTypes.Sid)?.Value;
            if (!Guid.TryParse(input: userId, result: out Guid parsedUserId))
            {
                return null;
            }

            return parsedUserId;
        }
        catch
        {
            return null;
        }
    }
}