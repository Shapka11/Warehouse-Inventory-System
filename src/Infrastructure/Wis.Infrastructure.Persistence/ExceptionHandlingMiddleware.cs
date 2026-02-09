using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Infrastructure.Persistence;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

        var code = HttpStatusCode.InternalServerError;
        var message = "Internal server error. Please try again later.";

        switch (exception)
        {
            case { } ex when IsDatabaseException(ex):
                code = HttpStatusCode.ServiceUnavailable;
                message = "Database is temporarily unavailable. Please try again later.";
                break;

            case KeyNotFoundException:
                code = HttpStatusCode.NotFound;
                message = exception.Message;
                break;

            case ArgumentException or InvalidOperationException:
                code = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;
        }

        var response = new { error = message };
        var payload = JsonSerializer.Serialize(response);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        await context.Response.WriteAsync(payload);
    }

    private static bool IsDatabaseException(Exception? ex)
    {
        if (ex == null) return false;

        return ex switch
        {
            SocketException => true,
            NpgsqlException nex => nex.InnerException is SocketException || nex.IsTransient,
            DbUpdateException => IsDatabaseException(ex.InnerException),
            TimeoutException => true,
            _ => IsDatabaseException(ex.InnerException)
        };
    }
}