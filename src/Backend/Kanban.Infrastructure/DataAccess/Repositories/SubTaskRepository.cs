using Kanban.Domain.Entities;
using Kanban.Domain.Repositories.SubTask;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Infrastructure.DataAccess.Repositories;

public class SubTaskRepository(KanbanDbContext context) : ISubTaskReadRepository, ISubTaskWriteRepository
{
    public async Task<List<SubTask>> GetAll(Guid taskId, Guid userId)
    {
        return await context.SubTasks.AsNoTracking()
            .Where(predicate: subTask => subTask.TaskId == taskId && subTask.Task.Column.Board.UserId == userId)
            .ToListAsync();
    }

    public async Task Add(SubTask subTask)
    {
        await context.SubTasks.AddAsync(entity: subTask);
    }

    public void Update(SubTask subTask)
    {
        context.SubTasks.Update(entity: subTask);
    }

    public void Delete(SubTask subTask)
    {
        context.SubTasks.Remove(entity: subTask);
    }

    async Task<SubTask?> ISubTaskWriteRepository.GetById(Guid id, Guid userId)
    {
        return await context.SubTasks
            .FirstOrDefaultAsync(predicate: subTask => subTask.Id == id && subTask.Task.Column.Board.UserId == userId);
    }

    async Task<SubTask?> ISubTaskReadRepository.GetById(Guid id, Guid userId)
    {
        return await context.SubTasks.AsNoTracking()
            .FirstOrDefaultAsync(predicate: subTask => subTask.Id == id && subTask.Task.Column.Board.UserId == userId);
    }
}
