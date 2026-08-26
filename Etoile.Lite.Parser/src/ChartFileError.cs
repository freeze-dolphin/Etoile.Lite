using Antlr4.Runtime;
using Etoile.Lite.Parser.Grammar;

namespace Etoile.Lite.Parser;

public class ChartFileError(
    string message,
    int    lineNumber,
    int    columnNumber
) : IAntlrPositionTrack
{
    public string Message      { get; } = message;
    public int    LineNumber   { get; } = lineNumber;
    public int    ColumnNumber { get; } = columnNumber;

    public ChartFileError(string message, IToken start) :
        this(message,
             start.Line,
             start.Column)
    {
    }

    public ChartFileError(string message, IToken start, IToken parentStart) :
        this(message,
             parentStart.Line,
             start.Column + parentStart.Column)
    {
    }
}