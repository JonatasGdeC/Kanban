namespace Kanban.Domain.Repositories.Column;
using System.Threading.Tasks;
using Entities;

public interface IColumWriteRepository
{
    Task Add(Column column);
    Task Update(Column column);
    void Delete(Column column);
    Task<Column?> GetById(Guid id, Guid userId);
}