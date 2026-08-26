using Etoile.Lite.Parser.Scenecontrol.IO;

namespace Etoile.Lite.Parser.Scenecontrol.Channels.MathChannels;

public class ModuloChannel(ValueChannel a, ValueChannel b) : ValueChannel
{
    public override List<object>? SerializeProperties(ScenecontrolSerialization serialization)
    {
        return
        [
            serialization.AddUnitAndGetId(a),
            serialization.AddUnitAndGetId(b)
        ];
    }

    public override float ValueAt(int timing)
        => a.ValueAt(timing) % b.ValueAt(timing);

    protected override IEnumerable<ValueChannel> GetChildrenChannels()
    {
        yield return a;
        yield return b;
    }
}