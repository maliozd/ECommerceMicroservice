using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler
{
    public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError("Error message: {Message},Time : {time}, StackTrace: {StackTrace}",
                              exception.Message, exception.StackTrace, DateTime.UtcNow);


            (string Detail, string Title, int StatusCode) = exception switch
            {
                InternalServerException => (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status500InternalServerError),

                ValidationException => (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status400BadRequest),

                BadRequestException => (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status400BadRequest),

                NotFoundException => (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status404NotFound),

                _ => (
                     exception.Message,
                     exception.GetType().Name,
                     StatusCodes.Status500InternalServerError)
            };

            var problemDetails = new ProblemDetails()
            {
                Title = Title,
                Detail = Detail,
                Status = StatusCode,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions.Add("TraceID", httpContext.TraceIdentifier);

            if (exception is ValidationException validationException)
            {
                problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
            }

            httpContext.Response.StatusCode = StatusCode;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}