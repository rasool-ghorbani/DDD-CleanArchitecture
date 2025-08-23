using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
      where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogInformation("Handling request: {RequestName}", requestName);
            var startTime = DateTime.Now;

            var response = await next();

            var elapsed = DateTime.Now - startTime;
            _logger.LogInformation("Handled request: {RequestName} in {ElapsedMilliseconds} ms", requestName, elapsed.TotalMilliseconds);

            return response;
        }
    }
}
