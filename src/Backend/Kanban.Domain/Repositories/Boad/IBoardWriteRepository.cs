using Kanban.Domain.Entities;

namespace Kanban.Domain.Repositories.Boad;
using System.Threading.Tasks;

public interface IBoardWriteRepository
{
    Task Add(Board board);
    void Update(Board board);
    void Delete(Board board);
    Task<Board?> GetById(Guid id, Guid userId);
}