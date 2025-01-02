using System.Collections.Frozen;
using EmailSender.Core.Common;

namespace CoreLib.Common;

public class BatchOperationResult<T> : BatchOperationResult
{
    public IReadOnlyCollection<T> SuccessfulEntities { get; }
    public IReadOnlyCollection<T> FailedEntities { get; }

    private BatchOperationResult(int successes, int failures, IEnumerable<Error> errorMessages,
        IEnumerable<T> successfulEntities, IEnumerable<T> failedEntities)
        : base(successes, failures, errorMessages)
    {
        SuccessfulEntities = successfulEntities.ToFrozenSet();
        FailedEntities = failedEntities.ToFrozenSet();
    }

    public static BatchOperationResult<T> FromOperationResults(ICollection<OperationResult<T>> results)
    {
        var successes = 0;
        var failures = 0;
        var errorMessages = new List<Error>(results.Count);
        var successfulEntities = new List<T>(results.Count);
        var failedEntities = new List<T>(results.Count);

        foreach (var result in results)
        {
            if (result.IsSuccess)
            {
                successes++;
                successfulEntities.Add(result.Result!);
            }
            else
            {
                failures++;
                if (result.Error != null)
                {
                    errorMessages.Add(result.Error);
                }

                failedEntities.Add(result.Result!);
            }
        }

        return new BatchOperationResult<T>(successes, failures, errorMessages, successfulEntities, failedEntities);
    }

    public override string ToString()
    {
        return
            $"Batch Operation Result: Successes={Successes}, Failures={Failures}, Errors={string.Join(", ", Errors)}, " +
            $"Successful Entities Count = {SuccessfulEntities.Count}, Failed Entities Count = {FailedEntities.Count}";
    }
}