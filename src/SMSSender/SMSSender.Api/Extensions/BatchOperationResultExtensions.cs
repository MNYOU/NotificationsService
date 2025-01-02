using CoreLib.Common;
using Microsoft.AspNetCore.Mvc;

namespace SMSSender.Api.Extensions;

public static class BatchOperationResultExtensions
{
    public static ActionResult<TResult> ToActionResult<TResult>(this BatchOperationResult result,
        Func<TResult> provider, StatusCode statusCode = StatusCode.Ok) =>
        result.IsSuccess
            ? new ObjectResult(provider()) { StatusCode = (int) statusCode }
            : new ObjectResult(result.Errors) { StatusCode = (int) GetAppropriateStatusCode(result.Errors.ToList()) };

    public static ActionResult<TResult> ToActionResult<TResult>(this BatchOperationResult result, TResult data,
        StatusCode statusCode = StatusCode.Ok) =>
        result.IsSuccess
            ? new ObjectResult(data) { StatusCode = (int) statusCode }
            : new ObjectResult(result.Errors) { StatusCode = (int) GetAppropriateStatusCode(result.Errors.ToList()) };

    public static ActionResult
        ToActionResult(this BatchOperationResult result, StatusCode successCode = StatusCode.Ok) =>
        result.IsSuccess
            ? new StatusCodeResult((int) successCode)
            : new ObjectResult(result.Errors) { StatusCode = (int) GetAppropriateStatusCode(result.Errors.ToList()) };

    private static StatusCode GetAppropriateStatusCode(ICollection<Error> errors)
    {
        return errors.Count switch
        {
            0 => StatusCode.Accepted,
            1 => errors.First().StatusCode,
            _ => StatusCode.InternalServiceError
        };
    }
}