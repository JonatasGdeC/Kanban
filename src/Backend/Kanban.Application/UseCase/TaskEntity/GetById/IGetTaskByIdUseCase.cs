using Kanban.Communication.Responses.Task;

namespace Kanban.Application.UseCase.TaskEntity.GetById;

public interface IGetTaskByIdUseCase
{
    Task<GetTaskByIdResponse> Execute(Guid id);
}
