
namespace Etoile.Lite.Parser.Types;

public class RawTimingGroup
{
    public bool   NoInput     { get; set; } = false;
    public bool   FadingHolds { get; set; } = false;
    public double AngleX      { get; set; } = 0;
    public double AngleY      { get; set; } = 0;

    public override string ToString()
    {
        var opts = GetPropertyStrings(true);
        return string.Join(",", opts);
    }

    private List<string> GetPropertyStrings(bool withName)
    {
        List<string> opts = [];

        if (NoInput)
        {
            opts.Add("noinput");
        }

        if (FadingHolds)
        {
            opts.Add("fadingholds");
        }

        if (AngleX != 0)
        {
            opts.Add($"anglex={AngleX:f2}");
        }

        if (AngleY != 0)
        {
            opts.Add($"angley={AngleY:f2}");
        }

        return opts;
    }
}