using Fluxor;
using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.Board.State;

[FeatureState]
public record BoardListState
{
    public bool IsLoading { get; init; }
    public List<BoardDto> Boards { get; init; } = [];
}