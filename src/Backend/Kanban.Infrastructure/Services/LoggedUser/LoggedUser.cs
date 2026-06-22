using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Kanban.Domain.Entities;
using Kanban.Domain.Security.Tokens;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Infrastructure.Services.LoggedUser;

public class LoggedUser(KanbanDbContext context, ITokenProvider tokenProvider) : ILoggedUser
{
    public async Task<User> Get()
    {
        string token = tokenProvider.TokenOnRequest();
        JwtSecurityTokenHandler tokenHandler = new();
        JwtSecurityToken? jwtSecurityToken = tokenHandler.ReadJwtToken(token: token);
        string userId = jwtSecurityToken.Claims.First(predicate: claim => claim.Type == ClaimTypes.Sid).Value;
        
        return await context.Users.AsNoTracking().FirstAsync(predicate: user => user.Id == Guid.Parse(userId));
    }
}