using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.Board.Action;

public record GetAllBoardsSuccessAction(List<BoardDto> Boards);