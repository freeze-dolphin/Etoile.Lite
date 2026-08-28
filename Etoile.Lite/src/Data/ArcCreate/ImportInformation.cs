namespace Etoile.Lite.Data;

public record ImportInformation
{
    public const string FileName      = "index.yml";
    public const string LevelType     = "level";
    public const string PackType      = "pack";
    public const string CharacterType = "character";

    public required string Directory    { get; set; }
    public required string Identifier   { get; set; }
    public required string SettingsFile { get; set; }
    public          int    Version      { get; set; } = 1;
    public required string Type         { get; set; }
}