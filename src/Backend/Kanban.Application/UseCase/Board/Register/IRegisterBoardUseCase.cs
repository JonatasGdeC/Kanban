using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Board;

namespace Kanban.Application.UseCase.Board.Register;

public interface IRegisterBoardUseCase
{
    Task<BoardDto> Execute(RegisterBoardRequest request);
}