using Microsoft.AspNetCore.Components;

namespace Kanban.App.Components.AddLoading;

public partial class AddLoading
{
    [Parameter] public string WithContainer { get; set; } = "100%";
    [Parameter] public string HeightContainer { get; set; } = "50vh";
    [Parameter] public string WidthSpin { get; set; } = "48px";
    [Parameter] public string HeightSpin { get; set; } = "48px";
}