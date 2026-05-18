using System.Net;
using System.Text.Json;

namespace SearchEngine.Api.Middlewares;

public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var statusCode = ex switch
            {
                KeyNotFoundException => HttpStatusCode.NotFound,
                ArgumentException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                error = ex.Message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
