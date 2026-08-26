using Etoile.Lite.Parser.Scenecontrol.IO;

namespace Etoile.Lite.Parser.Scenecontrol.Channels.ContextChannels;

public class ScreenIs16By9Channel : ValueChannel
{
    public override List<object>? SerializeProperties(ScenecontrolSerialization serialization)
    {
        return null;
    }

    public override float ValueAt(int timing)
    {
        throw new NotImplementedException();
    }

    protected override IEnumerable<ValueChannel> GetChildrenChannels()
    {
        yield break;
    }
}