using Timer = System.Timers.Timer;

namespace Kanban.App.Pages.Login.Components.KanbanShowcase;

public partial class KanbanShowcase : IDisposable
{
    private int _activeSlide;
    private int _previousSlide = -1;
    private Timer? _timer;
    private ShowcaseSlide[] _slides = [];

    protected override void OnInitialized()
    {
        string L(string key) => LoginLocalizer[name: key];

        _slides =
        [
            new(
                Label: L(key: "SHOWCASE_S1_LABEL"),
                Title: L(key: "SHOWCASE_S1_TITLE"),
                Description: L(key: "SHOWCASE_S1_DESC"),
                Columns:
                [
                    new(Name: L(key: "SHOWCASE_S1_C1_NAME"), Color: "#635FC7", Cards: [L(key: "SHOWCASE_S1_C1_T1"), L(key: "SHOWCASE_S1_C1_T2"), L(key: "SHOWCASE_S1_C1_T3")]),
                    new(Name: L(key: "SHOWCASE_S1_C2_NAME"), Color: "#49C4E5", Cards: [L(key: "SHOWCASE_S1_C2_T1"), L(key: "SHOWCASE_S1_C2_T2")]),
                    new(Name: L(key: "SHOWCASE_S1_C3_NAME"), Color: "#67E2AE", Cards: [L(key: "SHOWCASE_S1_C3_T1"), L(key: "SHOWCASE_S1_C3_T2")])
                ]
            ),
            new(
                Label: L(key: "SHOWCASE_S2_LABEL"),
                Title: L(key: "SHOWCASE_S2_TITLE"),
                Description: L(key: "SHOWCASE_S2_DESC"),
                Columns:
                [
                    new(Name: L(key: "SHOWCASE_S2_C1_NAME"), Color: "#635FC7", Cards: [L(key: "SHOWCASE_S2_C1_T1"), L(key: "SHOWCASE_S2_C1_T2"), L(key: "SHOWCASE_S2_C1_T3")]),
                    new(Name: L(key: "SHOWCASE_S2_C2_NAME"), Color: "#F4A261", Cards: [L(key: "SHOWCASE_S2_C2_T1"), L(key: "SHOWCASE_S2_C2_T2")]),
                    new(Name: L(key: "SHOWCASE_S2_C3_NAME"), Color: "#67E2AE", Cards: [L(key: "SHOWCASE_S2_C3_T1"), L(key: "SHOWCASE_S2_C3_T2")])
                ]
            ),
            new(
                Label: L(key: "SHOWCASE_S3_LABEL"),
                Title: L(key: "SHOWCASE_S3_TITLE"),
                Description: L(key: "SHOWCASE_S3_DESC"),
                Columns:
                [
                    new(Name: L(key: "SHOWCASE_S3_C1_NAME"), Color: "#635FC7", Cards: [L(key: "SHOWCASE_S3_C1_T1"), L(key: "SHOWCASE_S3_C1_T2"), L(key: "SHOWCASE_S3_C1_T3")]),
                    new(Name: L(key: "SHOWCASE_S3_C2_NAME"), Color: "#49C4E5", Cards: [L(key: "SHOWCASE_S3_C2_T1")]),
                    new(Name: L(key: "SHOWCASE_S3_C3_NAME"), Color: "#67E2AE", Cards: [L(key: "SHOWCASE_S3_C3_T1"), L(key: "SHOWCASE_S3_C3_T2"), L(key: "SHOWCASE_S3_C3_T3")])
                ]
            )
        ];

        _timer = new Timer(interval: 4000);
        _timer.Elapsed += async (_, _) => await InvokeAsync(workItem: NextSlide);
        _timer.AutoReset = true;
        _timer.Start();
    }

    private void NextSlide()
    {
        _previousSlide = _activeSlide;
        _activeSlide = (_activeSlide + 1) % _slides.Length;
        StateHasChanged();
    }

    private void GoTo(int index)
    {
        _previousSlide = _activeSlide;
        _activeSlide = index;
        _timer?.Stop();
        _timer?.Start();
        StateHasChanged();
    }

    public void Dispose() => _timer?.Dispose();

    private record ShowcaseSlide(string Label, string Title, string Description, ShowcaseColumn[] Columns);
    private record ShowcaseColumn(string Name, string Color, string[] Cards);
}
