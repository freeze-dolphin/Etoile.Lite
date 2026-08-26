namespace Etoile.Lite.Parser.Types;

public record RawTap : RawEvent
{
    public required double Lane { get; init; }
}