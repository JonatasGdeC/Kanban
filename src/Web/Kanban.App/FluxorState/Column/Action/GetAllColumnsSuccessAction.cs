using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.Column.Action;

public record GetAllColumnsSuccessAction(List<ColumnDto> Columns);
