using Fluxor;
using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.Task.State;

[FeatureState]
public record TaskState
{
    public bool IsLoading { get; init; }
    public TaskDto? Task { get; init; }
}
