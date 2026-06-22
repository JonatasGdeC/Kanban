using Kanban.Domain.Entities;

namespace Kanban.Domain.Security.Tokens;

public interface IPasswordResetTokenGenerator
{
    string Generate(User user);
}