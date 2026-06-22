using Kanban.Communication.Requests.User;

namespace Kanban.Application.UseCase.User.ResetPassword;

public interface IResetPassword
{
    Task Execute(ResetPasswordRequest request);
}