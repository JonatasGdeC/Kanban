using Kanban.Domain.Entities;

namespace Kanban.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}