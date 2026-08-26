using System.Globalization;
using Antlr4.Runtime.Tree;

namespace Etoile.Lite.Parser.Utility;

public static class AntlrExtensions
{
    extension(ITerminalNode node)
    {
        public int ParseInt()
            => int.Parse(node.GetText());

        public double ParseFloat()
            => double.Parse(node.GetText(), CultureInfo.InvariantCulture);
    }
}