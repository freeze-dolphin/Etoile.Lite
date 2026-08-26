using Etoile.Lite.Parser.Scenecontrol.IO;

namespace Etoile.Lite.Parser.Scenecontrol.Channels.StringChannels;

public abstract class StringChannel : ISerializableUnit
{
    public abstract string ValueAt(int timing);

    public abstract List<object>? SerializeProperties(ScenecontrolSerialization serialization);
}