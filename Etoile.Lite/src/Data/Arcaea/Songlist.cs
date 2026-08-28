using System.Text.Json.Serialization;

namespace Etoile.Lite.Data;

public record Songlist(
    [property: JsonPropertyName("songs")] IReadOnlyList<SonglistEntry> Songs
);