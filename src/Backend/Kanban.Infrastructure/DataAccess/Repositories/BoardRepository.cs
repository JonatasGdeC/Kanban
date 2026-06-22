using Kanban.Domain.Entities;
using Kanban.Domain.Repositories.Boad;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Infrastructure.DataAccess.Repositories;

public class BoardRepository(KanbanDbContext context) : IBoardReadRepository, IBoardWriteRepository
{
    public async Task<List<Board>> GetAll(Guid userId)
    {
        return await context.Boards.AsNoTracking().Where(predicate: board => board.UserId == userId).ToListAsync();
    }

    public async Task Add(Board board)
    {
        await context.Boards.AddAsync(entity: board);
    }

    public void Update(Board board)
    {
        context.Boards.Update(entity: board);
    }

    public void Delete(Board board)
    {
        context.Boards.Remove(entity: board);
    }

    async Task<Board?> IBoardWriteRepository.GetById(Guid id, Guid userId)
    {
        return await context.Boards.FirstOrDefaultAsync(predicate: board => board.Id == id && board.UserId == userId);
    }

    public async Task<Board?> GetByTitle(string title, Guid userId)
    {
        return await context.Boards.AsNoTracking().FirstOrDefaultAsync(predicate: board =>
            board.Name == title && board.UserId == userId);
    }

    async Task<Board?> IBoardReadRepository.GetById(Guid id, Guid userId)
    {
        return await context.Boards
            .AsNoTracking()
            .Include(navigationPropertyPath: board => board.Columns)
            .FirstOrDefaultAsync(predicate: board => board.Id == id && board.UserId == userId);
    }
}