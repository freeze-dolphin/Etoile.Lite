namespace Etoile.Lite.Parser.Scenecontrol.IO;

public interface ISerializableUnit
{
    List<object>? SerializeProperties(ScenecontrolSerialization serialization);
}