using CoreLib.Common;
using Microsoft.Extensions.Logging;

namespace CoreLib.Logging.Extensions;

public static class BatchResultLoggerExtensions
{
    public static void LogBatchResult(this ILogger logger, BatchOperationResult result)
    {
        if (result.IsSuccess)
        {
            logger.LogInformation("Batch operation: {message}", result.ToString());
        }
        else
        {
            logger.LogError("Batch operation: {message}", result.ToString());
        }
    }

    public static void LogBatchResult<TResult>(this ILogger logger, BatchOperationResult<TResult> result)
    {
        if (result.IsSuccess)
        {
            logger.LogInformation("Batch operation: {message}", result.ToString());
        }
        else
        {
            logger.LogError("Batch operation: {message}", result.ToString());
        }
    }

    public static void LogBatchResult<T>(this ILogger<T> logger, BatchOperationResult result)
    {
        LogBatchResult((ILogger)logger, result);
    }

    public static void LogBatchResult<T, TResult>(this ILogger<T> logger, BatchOperationResult<TResult> result)
    {
        LogBatchResult((ILogger)logger, result);
    }
}