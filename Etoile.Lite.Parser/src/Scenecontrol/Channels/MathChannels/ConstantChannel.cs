using Etoile.Lite.Parser.Scenecontrol.IO;

namespace Etoile.Lite.Parser.Scenecontrol.Channels.MathChannels;

public class ConstantChannel(float val) : ValueChannel
{
    public override List<object>? SerializeProperties(ScenecontrolSerialization serialization)
    {
        return [val];
    }

    public override float ValueAt(int timing) => val;

    protected override IEnumerable<ValueChannel> GetChildrenChannels()
    {
        yield break;
    }
}