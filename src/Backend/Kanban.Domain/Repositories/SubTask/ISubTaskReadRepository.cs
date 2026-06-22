namespace Kanban.Domain.Repositories.SubTask;
using Entities;

public interface ISubTaskReadRepository
{
    Task<List<SubTask>> GetAll(Guid taskId, Guid userId);
    Task<SubTask?> GetById(Guid id, Guid userId);
}