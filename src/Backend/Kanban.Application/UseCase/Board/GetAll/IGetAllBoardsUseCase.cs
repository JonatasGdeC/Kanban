using Kanban.Communication.Responses.Board;

namespace Kanban.Application.UseCase.Board.GetAll;

public interface IGetAllBoardsUseCase
{
    Task<GetAllBoardsResponse> Execute();
}