using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Shared.Dal.Utils.Services;
using System.Text;

namespace Shared.Dal.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CachedAttribute : Attribute, IAsyncActionFilter
{
    private readonly int _seconds;

    public CachedAttribute(int seconds)
        => _seconds = seconds;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

        var key = GenerateCacheKeyFromRequest(context.HttpContext.Request);
        var response = await cacheService.GetAsync(key);

        if (!string.IsNullOrEmpty(response))
        {
            var contentResult = new ContentResult
            {
                Content = response,
                ContentType = "application/json",
                StatusCode = 200
            };

            context.Result = contentResult;
            return;
        }

        var executedContext = await next();

        if (executedContext.Result is ObjectResult result)
            await cacheService.SetData(key, result.Value, TimeSpan.FromSeconds(_seconds));
    }

    private string GenerateCacheKeyFromRequest(HttpRequest request)
    {
        var keyBuilder = new StringBuilder();

        keyBuilder.Append($"{request.Path}");

        foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
        {
            keyBuilder.Append($"|{key} - {value}");
        }

        return keyBuilder.ToString();
    }
}
