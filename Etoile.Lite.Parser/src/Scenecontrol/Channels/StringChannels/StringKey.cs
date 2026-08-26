namespace Etoile.Lite.Parser.Scenecontrol.Channels.StringChannels;

public class StringKey
{
    public int Timing { get; set; }

    public string Value { get; set; }

    public int OverrideIndex { get; set; } = 0;

    public object Serialize()
    {
        return $"{Timing},{Value}";
    }
}