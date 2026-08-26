using Etoile.Lite.Parser.Types;

namespace Etoile.Lite.Parser.Scenecontrol.Handler;

public interface IScenecontrolHandler
{
    string Typename { get; }
    void ExecuteCommand(RawScenecontrol ev);
}