using Kanban.Communication.Dtos;

namespace Kanban.Communication.Responses.Board;

public class GetBoardByIdResponse
{
    public required BoardDto Board { get; init; }
    public List<ColumnDto> Columns { get; init; } = [];
}