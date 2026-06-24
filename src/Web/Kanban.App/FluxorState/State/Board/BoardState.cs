using Fluxor;
using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.State.Board;

[FeatureState]
public record BoardState
{
    public bool IsLoading { get; init; }
    public BoardDto? Board { get; init; }
}

[FeatureState]
public record BoardListState
{
    public bool IsLoading { get; init; }
    public List<BoardDto> Boards { get; init; } = [];
}