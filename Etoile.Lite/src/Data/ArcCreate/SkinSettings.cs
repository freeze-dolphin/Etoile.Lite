namespace Etoile.Lite.Data;

public record SkinSettings
{
    public required SideStyle       Side       { get; init; }
    public required NoteStyle       Note       { get; init; }
    public required ParticleStyle   Particle   { get; init; }
    public required TrackStyle      Track      { get; init; }
    public required AccentStyle     Accent     { get; init; }
    public required SingleLineStyle SingleLine { get; init; }

    public object FlattenForSerialization()
    {
        var side = Side switch
        {
            SideStyle.Light      => "light",
            SideStyle.Conflict   => "conflict",
            SideStyle.Colorless  => "colorless",
            SideStyle.Lephon     => "lephon",
            SideStyle.DarkLephon => "conflict",
            _                    => throw new ArgumentOutOfRangeException($"Unknown side: {Side}.")
        };

        var note = Note switch
        {
            NoteStyle.Light    => "light",
            NoteStyle.Conflict => "conflict",
            _                  => throw new ArgumentOutOfRangeException($"Unknown note style: {Note}.")
        };

        var particle = Particle switch
        {
            ParticleStyle.Light         => "light",
            ParticleStyle.Conflict      => "conflict",
            ParticleStyle.Colorless     => "colorless",
            ParticleStyle.MiraiConflict => "miraiconflict",
            ParticleStyle.MiraiLight    => "mirailight",
            _                           => throw new ArgumentOutOfRangeException($"Unknown particle style: {Particle}.")
        };

        var track = Track switch
        {
            TrackStyle.Light         => "light",
            TrackStyle.Conflict      => "conflict",
            TrackStyle.Black         => "black",
            TrackStyle.Nijuusei      => "nijuusei",
            TrackStyle.Rei           => "rei",
            TrackStyle.ConflictVs    => "conflictvs",
            TrackStyle.Tempestissimo => "tempestissimo",
            TrackStyle.Finale        => "finale",
            TrackStyle.Pentiment     => "pentiment",
            TrackStyle.Arcana        => "arcana",
            TrackStyle.Colorless     => "colorless",
            _                        => throw new ArgumentOutOfRangeException($"Unknown track style: {Track}.")
        };

        var accent = Accent switch
        {
            AccentStyle.Light     => "light",
            AccentStyle.Conflict  => "conflict",
            AccentStyle.Dynamix   => "dynamix",
            AccentStyle.Colorless => "colorless",
            _                     => throw new ArgumentOutOfRangeException($"Unknown accent style: {Accent}.")
        };

        var singleLine = SingleLine switch
        {
            SingleLineStyle.None     => "none",
            SingleLineStyle.Light    => "light",
            SingleLineStyle.Conflict => "conflict",
            SingleLineStyle.Neo      => "neo",
            _                        => throw new ArgumentOutOfRangeException($"Unknown single line style: {SingleLine}.")
        };

        return new
        {
            side,
            note,
            particle,
            track,
            accent,
            singleLine
        };
    }

    public enum SideStyle
    {
        Light,
        Conflict,
        Colorless,
        Lephon,
        DarkLephon
    }

    public enum NoteStyle
    {
        Light,
        Conflict,
    }

    public enum ParticleStyle
    {
        Light,
        Conflict,
        Colorless,
        MiraiConflict,
        MiraiLight,
    }

    public enum TrackStyle
    {
        Light,
        Conflict,
        Black,
        Nijuusei,
        Rei,
        ConflictVs,
        Tempestissimo,
        Finale,
        Pentiment,
        Arcana,
        Colorless
    }

    public enum AccentStyle
    {
        Light,
        Conflict,
        Dynamix,
        Colorless
    }

    public enum SingleLineStyle
    {
        None,
        Light,
        Conflict,
        Neo
    }
}

