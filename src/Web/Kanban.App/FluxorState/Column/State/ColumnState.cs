using Fluxor;
using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.Column.State;

[FeatureState]
public record ColumnState
{
    public bool IsLoading { get; init; }
    public ColumnDto? Column { get; init; }
}

