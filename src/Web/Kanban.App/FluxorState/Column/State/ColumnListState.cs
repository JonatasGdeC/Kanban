using Fluxor;
using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.Column.State;

[FeatureState]
public record ColumnListState
{
    public bool IsLoading { get; init; }
    public List<ColumnDto> Columns { get; init; } = [];
}
