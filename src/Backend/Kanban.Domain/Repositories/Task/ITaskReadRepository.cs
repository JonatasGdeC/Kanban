using Kanban.Domain.Entities;

namespace Kanban.Domain.Repositories.Task;

public interface ITaskReadRepository
{
    Task<List<TaskEntity>> GetAll(Guid columnId, Guid userId);
    Task<TaskEntity?> GetById(Guid id, Guid userId);
    Task<bool> ExistsTaskInPosition(Guid columnId, int position, Guid ignoreTaskId);
}