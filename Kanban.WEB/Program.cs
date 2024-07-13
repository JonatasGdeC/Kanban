using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Kanban.WEB;
using Kanban.WEB.Service;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

builder.Services.AddTransient<BoardApi>();
builder.Services.AddTransient<ColumnsApi>();

builder.Services.AddHttpClient("API", client =>
{
  client.BaseAddress = new Uri(builder.Configuration["APIServer:Url"]!);
  client.DefaultRequestHeaders.Add("Accept", "application/json");
});

await builder.Build().RunAsync();
