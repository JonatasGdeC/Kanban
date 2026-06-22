using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.SubTask;

namespace Kanban.Application.UseCase.SubTask.Register;

public interface IRegisterSubTaskUseCase
{
    Task<SubTaskDto> Execute(Guid taskId, RegisterSubTaskRequest request);
}
