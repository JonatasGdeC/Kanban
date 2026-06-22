using Kanban.Communication.Requests.SubTask;

namespace Kanban.Application.UseCase.SubTask.Update;

public interface IUpdateSubTaskUseCase
{
    Task Execute(Guid id, UpdateSubTaskRequest request);
}
