using Kanban.Domain.Repositories;

namespace Kanban.Infrastructure.DataAccess;

internal class UnitOfWork(KanbanDbContext context) : IUnitOfWork
{
    public async Task Commit()
    {
        await context.SaveChangesAsync();
    }
}