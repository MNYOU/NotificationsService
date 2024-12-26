using System.Collections.Immutable;
using System.Diagnostics.Contracts;

namespace EmailSender.Core.Extensions;

public static class CollectionExtensions
{
    public static IReadOnlySet<T> EmptyIfNull<T>(this IReadOnlySet<T>? comparer) => 
        comparer ?? ImmutableHashSet<T>.Empty;
    
    public static IReadOnlyList<T> EmptyIfNull<T>(this IReadOnlyList<T>? comparer) =>
        comparer ?? ImmutableList<T>.Empty;
    
    public static IReadOnlyCollection<T> EmptyIfNull<T>(this IReadOnlyCollection<T>? comparer) =>
        comparer ?? ImmutableArray<T>.Empty;
}