public static class SkinSettingsExtensions
{
    extension(SkinSettings.SideStyle side)
    {
        public SkinSettings.TrackStyle ToTrackStyle() =>
            side switch
            {
                SkinSettings.SideStyle.Light => SkinSettings.TrackStyle.Light,
                SkinSettings.SideStyle.Conflict => SkinSettings.TrackStyle.Conflict,
                SkinSettings.SideStyle.Colorless => SkinSettings.TrackStyle.Colorless,
                SkinSettings.SideStyle.Lephon => SkinSettings.TrackStyle.Light,
                SkinSettings.SideStyle.DarkLephon => SkinSettings.TrackStyle.Tempestissimo,
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, $"Unable to convert side `{side}` to the corresponding track style.")
            };

        public SkinSettings.ParticleStyle ToParticleStyle() =>
            side switch
            {
                SkinSettings.SideStyle.Light => SkinSettings.ParticleStyle.Light,
                SkinSettings.SideStyle.Conflict => SkinSettings.ParticleStyle.Conflict,
                SkinSettings.SideStyle.Colorless => SkinSettings.ParticleStyle.Colorless,
                SkinSettings.SideStyle.Lephon => SkinSettings.ParticleStyle.Light,
                SkinSettings.SideStyle.DarkLephon => SkinSettings.ParticleStyle.Conflict,
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, $"Unable to convert side `{side}` to the corresponding particle style.")
            };

        public SkinSettings.NoteStyle ToNoteStyle() =>
            side switch
            {
                SkinSettings.SideStyle.Light => SkinSettings.NoteStyle.Light,
                SkinSettings.SideStyle.Conflict => SkinSettings.NoteStyle.Conflict,
                SkinSettings.SideStyle.Colorless => SkinSettings.NoteStyle.Light,
                SkinSettings.SideStyle.Lephon => SkinSettings.NoteStyle.Light,
                SkinSettings.SideStyle.DarkLephon => SkinSettings.NoteStyle.Conflict,
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, $"Unable to convert side `{side}` to the corresponding note style.")
            };

        public SkinSettings.AccentStyle ToAccentStyle() =>
            side switch
            {
                SkinSettings.SideStyle.Light => SkinSettings.AccentStyle.Light,
                SkinSettings.SideStyle.Conflict => SkinSettings.AccentStyle.Conflict,
                SkinSettings.SideStyle.Colorless => SkinSettings.AccentStyle.Colorless,
                SkinSettings.SideStyle.Lephon => SkinSettings.AccentStyle.Light,
                SkinSettings.SideStyle.DarkLephon => SkinSettings.AccentStyle.Conflict,
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, $"Unable to convert side `{side}` to the corresponding accent style.")
            };

        public SkinSettings.SingleLineStyle ToSingleLineStyle() =>
            side switch
            {
                SkinSettings.SideStyle.Light => SkinSettings.SingleLineStyle.Light,
                SkinSettings.SideStyle.Conflict => SkinSettings.SingleLineStyle.Conflict,
                SkinSettings.SideStyle.Colorless => SkinSettings.SingleLineStyle.Light,
                SkinSettings.SideStyle.Lephon => SkinSettings.SingleLineStyle.Light,
                SkinSettings.SideStyle.DarkLephon => SkinSettings.SingleLineStyle.Conflict,
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, $"Unable to convert side `{side}` to the corresponding single line style.")
            };

        public string ToDefaultBackground() => side switch
        {
            SkinSettings.SideStyle.Light => "base_light",
            SkinSettings.SideStyle.Conflict => "base_conflict",
            SkinSettings.SideStyle.Colorless => "epilogue",
            SkinSettings.SideStyle.Lephon => "lephon",
            SkinSettings.SideStyle.DarkLephon => "konzetsu-a",
            _ => throw new ArgumentOutOfRangeException(nameof(side), side, $"Unable to convert side `{side}` to the corresponding background.")
        };
    }
}