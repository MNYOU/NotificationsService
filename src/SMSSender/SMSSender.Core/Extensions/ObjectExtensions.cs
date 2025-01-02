namespace SMSSender.Core.Extensions;

public static class ObjectExtensions
{
    public static void EnsureNotNull<T>(this T? value) where T : class
    {
        ArgumentNullException.ThrowIfNull(value);
    }
}