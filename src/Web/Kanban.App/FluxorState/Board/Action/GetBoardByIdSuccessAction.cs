using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.Board.Action;

public record GetBoardByIdSuccessAction(BoardDto Board);