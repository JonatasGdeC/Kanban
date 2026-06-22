using Kanban.Communication.Requests.User;

namespace Kanban.Application.UseCase.User.UpdatePassword;

public interface IUpdatePasswordUseCase
{
    Task Execute(UpdatePasswordRequest request);
}