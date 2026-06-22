using Kanban.Communication.Requests.User;
using Kanban.Communication.Responses.User;

namespace Kanban.Application.UseCase.User.Login;

public interface ILoginUseCase
{
    Task<LoginResponse> Execute(LoginRequest request);
}
