using Kanban.Communication.Dtos;

namespace Kanban.Communication.Responses.Board;

public record GetAllBoardsResponse
{
    public List<BoardDto> ListBoards { get; init; } = [];
}