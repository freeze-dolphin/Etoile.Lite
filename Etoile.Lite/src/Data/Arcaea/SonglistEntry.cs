using System.Text.Json.Serialization;

namespace Etoile.Lite.Data;

public record SonglistEntry(
    [property: JsonPropertyName("id")]              string                         Id,
    [property: JsonPropertyName("deleted")]         bool?                          Deleted,
    [property: JsonPropertyName("title_localized")] Dictionary<string, string>     Title,
    [property: JsonPropertyName("artist")]          string                         Artist,
    [property: JsonPropertyName("search_title")]    Dictionary<string, string[]>?  SearchTitle,
    [property: JsonPropertyName("search_artist")]   Dictionary<string, string[]>?  SearchArtist,
    [property: JsonPropertyName("bpm")]             string                         BpmText,
    [property: JsonPropertyName("bpm_base")]        double                         BpmBase,
    [property: JsonPropertyName("set")]             string                         PackName,
    [property: JsonPropertyName("audioPreview")]    long                           AudioPreview,
    [property: JsonPropertyName("audioPreviewEnd")] long                           AudioPreviewEnd,
    [property: JsonPropertyName("side")]            int                            Side,
    [property: JsonPropertyName("bg")]              string                         Background,
    [property: JsonPropertyName("bg_inverse")]      string?                        BackgroundInverse,
    [property: JsonPropertyName("difficulties")]    IReadOnlyList<DifficultyEntry> Difficulties
);