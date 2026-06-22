using System.Globalization;

namespace Kanban.Api.Middleware;

public class CultureMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        List<CultureInfo> supportedCultures = CultureInfo.GetCultures(types: CultureTypes.AllCultures).ToList();

        string? requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

        CultureInfo cultureInfo = new(name: "en");

        if (!string.IsNullOrWhiteSpace(value: requestedCulture) && supportedCultures.Exists(match: language => language.Name.Equals(value: requestedCulture)))
        {
            cultureInfo = new CultureInfo(name: requestedCulture);
        }

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await next(context: context);
    }
}