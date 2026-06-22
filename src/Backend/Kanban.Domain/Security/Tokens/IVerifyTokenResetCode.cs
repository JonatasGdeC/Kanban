using Kanban.Domain.Entities;

namespace Kanban.Domain.Security.Tokens;

public interface IVerifyTokenResetCode
{
    Guid? GetUserId(string token);
}