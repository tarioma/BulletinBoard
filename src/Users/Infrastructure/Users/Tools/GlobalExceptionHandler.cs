using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Users.Dal.Exceptions;

namespace Users.Tools;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly Dictionary<Type, HttpStatusCode> _exceptions = new()
    {
        { typeof(NotFoundException), HttpStatusCode.NotFound },
        { typeof(ArgumentException), HttpStatusCode.BadRequest }
    };

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken = default)
    {
        var statusCode = _exceptions.GetValueOrDefault(exception.GetType(), HttpStatusCode.InternalServerError);

        var problemDetails = new ProblemDetails
        {
            Title = "Ошибка",
            Status = (int)statusCode,
            Detail = _exceptions.ContainsKey(exception.GetType()) ? exception.Message : null
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}