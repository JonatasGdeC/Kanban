using Fluxor;
using Fluxor.Blazor.Web.Components;
using Kanban.App.FluxorState.State.Board;
using Microsoft.AspNetCore.Components;

namespace Kanban.App.Layout.HeaderLayout;

public partial class HeaderLayout : FluxorComponent
{
    [Inject] private IState<BoardState> BoardState { get; set; } = default!;

    private bool _mobileSidebar;
}