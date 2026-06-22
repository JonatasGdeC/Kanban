namespace Kanban.Domain.Repositories.User;
using Entities;

public interface IUserReadRepository
{
    Task<User?> GetByEmail(string email);
}