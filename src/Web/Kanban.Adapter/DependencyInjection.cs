using Kanban.Adapter.Auth;
using Kanban.Adapter.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Kanban.Adapter;

public static class DependencyInjection
{
    public static void AddAdapter(this IServiceCollection services, WebAssemblyHostBuilder builder)
    {
        string apiBaseUrl = builder.Configuration[key: "ApiBaseUrl"] ?? "http://localhost:5062/";
        services.AddScoped(implementationFactory: _ => new HttpClient { BaseAddress = new Uri(uriString: apiBaseUrl) });

        services.AddScoped<UserServiceApi>();
        services.AddScoped<BoardServiceApi>();
        services.AddScoped<ColumnServiceApi>();
        services.AddScoped<TaskServiceApi>();
        services.AddScoped<SubTaskServiceApi>();

        services.AddAuthorizationCore();
        services.AddScoped<CookieAuthenticationStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(implementationFactory: sp => sp.GetRequiredService<CookieAuthenticationStateProvider>());
    }
}
