using CoreLib.Common;
using Microsoft.AspNetCore.Mvc;

namespace WhatsappSender.Api.Extensions;

public static class OperationResultExtensions
{
    public static ActionResult<TResult> ToActionResult<TResult>(this OperationResult<TResult> result, StatusCode statusCode = StatusCode.Ok) =>
        result.ToActionResult(result.IsSuccess ? result.Result : default!, statusCode);

    public static ActionResult<TResult> ToActionResult<TData, TResult>(this OperationResult<TData> result, Func<TData, TResult> mapper, StatusCode statusCode = StatusCode.Ok) =>
        result.IsSuccess
            ? new ObjectResult(mapper(result.Result)) {StatusCode = (int)statusCode}
            : new ObjectResult(result.Error) {StatusCode = (int)result.Error!.StatusCode};

    public static ActionResult<TResult> ToActionResult<TResult>(this OperationResult result, Func<TResult> provider, StatusCode statusCode = StatusCode.Ok) =>
        result.IsSuccess
            ? new ObjectResult(provider()) {StatusCode = (int)statusCode}
            : new ObjectResult(result.Error) {StatusCode = (int)result.Error!.StatusCode};

    public static ActionResult<TResult> ToActionResult<TResult>(this OperationResult result, TResult data, StatusCode statusCode = StatusCode.Ok) =>
        result.IsSuccess
            ? new ObjectResult(data) {StatusCode = (int)statusCode}
            : new ObjectResult(result.Error) {StatusCode = (int)result.Error!.StatusCode};

    public static ActionResult ToActionResult(this OperationResult result, StatusCode successCode = StatusCode.Ok) =>
        result.IsSuccess
            ? new StatusCodeResult((int)successCode)
            : new ObjectResult(result.Error) {StatusCode = (int)result.Error!.StatusCode};
}