using System.Numerics;

namespace Etoile.Lite.Parser.Types;

public record RawCamera : RawEvent
{
    public required Vector3 Move       { get; init; }
    public required Vector3 Rotate     { get; init; }
    public required string  CameraType { get; init; }
    public required int     Duration   { get; init; }
}