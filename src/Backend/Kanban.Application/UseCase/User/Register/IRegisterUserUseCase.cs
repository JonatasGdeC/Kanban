using Kanban.Communication.Requests.User;
using Kanban.Communication.Responses.User;

namespace Kanban.Application.UseCase.User.Register;

public interface IRegisterUserUseCase
{
    Task<RegisteredUserResponse> Execute(RegisterUserRequest request);
}
