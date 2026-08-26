namespace Etoile.Lite.Parser.Types;

public record RawArc : RawEvent
{
    public required int                      EndTiming               { get; init; }
    public required double                   XStart                  { get; init; }
    public required double                   XEnd                    { get; init; }
    public required string                   LineType                { get; init; }
    public required double                   YStart                  { get; init; }
    public required double                   YEnd                    { get; init; }
    public required int                      Color                   { get; init; }
    public required bool                     IsTrace                 { get; init; }
    public          string                   Sfx                     { get; init; } = "none";
    public          double                   ArcResolutionMultiplier { get; init; } = 1.0;
    public          IReadOnlyList<RawArcTap> ArcTaps                 { get; init; } = [];
}