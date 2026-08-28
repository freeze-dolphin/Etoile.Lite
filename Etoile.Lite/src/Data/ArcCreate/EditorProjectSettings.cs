namespace Etoile.Lite.Data;

public record EditorProjectSettings
{
    public required string LastUsedPublisher   { get; init; }
    public required string LastUsedPackageName { get; init; }
}