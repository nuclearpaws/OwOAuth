using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace OwOAuth.Core.Middleware;

internal sealed class PerformanceMonitor<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger _logger;

    public PerformanceMonitor(
        ILogger<PerformanceMonitor<TRequest, TRequest>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var stopWatch = new Stopwatch();

        stopWatch.Reset();
        stopWatch.Start();
        var response = await next();
        stopWatch.Stop();

        _logger.LogInformation("UseCase for request {requestName} took {ms}ms.", typeof(TRequest).FullName, stopWatch.ElapsedMilliseconds);

        return response;
    }
}
