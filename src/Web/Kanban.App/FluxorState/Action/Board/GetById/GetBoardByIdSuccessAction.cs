using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.Action.Board.GetById;

public record GetBoardByIdSuccessAction(BoardDto Board);