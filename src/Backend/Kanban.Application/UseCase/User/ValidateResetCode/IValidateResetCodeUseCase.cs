using Kanban.Communication.Requests.User;
using Kanban.Communication.Responses.User;

namespace Kanban.Application.UseCase.User.ValidateResetCode;

public interface IValidateResetCodeUseCase
{
    Task<ValidateResetCodeResponse> Execute(ValidateResetCodeRequest request);
}