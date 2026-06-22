using Kanban.Domain.Entities;
using Kanban.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Infrastructure.DataAccess.Repositories;

public class UserRepository(KanbanDbContext context) : IUserReadRepository, IUserWriteRepository
{
    public async Task<User?> GetByEmail(string email)
    {
        return await context.Users.AsNoTracking().FirstOrDefaultAsync(predicate: user => user.Email == email);
    }

    public async Task Add(User user)
    {
        await context.Users.AddAsync(entity: user);
    }

    public void Update(User user)
    {
        context.Users.Update(entity: user);
    }

    public void Delete(User user)
    {
        context.Users.Remove(entity: user);
    }

    public Task<User?> GetById(Guid id)
    {
        return context.Users.FirstOrDefaultAsync(predicate: user => user.Id == id);
    }
}