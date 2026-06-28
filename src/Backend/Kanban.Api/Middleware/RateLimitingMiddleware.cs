using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace Kanban.Api.Middleware;

public static class RateLimitingMiddleware
{
    public static void KanbanApiRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(configureOptions: options =>
        {
            options.AddFixedWindowLimiter(policyName: "login", configureOptions: opt =>
            {
                opt.PermitLimit = 10;
                opt.Window = TimeSpan.FromMinutes(minutes: 1);
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                opt.QueueLimit = 0;
            });

            options.AddFixedWindowLimiter(policyName: "forgot-password", configureOptions: opt =>
            {
                opt.PermitLimit = 5;
                opt.Window = TimeSpan.FromMinutes(minutes: 15);
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                opt.QueueLimit = 0;
            });

            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.Headers.RetryAfter = "60";
                await context.HttpContext.Response.WriteAsJsonAsync(
                    value: new { errorMessages = new[] { "Too many requests. Try again later." } },
                    cancellationToken: cancellationToken);
            };
        });
    }
}