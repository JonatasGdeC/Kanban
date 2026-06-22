using Kanban.Communication.Requests.User;

namespace Kanban.Application.UseCase.User.Update;

public interface IUpdateUserUseCase
{
    Task Execute(UpdateUserRequest request);
}
