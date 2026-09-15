using Etoile.Lite.Parser.Types;

namespace Etoile.Lite.Parser.Scenecontrol.Handler;

public interface IScenecontrolPassiveHandler
{
    void ExecuteCommand(List<(RawTimingGroup, IEnumerable<RawEvent>)> groups);
}