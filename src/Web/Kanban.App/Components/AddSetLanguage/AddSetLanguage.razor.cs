using Microsoft.JSInterop;

namespace Kanban.App.Components.AddSetLanguage;

public partial class AddSetLanguage
{
    private record LangOption(string Code, string Label, string Flag);

    private readonly LangOption[] _languages =
    [
        new(Code: "en", Label: "English",    Flag: "https://www.worldometers.info/images/flags/original/gb.webp"),
        new(Code: "pt", Label: "Português",  Flag: "https://www.worldometers.info/images/flags/original/br.webp"),
        new(Code: "es", Label: "Español",    Flag: "https://www.worldometers.info/images/flags/original/es.webp")
    ];

    private string _currentCulture = "en";

    protected override async Task OnInitializedAsync()
    {
        string? stored = await JsRuntime.InvokeAsync<string?>(identifier: "localStorage.getItem", args: ["culture"]);
        _currentCulture = stored ?? System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
    }

    private async Task NavigateToCulture(string culture)
    {
        if (_currentCulture == culture)
        {
            return;
        }

        await JsRuntime.InvokeVoidAsync(identifier: "localStorage.setItem", args: ["culture", culture]);
        NavigationManager.NavigateTo(uri: NavigationManager.Uri, forceLoad: true);
    }
}
