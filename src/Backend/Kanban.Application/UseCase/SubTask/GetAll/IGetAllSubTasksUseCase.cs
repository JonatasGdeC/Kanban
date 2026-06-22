using Kanban.Communication.Responses.SubTask;

namespace Kanban.Application.UseCase.SubTask.GetAll;

public interface IGetAllSubTasksUseCase
{
    Task<GetAllSubTasksResponse> Execute(Guid taskId);
}
