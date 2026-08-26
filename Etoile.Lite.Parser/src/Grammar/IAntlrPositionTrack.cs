namespace Etoile.Lite.Parser.Grammar;

public interface IAntlrPositionTrack
{
    int LineNumber   { get; }
    int ColumnNumber { get; }
}