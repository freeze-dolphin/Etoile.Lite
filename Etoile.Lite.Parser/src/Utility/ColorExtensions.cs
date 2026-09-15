using System.Drawing;
using Moonad;

namespace Etoile.Lite.Parser.Utility;

public static class ColorExtensions
{
    public static Result<Color, Exception> ToColor(this string hex)
    {
        try
        {
            return ColorTranslator.FromHtml(hex);
        }
        catch (ArgumentException e)
        {
            return e;
        }
    }

    public static bool TryParseColor(this string hex, out Color? val)
    {
        var result = hex.ToColor();

        val = result.IsOk ? result.ResultValue : null;
        return result.IsOk;
    }
}