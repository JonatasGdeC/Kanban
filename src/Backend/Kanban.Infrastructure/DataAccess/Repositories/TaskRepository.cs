using Kanban.Domain.Entities;
using Kanban.Domain.Repositories.Task;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Infrastructure.DataAccess.Repositories;

public class TaskRepository(KanbanDbContext context) : ITaskReadRepository, ITaskWriteRepository
{
    public async Task<List<TaskEntity>> GetAll(Guid columnId, Guid userId)
    {
        return await context.Tasks.AsNoTracking().Where(predicate: task => task.ColumnId == columnId && task.Column.Board.UserId == userId).ToListAsync();
    }

    public async Task Add(TaskEntity task)
    {
        await context.Tasks.AddAsync(entity: task);
    }

    public void Update(TaskEntity task)
    {
        context.Tasks.Update(entity: task);
    }

    public void Delete(TaskEntity task)
    {
        context.Tasks.Remove(entity: task);
    }

    async Task<TaskEntity?> ITaskWriteRepository.GetById(Guid id, Guid userId)
    {
        return await context.Tasks
            .FirstOrDefaultAsync(predicate: task => task.Id == id && task.Column.Board.UserId == userId);
    }

    async Task<TaskEntity?> ITaskReadRepository.GetById(Guid id, Guid userId)
    {
        return await context.Tasks.AsNoTracking()
            .Include(navigationPropertyPath: task => task.SubTasks)
            .FirstOrDefaultAsync(predicate: task => task.Id == id && task.Column.Board.UserId == userId);
    }

    public async Task<bool> ExistsTaskInPosition(Guid columnId, int position, Guid ignoreTaskId)
    {
        return await context.Tasks.AsNoTracking().AnyAsync(predicate: task => task.ColumnId == columnId && task.Order == position && task.Id != ignoreTaskId);
    }
}
