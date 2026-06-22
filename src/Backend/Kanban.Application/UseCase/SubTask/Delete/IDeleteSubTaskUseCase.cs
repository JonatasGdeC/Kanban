namespace Kanban.Application.UseCase.SubTask.Delete;

public interface IDeleteSubTaskUseCase
{
    Task Execute(Guid id);
}
