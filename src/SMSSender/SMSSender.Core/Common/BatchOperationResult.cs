using System.Collections.Frozen;

namespace SMSSender.Core.Common;

public class BatchOperationResult(int successes, int failures, IEnumerable<Error> errorMessages)
{
    public int Successes { get; } = successes;
    public int Failures { get; } = failures;
    public IReadOnlyCollection<Error> Errors { get; } = errorMessages.ToFrozenSet();

    public bool IsSuccess => Failures == 0;
    public bool IsFailure => Failures > 0;
    

    public static BatchOperationResult FromOperationResults(ICollection<OperationResult> results)
    {
        var successes = 0;
        var failures = 0;
        var errorMessages = new List<Error>(results.Count);

        foreach (var result in results)
        {
            if (result.IsSuccess)
            {
                successes++;
            }
            else
            {
                failures++;
                if (result.Error != null)
                {
                    errorMessages.Add(result.Error);
                }
            }
        }

        return new BatchOperationResult(successes, failures, errorMessages);
    }

    public override string ToString()
    {
        return
            $"Batch Operation Result: Successes={Successes}, Failures={Failures}, Errors={string.Join(", ", Errors)}";
    }
}