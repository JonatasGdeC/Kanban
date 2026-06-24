using Fluxor;
using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.SubTask.State;

[FeatureState]
public record SubTaskListState
{
    public bool IsLoading { get; init; }
    public Dictionary<Guid, List<SubTaskDto>> SubTasksByTaskId { get; init; } = [];
}
