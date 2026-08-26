namespace Etoile.Lite.Parser.Types;

public record RawArcTap : RawEvent
{
    public double Width { get; init; } = 1;
}