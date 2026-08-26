namespace Etoile.Lite.Parser.Types;

public record RawHold : RawEvent
{
    public required int    EndTiming { get; init; }
    public required double Lane      { get; init; }
}