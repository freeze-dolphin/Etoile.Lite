using Etoile.Lite.Parser.Scenecontrol.Channels;
using Etoile.Lite.Parser.Scenecontrol.Channels.ContextChannels;
using Etoile.Lite.Parser.Scenecontrol.Channels.MathChannels;
using Etoile.Lite.Parser.Scenecontrol.IO;

namespace Etoile.Lite.Parser.Scenecontrol;

public class Context(ScenecontrolService scenecontrolService) : ISceneController
{
    public string SerializedType { get; init; } = "context";

    public void CleanController()
    {
        LaneFrom = new ConstantChannel(1);
        LaneTo = new ConstantChannel(4);
    }

    public static ScreenIs16By9Channel Is16By9 => new();

    public ValueChannel LaneFrom
    {
        get;
        set
        {
            field = value;
            scenecontrolService.AddReferencedController(this);
        }
    } = new ConstantChannel(1);

    public ValueChannel LaneTo
    {
        get;
        set
        {
            field = value;
            scenecontrolService.AddReferencedController(this);
        }
    } = new ConstantChannel(4);


    public List<object>? SerializeProperties(ScenecontrolSerialization serialization)
    {
        return
        [
            serialization.AddUnitAndGetId(LaneFrom),
            serialization.AddUnitAndGetId(LaneTo)
        ];
    }
}