namespace Etoile.Lite.Utility;

public static class CollectionExtensions
{
    public static IEnumerable<T> ConcatIfNotNull<T>(this IEnumerable<T> first, IEnumerable<T>? second) => second is null ? first : first.Concat(second);
}