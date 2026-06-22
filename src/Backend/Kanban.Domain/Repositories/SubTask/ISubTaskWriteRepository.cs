namespace Kanban.Domain.Repositories.SubTask;
using Entities;
using System.Threading.Tasks;

public interface ISubTaskWriteRepository
{
    Task Add(SubTask subTask);
    void Update(SubTask subTask);
    void Delete(SubTask subTask);
    Task<SubTask?> GetById(Guid id, Guid userId);
}