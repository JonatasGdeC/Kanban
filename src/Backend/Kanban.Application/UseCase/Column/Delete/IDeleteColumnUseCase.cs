namespace Kanban.Application.UseCase.Column.Delete;

public interface IDeleteColumnUseCase
{
    Task Execute(Guid id);
}
