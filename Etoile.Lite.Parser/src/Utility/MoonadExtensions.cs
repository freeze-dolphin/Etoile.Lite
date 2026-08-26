using Moonad;

namespace Etoile.Lite.Parser.Utility;

public static class MoonadExtensions
{
    public static bool TryUnwrap<T, TE>(
        this Result<T, TE> result,
        out  T             value,
        out  TE            error
    )
        where T : notnull
        where TE : notnull
    {
        value = result.ResultValue;
        error = result.ErrorValue;

        return !result.IsError;
    }
}