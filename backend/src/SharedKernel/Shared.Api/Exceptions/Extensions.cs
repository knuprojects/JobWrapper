using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.Api.Exceptions.ExceptionsMapper;
using Shared.Api.Exceptions.Middlewares;

namespace Shared.Api.Exceptions;

public static class Extensions
{
    public static IServiceCollection AddErrorHandling(this IServiceCollection services)
        => services
                  .AddSingleton<ErrorHandlerMiddleware>()
                  .AddSingleton<IExceptionMapper, ExceptionMapper>();   
    
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
        => app.UseMiddleware<ErrorHandlerMiddleware>();
}
