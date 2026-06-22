namespace Kanban.Domain.Repositories.PasswordResetCode;
using System.Threading.Tasks;
using Entities;

public interface IPasswordResetCodeRepository
{
    Task Add(PasswordResetCode passwordResetCode);
    void Remove(PasswordResetCode passwordResetCode);
    void Update(PasswordResetCode passwordResetCode);
    Task<PasswordResetCode?> GetByUserId(Guid userId);
}