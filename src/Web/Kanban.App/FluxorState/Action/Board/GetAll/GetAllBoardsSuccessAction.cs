using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.Action.Board.GetAll;

public record GetAllBoardsSuccessAction(List<BoardDto> Boards);