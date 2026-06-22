using Kanban.Communication.Requests.Board;

namespace Kanban.Application.UseCase.Board.Update;

public interface IUpdateBoardUseCase
{
    Task Execute(Guid id, RegisterBoardRequest request);
}