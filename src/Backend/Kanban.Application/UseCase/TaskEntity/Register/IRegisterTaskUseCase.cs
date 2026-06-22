using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Task;

namespace Kanban.Application.UseCase.TaskEntity.Register;

public interface IRegisterTaskUseCase
{
    Task<TaskDto> Execute(Guid columnId, RegisterTaskRequest request);
}
