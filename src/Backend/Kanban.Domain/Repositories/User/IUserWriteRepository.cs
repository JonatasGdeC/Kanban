namespace Kanban.Domain.Repositories.User;
using Entities;
using System.Threading.Tasks;

public interface IUserWriteRepository
{
    Task Add(User user);
    void Update(User user);
    void Delete(User user);
    Task<User?> GetById(Guid id);
}