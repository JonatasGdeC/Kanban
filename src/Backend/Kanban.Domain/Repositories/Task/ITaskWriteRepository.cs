using Kanban.Domain.Entities;

namespace Kanban.Domain.Repositories.Task;
using System.Threading.Tasks;

public interface ITaskWriteRepository
{
    Task Add(TaskEntity task);
    void Update(TaskEntity task);
    void Delete(TaskEntity task);
    Task<TaskEntity?> GetById(Guid id, Guid userId);
}