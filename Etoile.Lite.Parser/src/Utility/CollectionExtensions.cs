namespace Etoile.Lite.Parser.Utility;

public static class CollectionExtensions
{
    /// <summary>
    /// Taken from ArcadeZero. Idk how it works anymore.
    /// </summary>
    /// <param name="list">The list to search from.</param>
    /// <param name="value">The value to search for.</param>
    /// <param name="property">Property extractor.</param>
    /// <typeparam name="T">The list's element's type.</typeparam>
    /// <typeparam name="R">Type of the property to search by.</typeparam>
    /// <returns>Index of the nearest element.</returns>
    public static int BinarySearchNearest<T, R>(this IList<T> list, R value, Func<T, R> property)
        where R : IComparable<R>
    {
        if (value.CompareTo(property(list[0])) <= 0)
        {
            return 0;
        }

        if (value.CompareTo(property(list[list.Count - 1])) >= 0)
        {
            return list.Count - 1;
        }

        int index = 0;

        int first = 0;
        int last = list.Count - 1;
        int mid = 0;
        R midValue = property(list[mid]);

        while (first < last - 1)
        {
            mid = (first + last) / 2;
            midValue = property(list[mid]);
            if (value.CompareTo(midValue) == 0)
            {
                index = mid;
                break;
            }
            else if (value.CompareTo(midValue) < 0)
            {
                last = mid;
            }
            else
            {
                first = mid;
            }
        }

        if (midValue.CompareTo(value) <= 0)
        {
            index = mid;
        }
        else
        {
            index = mid - 1;
        }

        index = Math.Clamp(index, 0, list.Count - 1);
        return index;
    }
}