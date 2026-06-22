using Kanban.Communication.Dtos;

namespace Kanban.Communication.Responses.Column;

public class GetColumnByIdResponse
{
    public required ColumnDto Column { get; init; }
    public List<TaskDto> Tasks { get; init; } = [];
}
