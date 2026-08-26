using Etoile.Lite.Parser.Scenecontrol.Handler;
using Etoile.Lite.Parser.Types;
using Moonad;

namespace Etoile.Lite.Parser.Scenecontrol;

public class ScenecontrolEnvironment(ScenecontrolService scenecontrolService)
{
    private readonly Dictionary<string, IScenecontrolHandler> scenecontrolTypes = new();

    public Result<(Exception, string)> Rebuild(IEnumerable<RawScenecontrol> events)
    {
        Clean();
        AddBuiltInTypes();
        return ExecuteEvents(events);
    }

    private Result<(Exception, string)> ExecuteEvents(IEnumerable<RawScenecontrol> events)
    {
        string lastTypename = "";
        try
        {
            foreach (var ev in events)
            {
                if (!scenecontrolTypes.ContainsKey(ev.ScenecontrolTypeName))
                {
                    continue;
                }

                lastTypename = ev.ScenecontrolTypeName;
                scenecontrolTypes[ev.ScenecontrolTypeName].ExecuteCommand(ev);
            }

            return Result<(Exception, string)>.Ok();
        }
        catch (Exception e)
        {
            Clean();
            return (e, lastTypename);
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