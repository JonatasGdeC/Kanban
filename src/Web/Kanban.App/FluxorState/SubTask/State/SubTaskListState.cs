using Fluxor;
using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.SubTask.State;

[FeatureState]
public record SubTaskListState
{
    public bool IsLoading { get; init; }
    public List<SubTaskDto> SubTasks { get; init; } = [];
}
