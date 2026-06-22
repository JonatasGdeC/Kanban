using Kanban.Communication.Responses.Task;

namespace Kanban.Application.UseCase.TaskEntity.GetAll;

public interface IGetAllTasksUseCase
{
    Task<GetAllTasksResponse> Execute(Guid columnId);
}
