using Etoile.Lite.Parser.Types;

namespace Etoile.Lite.Parser.Scenecontrol.Handler;

public interface IScenecontrolCommandHandler
{
    string Typename { get; }
    void ExecuteCommand(RawScenecontrol ev);
}