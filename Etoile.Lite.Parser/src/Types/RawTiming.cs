namespace Etoile.Lite.Parser.Types;

public record RawTiming : RawEvent
{
    public required double Bpm     { get; init; }
    public required double Divisor { get; init; }
}