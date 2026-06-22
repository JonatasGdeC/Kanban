using Kanban.Domain.Entities;

namespace Kanban.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    Task<User> Get();
}