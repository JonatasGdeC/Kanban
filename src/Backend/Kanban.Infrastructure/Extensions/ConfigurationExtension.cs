using Microsoft.Extensions.Configuration;

namespace Kanban.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static bool IsTestEnvironment(this IConfiguration configuration) => configuration.GetValue<bool>(key: "InMemoryTest");
}