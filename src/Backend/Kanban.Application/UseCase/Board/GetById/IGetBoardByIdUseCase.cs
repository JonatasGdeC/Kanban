using Kanban.Communication.Responses.Board;

namespace Kanban.Application.UseCase.Board.GetById;

public interface IGetBoardByIdUseCase
{
    Task<GetBoardByIdResponse> Execute(Guid id);
}