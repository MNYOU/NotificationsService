﻿using JetBrains.Annotations;

namespace CoreLib.Common;

[PublicAPI]
public class OperationResult<TResult> : OperationResult
{
    private OperationResult(TResult result)
    {
        Result = result;
    }

    private OperationResult(Error error) : base(error)
    {
        Result = default!;
    }

    /// <summary>
    /// Not null when IsSuccess == true
    /// </summary>
    public TResult Result { get; }

    public static OperationResult<TResult> Success(TResult result)
    {
        return new OperationResult<TResult>(result);
    }

    public new static OperationResult<TResult> Failure(Error error)
    {
        return new OperationResult<TResult>(error);
    }

    public static implicit operator OperationResult<TResult>(TResult obj)
    {
        return Success(obj);
    }

    public static implicit operator OperationResult<TResult>(Error error)
    {
        return Failure(error);
    }
}