using Kanban.Communication.Requests.User;

namespace Kanban.Application.UseCase.User.ForgotPassword;

public interface IForgotPassword
{
    Task Execute(ForgotPasswordRequest request);
}