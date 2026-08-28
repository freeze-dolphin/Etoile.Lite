namespace Etoile.Lite.Data;

public record ProjectSettings
{
    public required string?               LastOpenedChartPath { get; init; }
    public required List<ChartSettings>   Charts              { get; init; }
    public required EditorProjectSettings EditorSettings      { get; init; }
}