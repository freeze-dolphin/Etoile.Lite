using System.Text;

namespace Etoile.Lite.Parser.Utility;

public static class StringBuilderExtensions
{
    public static void AppendLfLine(this StringBuilder builder, string line)
    {
        builder.Append($"{line}\n");
    }
}