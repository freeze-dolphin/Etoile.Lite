namespace Etoile.Lite.Data;

public record ChartSettings
{
    public required string  ChartPath         { get; init; }
    public required string  AudioPath         { get; init; }
    public required string  JacketPath        { get; init; }
    public required double  BaseBpm           { get; init; }
    public required string  BpmText           { get; init; }
    public          bool    SyncBaseBpm       { get; init; }
    public required string? BackgroundPath    { get; init; }
    public required string  Title             { get; init; }
    public required string  Composer          { get; init; }
    public required string  Charter           { get; init; }
    public required string  Alias             { get; init; }
    public required string  Illustrator       { get; init; }
    public required string  Difficulty        { get; init; }
    public required double  ChartConstant     { get; init; }
    public required string  DifficultyColor   { get; init; }
    public required object  Skin              { get; init; }
    public          object? Colors            { get; init; } = null;
    public          int?    LastWorkingTiming { get; init; } = null;
    public          int     PreviewStart      { get; init; } = 0;
    public          int     PreviewEnd        { get; init; } = 5000;
    public required string  SearchTags        { get; init; }
}