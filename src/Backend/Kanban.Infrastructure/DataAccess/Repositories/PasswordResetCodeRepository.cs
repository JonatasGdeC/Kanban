using Kanban.Domain.Entities;
using Kanban.Domain.Repositories.PasswordResetCode;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Infrastructure.DataAccess.Repositories;

public class PasswordResetCodeRepository(KanbanDbContext context) : IPasswordResetCodeRepository
{
    public async Task Add(PasswordResetCode passwordResetCode)
    {
        context.PasswordResetCodes.Add(entity: passwordResetCode);
    }

    public void Remove(PasswordResetCode passwordResetCode)
    {
        context.PasswordResetCodes.Remove(entity: passwordResetCode);
    }

    public void Update(PasswordResetCode passwordResetCode)
    {
        context.PasswordResetCodes.Update(entity: passwordResetCode);
    }

    public async Task<PasswordResetCode?> GetByUserId(Guid userId)
    {
        return await context.PasswordResetCodes.FirstOrDefaultAsync(predicate: p => p.UserId == userId);
    }
}