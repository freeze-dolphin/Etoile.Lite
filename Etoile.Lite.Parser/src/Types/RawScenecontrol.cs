namespace Etoile.Lite.Parser.Types;

public record RawScenecontrol : RawEvent
{
    public required string               ScenecontrolTypeName { get; init; }
    public          IReadOnlyList<float> Arguments            { get; init; } = [];
}