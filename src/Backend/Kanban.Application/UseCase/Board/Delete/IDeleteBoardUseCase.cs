namespace Kanban.Application.UseCase.Board.Delete;

public interface IDeleteBoardUseCase
{
    Task Execute(Guid id);
}