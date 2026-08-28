namespace Etoile.Lite.Parser.Types;

public record RawEvent
{
    public int          Timing      { get; init; }
    public RawEventType Type        { get; init; }
    public int          TimingGroup { get; init; }

    public required int LineNumber { get; init; }
}