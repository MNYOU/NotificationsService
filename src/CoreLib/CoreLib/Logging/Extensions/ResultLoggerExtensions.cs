using CoreLib.Common;
using Microsoft.Extensions.Logging;

namespace CoreLib.Logging.Extensions;

public static class ResultLoggerExtensions
{
    public static void LogResult(this ILogger logger, OperationResult result)
    {
        if (result.IsSuccess)
        {
            logger.LogInformation("Operation succeeded.");
        }
        else
        {
            logger.LogError("Operation failed: {Error}", result.Error?.ToString());
        }
    }

    public static void LogResult<TResult>(this ILogger logger, OperationResult<TResult> result)
    {
        if (result.IsSuccess)
        {
            logger.LogInformation("Operation succeeded with result: {Result}", result.Result);
        }
        else
        {
            logger.LogError("Operation failed: {Error}", result.Error?.ToString());
        }
    }

    public static void LogResult<T>(this ILogger<T> logger, OperationResult result)
    {
        LogResult((ILogger)logger, result);
    }

    public static void LogResult<T, TResult>(this ILogger<T> logger, OperationResult<TResult> result)
    {
        LogResult((ILogger)logger, result);
    }
}