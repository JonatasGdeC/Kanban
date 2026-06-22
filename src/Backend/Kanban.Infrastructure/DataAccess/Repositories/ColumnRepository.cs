using Kanban.Domain.Entities;
using Kanban.Domain.Repositories.Column;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Infrastructure.DataAccess.Repositories;

public class ColumnRepository(KanbanDbContext context) : IColumnReadRepository, IColumWriteRepository
{
    public async Task<List<Column>> GetAll(Guid boardId, Guid userId)
    {
        return await context.Columns.AsNoTracking().Where(predicate: column => column.BoardId == boardId && column.Board.UserId == userId).ToListAsync();
    }

    public async Task Add(Column column)
    {
        await context.Columns.AddAsync(entity: column);
    }

    public async Task Update(Column column)
    {
        int oldOrder = await context.Columns.Where(predicate: c => c.Id == column.Id).Select(selector: c => c.Order).FirstAsync();
        Column? existsColumnInPosition = await context.Columns.FirstOrDefaultAsync(predicate: c => c.BoardId == column.BoardId && c.Order == column.Order && c.Id != column.Id);

        if (existsColumnInPosition != null)
        {
            existsColumnInPosition.Order = oldOrder;
            context.Columns.Update(entity: existsColumnInPosition);
        }

        context.Columns.Update(entity: column);
    }

    public void Delete(Column column)
    {
        context.Columns.Remove(entity: column);
    }

    async Task<Column?> IColumWriteRepository.GetById(Guid id, Guid userId)
    {
        return await context.Columns.FirstOrDefaultAsync(predicate: column => column.Id == id && column.Board.UserId == userId);
    }

    async Task<Column?> IColumnReadRepository.GetById(Guid id, Guid userId)
    {
        return await context.Columns.AsNoTracking()
            .Include(navigationPropertyPath: column => column.Tasks)
            .FirstOrDefaultAsync(predicate: column => column.Id == id && column.Board.UserId == userId);
    }
}
