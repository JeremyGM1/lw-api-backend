using Microsoft.Extensions.Logging;
using LoggingService.Interfaces;

namespace LoggingService.Repository;

public class LoggerAdapter<T> : IAppLogger<T>
{
    private readonly ILogger<T> _logger;

    public LoggerAdapter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<T>();
    }
    public void LogInformation(string message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }
    public void LogWarning(string message, params object[] args)
    {
        _logger.LogWarning(message, args);
    }

    public void LogDebug(string message, params object[] args) => _logger.LogDebug(message, args);

    public void LogError(string message, params object[] args) => _logger.LogError(message, args);

    public void LogError(Exception exception, string message, params object[] args) => _logger.LogError(exception, message, args);

    public void LogTrace(string message, params object[] args) => _logger.LogTrace(message, args);

}