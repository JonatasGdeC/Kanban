using System.Globalization;
using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using Kanban.Adapter;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Kanban.App;
using Kanban.App.UseState;
using Microsoft.JSInterop;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args: args);
builder.Services.AddLocalization();
builder.RootComponents.Add<App>(selector: "#app");
builder.RootComponents.Add<HeadOutlet>(selector: "head::after");

builder.Services.AddScoped(implementationFactory: sp => new HttpClient { BaseAddress = new Uri(uriString: builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ModalUseState>();
builder.Services.AddScoped<DragTaskState>();
builder.Services.AddScoped<DragColumnState>();

builder.Services.AddFluxor(configure: options =>
{
    options.ScanAssemblies(assemblyToScan: typeof(Program).Assembly);
    options.UseReduxDevTools();
});

builder.Services.AddAdapter(builder: builder);

WebAssemblyHost host = builder.Build();
string? selectedCulture = await host.Services.GetRequiredService<IJSRuntime>().InvokeAsync<string?>(identifier: "localStorage.getItem", args: "culture");
string[] supportedCultures = ["pt", "en", "es"];
string cultureName = supportedCultures.Contains(value: selectedCulture) ? selectedCulture! : supportedCultures[0];
CultureInfo culture = new(name: cultureName);
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();