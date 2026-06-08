using DZDDashboard.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Api.Filters;

public class ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var ex      = context.Exception;
        var request = context.HttpContext.Request;

        context.Result = ex switch
        {
            EntityNotFoundException e      => LogAndMap(e, StatusCodes.Status404NotFound,             "Not Found",           e.Message, isWarning: true),
            DomainValidationException e    => LogAndMap(e, StatusCodes.Status400BadRequest,           "Validation Error",    e.Message, isWarning: true),
            DomainConflictException e      => LogAndMap(e, StatusCodes.Status409Conflict,             "Conflict",            e.Message, isWarning: true),
            // HTTP 403 Forbidden — caller is authenticated but not authorised.
            // HTTP 401 means "not authenticated"; using 401 for authorisation failures causes login-redirect loops.
            UnauthorizedAccessException e  => LogAndMap(e, StatusCodes.Status403Forbidden,            "Forbidden",           e.Message, isWarning: true),
            KeyNotFoundException e         => LogAndMap(e, StatusCodes.Status404NotFound,             "Not Found",           e.Message, isWarning: true),
            // InvalidOperationException intentionally absent — thrown by EF Core / ASP.NET internals and
            // must NOT map to 400. Use DomainValidationException for business-rule violations instead.
            DbUpdateConcurrencyException e => LogAndMap(e, StatusCodes.Status409Conflict,             "Conflict",            "The resource was modified by another request. Please retry.", isWarning: true),
            _                              => LogAndMap(ex, StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occurred. Please contact support.", isWarning: false)
        };

        context.ExceptionHandled = true;

        ObjectResult LogAndMap(Exception e, int statusCode, string title, string detail, bool isWarning)
        {
            if (isWarning)
                logger.LogWarning(e, "{Method} {Path} → {StatusCode} {Title}", request.Method, request.Path, statusCode, title);
            else
                logger.LogError(e, "Unhandled exception for {Method} {Path}", request.Method, request.Path);

            return Problem(statusCode, title, detail);
        }
    }

    private static ObjectResult Problem(int statusCode, string title, string detail)
        => new(new ProblemDetails { Status = statusCode, Title = title, Detail = detail })
        {
            StatusCode = statusCode
        };
}
