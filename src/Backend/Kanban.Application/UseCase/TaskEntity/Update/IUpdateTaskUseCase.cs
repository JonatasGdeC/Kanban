using Kanban.Communication.Requests.Task;

namespace Kanban.Application.UseCase.TaskEntity.Update;

public interface IUpdateTaskUseCase
{
    Task Execute(Guid id, UpdateTaskRequest request);
}
