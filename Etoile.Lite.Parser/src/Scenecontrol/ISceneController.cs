using Etoile.Lite.Parser.Scenecontrol.IO;

namespace Etoile.Lite.Parser.Scenecontrol;

public interface ISceneController : ISerializableUnit
{
    string SerializedType { get; init; }

    void CleanController();
}