namespace Etoile.Lite.Parser.Scenecontrol.Channels.StringChannels;

public class StringChannelBuilder
{
    public static KeyStringChannel Create()
    {
        return new KeyStringChannel();
    }

    public static StringChannel Constant(string value)
    {
        KeyStringChannel channel = new KeyStringChannel();
        channel.AddKey(int.MinValue, value);
        return channel;
    }
}