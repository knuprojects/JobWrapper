using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Api.Exceptions.ExceptionsMapper;
using System.Net;

namespace Shared.Api.Exceptions.Middlewares;

internal sealed class ErrorHandlerMiddleware : IMiddleware
{
    private readonly IExceptionMapper _exceptionMapper;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(
        IExceptionMapper exceptionMapper,
        ILogger<ErrorHandlerMiddleware> logger)
    {
        _exceptionMapper = exceptionMapper;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await HandleErrorAsync(context, exception);
        }
    }

    private async Task HandleErrorAsync(HttpContext context, Exception exception)
    {
        var errorResponse = _exceptionMapper.Map(exception);

        context.Response.StatusCode = (int)(errorResponse?.StatusCode ?? HttpStatusCode.InternalServerError);

        var response = errorResponse?.Response;

        if (response is null) return;

        await context.Response.WriteAsync((string)response);
    }
}
