using Kanban.Communication.Dtos;

namespace Kanban.Communication.Responses.Column;

public record GetAllColumnsResponse
{
    public List<ColumnDto> ListColumns { get; init; } = [];
}
