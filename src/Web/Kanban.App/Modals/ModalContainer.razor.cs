using Kanban.App.Modals.Board.AddBoard;
using Kanban.App.Modals.Board.DeleteBoard;
using Kanban.App.Modals.Board.EditBoard;
using Kanban.App.Modals.Task.AddTask;
using Kanban.App.Modals.Task.DeleteTask;
using Kanban.App.Modals.Task.EditTask;
using Kanban.App.Modals.Task.ViewTask;
using Kanban.App.Modals.User.EditProfile;
using Kanban.App.UseState;

namespace Kanban.App.Modals;

public partial class ModalContainer : IDisposable
{
    protected override void OnInitialized() => ModalUseState.OnChange += StateHasChanged;

    private Type? CurrentModal => ModalUseState.Current switch
    {
        ModalUseState.ModalType.AddBoard     => typeof(AddBoard),
        ModalUseState.ModalType.EditBoard    => typeof(EditBoard),
        ModalUseState.ModalType.DeleteBoard  => typeof(DeleteBoard),
        ModalUseState.ModalType.AddTask      => typeof(AddTask),
        ModalUseState.ModalType.EditTask     => typeof(EditTask),
        ModalUseState.ModalType.DeleteTask   => typeof(DeleteTask),
        ModalUseState.ModalType.ViewTask     => typeof(ViewTask),
        ModalUseState.ModalType.EditProfile  => typeof(EditProfile),
        _                                    => null
    };
    
    public void Dispose() => ModalUseState.OnChange -= StateHasChanged;
}
