using Kanban.Communication.Dtos;
using Microsoft.AspNetCore.Components;

namespace Kanban.App.Layout.HeaderLayout;

public partial class HeaderLayout
{
    [Parameter] public BoardDto? CurrentBoard { get; set; }
    private bool _mobileSidebar;
}