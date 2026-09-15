using Etoile.Lite.Parser.Scenecontrol.Handler;
using Etoile.Lite.Parser.Types;
using Moonad;

namespace Etoile.Lite.Parser.Scenecontrol;

public class ScenecontrolEnvironment(ScenecontrolService scenecontrolService)
{
    private readonly Dictionary<string, IScenecontrolCommandHandler> scenecontrolCommandTypes = new();

    public Result<(Exception, RawScenecontrol?)> Rebuild(List<(RawTimingGroup, IEnumerable<RawEvent>)> groups)
    {
        var sc = groups.SelectMany(x => x.Item2).OfType<RawScenecontrol>();

        Clean();
        AddBuiltInCommandTypes();
        AddBuiltInPassiveTypes(groups);
        return ExecuteEvents(sc);
    }

    private Result<(Exception, RawScenecontrol?)> ExecuteEvents(IEnumerable<RawScenecontrol> events)
    {
        RawScenecontrol? lastEv = null;
        try
        {
            foreach (var ev in events)
            {
                if (!scenecontrolCommandTypes.ContainsKey(ev.ScenecontrolTypeName))
                {
                    continue;
                }

                lastEv = ev;
                scenecontrolCommandTypes[ev.ScenecontrolTypeName].ExecuteCommand(ev);
            }

            return Result<(Exception, RawScenecontrol?)>.Ok();
        }
        catch (Exception e)
        {
            Clean();
            return (e, lastEv);
        }
    }

    private void Clean()
    {
        scenecontrolCommandTypes.Clear();
        scenecontrolService.Clean();
    }

    private void AddBuiltInCommandTypes()
    {
        AddCommandType(new TrackDisplayType(scenecontrolService));
        AddCommandType(new HideGroupType(scenecontrolService));
        AddCommandType(new GroupAlphaType(scenecontrolService));
        AddCommandType(new EnwidenLanesType(scenecontrolService));
        AddCommandType(new EnwidenCameraType(scenecontrolService));
    }

    private void AddBuiltInPassiveTypes(List<(RawTimingGroup, IEnumerable<RawEvent>)> groups)
    {
    }

    private void AddCommandType(IScenecontrolCommandHandler type)
    {
        scenecontrolCommandTypes.Add(type.Typename, type);
    }

    private void AddPassiveType(IScenecontrolPassiveHandler type, List<(RawTimingGroup, IEnumerable<RawEvent>)> groups)
    {
        type.ExecuteCommand(groups);
    }
}