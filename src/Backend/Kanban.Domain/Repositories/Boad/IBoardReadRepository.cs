using Kanban.Domain.Entities;

namespace Kanban.Domain.Repositories.Boad;
using System.Threading.Tasks;

public interface IBoardReadRepository
{
    Task<List<Board>> GetAll(Guid userId);
    Task<Board?> GetById(Guid id, Guid userId);
    Task<Board?> GetByTitle(string title, Guid userId);
}