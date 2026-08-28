namespace Etoile.Lite.Data;

public record ColorSettings
{
    public string?       Trace  { get; init; } = null;
    public string?       Shadow { get; init; } = null;
    public List<string>? Arc    { get; init; } = null;
    public List<string>? ArcLow { get; init; } = null;
}