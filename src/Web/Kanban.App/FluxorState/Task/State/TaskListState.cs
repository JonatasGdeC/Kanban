using Fluxor;
using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.Task.State;

[FeatureState]
public record TaskListState
{
    public bool IsLoading { get; init; }
    public List<TaskDto> Tasks { get; init; } = [];
}
