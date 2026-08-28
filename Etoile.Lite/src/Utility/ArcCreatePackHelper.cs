using Etoile.Lite.Data;

namespace Etoile.Lite.Utility;

public static class ArcCreatePackHelper
{
    public static string GetDifficultyString(int ratingClass, int rating, bool? ratingPlus, int? ratingClassAlias)
    {
        if (rating <= 0)
        {
            return $"{GetDifficultyPrefix()} ?";
        }

        var ratingPlusLabel = ratingPlus is true ? "+" : "";
        return $"{GetDifficultyPrefix()} {rating}{ratingPlusLabel}";

        string GetDifficultyPrefix()
        {
            var pfx = ratingClass switch
            {
                0 => "Past",
                1 => "Present",
                2 => "Future",
                3 => "Beyond",
                4 => "Eternal",
                _ => "Future"
            };

            if (ratingClassAlias == 1 && ratingClass == 3)
            {
                pfx = "Inscribed";
            }

            return pfx;
        }
    }

    /// <summary>
    /// Data grabbed from https://wiki.arcaea.cn CSS
    /// </summary>
    public static string GetDifficultyColor(int ratingClass, int? ratingClassAlias)
    {
        var col = ratingClass switch
        {
            0 => "#0a82beff", // "#3A6B78FF",
            1 => "#648c3cff", // "#566947FF",
            // 2 => default 
            3 => "#822328ff", // "#7C1C30FF",
            4 => "#5d4e76ff", // "#433455FF",
            _ => "#50194bff"  // "#482B54FF",
        };

        if (ratingClassAlias == 1 && ratingClass == 3)
        {
            col = "#243078ff";
        }

        return col;
    }

    public static string? GetLastOpenedChartPath(List<DifficultyEntry> difficultyEntries)
    {
        if (difficultyEntries.Count == 0) return null;
        if (difficultyEntries.Any(x => x.RatingClass == 2))
        {
            return "2.aff";
        }

        return $"{difficultyEntries.Max(x => x.RatingClass)}.aff";
    }

    public static SkinSettings GetSkinSettings(int side, string id, string set, string? background)
    {
        if (!Enum.IsDefined(typeof(SkinSettings.SideStyle), side)) throw new ArgumentOutOfRangeException(nameof(side), side, $"Unknown side: {side}.");

        var sideStyle = (SkinSettings.SideStyle)side;
        var qualifiedBackground = background ?? sideStyle.ToDefaultBackground();
        var noteStyle = sideStyle.ToNoteStyle();
        var particleStyle = set switch
        {
            "mirai" => sideStyle switch
            {
                SkinSettings.SideStyle.Conflict or SkinSettings.SideStyle.DarkLephon => SkinSettings.ParticleStyle.MiraiConflict,
                _                                                                    => SkinSettings.ParticleStyle.MiraiLight
            },
            "nijuusei" => SkinSettings.ParticleStyle.MiraiLight,
            _          => sideStyle.ToParticleStyle()
        };

        var trackStyle = qualifiedBackground switch
        {
            _ when qualifiedBackground.StartsWith("byd_")                                                => sideStyle.ToTrackStyle(),
            _ when id.StartsWith("alexandrite")                                                          => SkinSettings.TrackStyle.Black,
            "dynamix_conflict" or "mirai_conflict" or "lethaeus" or "mirai_awakened" or "saikyostronger" => SkinSettings.TrackStyle.Black,
            _ when !qualifiedBackground.StartsWith("nijuusei") || qualifiedBackground != "vs_conflict"
                => id.StartsWith("etherstrike")
                    ? SkinSettings.TrackStyle.Rei
                    : id.StartsWith("tempestissimo")
                        ? SkinSettings.TrackStyle.Tempestissimo
                        : qualifiedBackground switch
                        {
                            "finale_conflict" or "alterego"
                                => SkinSettings.TrackStyle.Finale,

                            "pentiment" or "apophenia"
                                => SkinSettings.TrackStyle.Pentiment,

                            "arcanaeden"
                                => SkinSettings.TrackStyle.Arcana,

                            _ => sideStyle.ToTrackStyle()
                        },

            _ => sideStyle.ToTrackStyle()
        };

        var accentStyle = set == "dynamix" || id == "alexandrite"
            ? SkinSettings.AccentStyle.Dynamix
            : sideStyle.ToAccentStyle();

        var singleLineStyle = set == "single"
            ? id == "neowings"
                ? SkinSettings.SingleLineStyle.Neo
                : sideStyle.ToSingleLineStyle()
            : SkinSettings.SingleLineStyle.None;

        return new SkinSettings
        {
            Side = sideStyle,
            Note = noteStyle,
            Particle = particleStyle,
            Track = trackStyle,
            Accent = accentStyle,
            SingleLine = singleLineStyle
        };
    }
}