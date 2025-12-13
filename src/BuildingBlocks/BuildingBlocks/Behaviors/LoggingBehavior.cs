using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var timer = new Stopwatch();

        logger.LogInformation("[START] Handle request = {Request} - Response = {Response} - Request Data = {RequestData}", typeof(TRequest).Name, typeof(TResponse).Name, request);

        timer.Start();
        var response = await next();
        timer.Stop();

        var timeElapsed = timer.Elapsed;

        if (timeElapsed.Seconds > 3)
        {
            logger.LogWarning("[PERFORMANCE] The request {Request} took {Elapsed} ms to process.", typeof(TRequest).Name, timeElapsed.Seconds);
        }

        logger.LogInformation("[END] Handle request = {Request} - Response = {Response} with {ElapsedMiliseconds} miliseconds", typeof(TRequest).Name, typeof(TResponse).Name, timeElapsed.Milliseconds);

        return response;
    }
}
