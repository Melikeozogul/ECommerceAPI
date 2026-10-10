
using System.Net;
using System.Text.Json;

namespace ECommerceAPI.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "İşlem gerçekleştirilemedi.");

            context.Response.StatusCode =
                (int)HttpStatusCode.BadRequest;

            context.Response.ContentType = "application/json";

            var response = new
            {
                statusCode = 400,
                message = ex.Message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Beklenmeyen bir hata oluştu.");

            if (context.Response.HasStarted)
                throw;

            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";

            var response = new
            {
                statusCode = 500,
                message = "Sunucu tarafında beklenmeyen bir hata oluştu."
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
