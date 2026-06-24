using Fluxor;
using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.Board.State;

[FeatureState]
public record BoardState
{
    public bool IsLoading { get; init; }
    public BoardDto? Board { get; init; }
}
