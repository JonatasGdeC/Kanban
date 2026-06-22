namespace Kanban.Application.UseCase.TaskEntity.Delete;

public interface IDeleteTaskUseCase
{
    Task Execute(Guid id);
}
