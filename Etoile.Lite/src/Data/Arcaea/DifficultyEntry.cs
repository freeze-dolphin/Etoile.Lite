using System.Text.Json.Serialization;

namespace Etoile.Lite.Data;

public record DifficultyEntry(
    [property: JsonPropertyName("ratingClass")]      int                         RatingClass,
    [property: JsonPropertyName("ratingClassAlias")] int?                        RatingClassAlias,
    [property: JsonPropertyName("title_localized")]  Dictionary<string, string>? Title,
    [property: JsonPropertyName("artist")]           string?                     Artist,
    [property: JsonPropertyName("bpm")]              string?                     BpmText,
    [property: JsonPropertyName("bpm_base")]         double?                     BpmBase,
    [property: JsonPropertyName("audioOverride")]    bool?                       AudioOverride,
    [property: JsonPropertyName("chartDesigner")]    string                      ChartDesigner,
    [property: JsonPropertyName("jacketDesigner")]   string                      JacketDesigner,
    [property: JsonPropertyName("jacketOverride")]   bool?                       JacketOverride,
    [property: JsonPropertyName("bg")]               string?                     Background,
    [property: JsonPropertyName("bg_inverse")]       string?                     BackgroundInverse,
    [property: JsonPropertyName("rating")]           int                         Rating,
    [property: JsonPropertyName("ratingPlus")]       bool?                       RatingPlus
);