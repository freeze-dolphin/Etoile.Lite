using Etoile.Lite.Parser.Scenecontrol.IO;

namespace Etoile.Lite.Parser.Scenecontrol.Channels.MathChannels;

public class NegateChannel(ValueChannel channel) : ValueChannel
{
    public override List<object>? SerializeProperties(ScenecontrolSerialization serialization)
    {
        return [serialization.AddUnitAndGetId(channel)];
    }

    public override float ValueAt(int timing)
    {
        return -channel.ValueAt(timing);
    }

    protected override IEnumerable<ValueChannel> GetChildrenChannels()
    {
        yield return channel;
    }
}