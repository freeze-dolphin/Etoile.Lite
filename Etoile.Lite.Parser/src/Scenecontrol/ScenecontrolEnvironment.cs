using Etoile.Lite.Parser.Scenecontrol.Handler;
using Etoile.Lite.Parser.Types;
using Moonad;

namespace Etoile.Lite.Parser.Scenecontrol;

public class ScenecontrolEnvironment(ScenecontrolService scenecontrolService)
{
    private readonly Dictionary<string, IScenecontrolHandler> scenecontrolTypes = new();

    public Result<(Exception, RawScenecontrol?)> Rebuild(IEnumerable<RawScenecontrol> events)
    {
        Clean();
        AddBuiltInTypes();
        return ExecuteEvents(events);
    }

    private Result<(Exception, RawScenecontrol?)> ExecuteEvents(IEnumerable<RawScenecontrol> events)
    {
        RawScenecontrol? lastEv = null;
        try
        {
            foreach (var ev in events)
            {
                if (!scenecontrolTypes.ContainsKey(ev.ScenecontrolTypeName))
                {
                    continue;
                }

                lastEv = ev;
                scenecontrolTypes[ev.ScenecontrolTypeName].ExecuteCommand(ev);
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
        scenecontrolTypes.Clear();
        scenecontrolService.Clean();
    }

    private void AddBuiltInTypes()
    {
        AddType(new TrackDisplayType(scenecontrolService));
        AddType(new HideGroupType(scenecontrolService));
        AddType(new GroupAlphaType(scenecontrolService));
        AddType(new EnwidenLanesType(scenecontrolService));
        AddType(new EnwidenCameraType(scenecontrolService));
    }

    private void AddType(IScenecontrolHandler type)
    {
        scenecontrolTypes.Add(type.Typename, type);
    }
}