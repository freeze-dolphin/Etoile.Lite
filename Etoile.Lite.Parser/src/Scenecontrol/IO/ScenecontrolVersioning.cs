namespace Etoile.Lite.Parser.Scenecontrol.IO;

internal class ScenecontrolVersioning(EnabledFeatures features) : ISerializableUnit
{
    public List<object> SerializeProperties(ScenecontrolSerialization serialization)
    {
        return [(long)features];
    }
}