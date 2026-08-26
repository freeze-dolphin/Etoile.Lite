using Etoile.Lite.Parser.Scenecontrol.Channels.MathChannels;
using Etoile.Lite.Parser.Scenecontrol.IO;

namespace Etoile.Lite.Parser.Scenecontrol.Channels;

public abstract class ValueChannel : ISerializableUnit
{
    public static ValueChannel ConstantZeroChannel { get; } = new ConstantChannel(0);

    public static ValueChannel ConstantOneChannel { get; } = new ConstantChannel(1);

    public string Name { get; set; }

    public static ValueChannel operator +(ValueChannel a) => a;

    public static NegateChannel operator -(ValueChannel a) => new NegateChannel(a);

    public static SumChannel operator +(ValueChannel a, ValueChannel b) => new SumChannel(a, b);

    public static SumChannel operator +(ValueChannel a, float b) => a + new ConstantChannel(b);

    public static SumChannel operator +(float b, ValueChannel a) => a + new ConstantChannel(b);

    public static SumChannel operator -(ValueChannel a, ValueChannel b) => new SumChannel(a, -b);

    public static SumChannel operator -(ValueChannel a, float b) => a + new ConstantChannel(-b);

    public static SumChannel operator -(float b, ValueChannel a) => -a + new ConstantChannel(b);

    public static ProductChannel operator *(ValueChannel a, float b) => a * new ConstantChannel(b);

    public static ProductChannel operator *(float b, ValueChannel a) => a * new ConstantChannel(b);

    public static ProductChannel operator *(ValueChannel a, ValueChannel b) => new ProductChannel(a, b);

    public static ProductChannel operator /(ValueChannel a, float b) => a * new ConstantChannel(1 / b);

    public static ProductChannel operator /(float b, ValueChannel a) => new ConstantChannel(b) * new InverseChannel(a);

    public static ProductChannel operator /(ValueChannel a, ValueChannel b) => a * new InverseChannel(b);

    public static ModuloChannel operator %(ValueChannel a, ValueChannel b) => new ModuloChannel(a, b);

    public static ModuloChannel operator %(float a, ValueChannel b) => new ConstantChannel(a) % b;

    public static ModuloChannel operator %(ValueChannel a, float b) => a % new ConstantChannel(b);

    public abstract float ValueAt(int timing);

    public abstract List<object>? SerializeProperties(ScenecontrolSerialization serialization);

    public ValueChannel Find(string name)
    {
        if (Name == name)
        {
            return this;
        }

        foreach (var child in GetChildrenChannels())
        {
            var channel = child.Find(name);
            if (channel != null)
            {
                return channel;
            }
        }

        return null;
    }

    protected abstract IEnumerable<ValueChannel> GetChildrenChannels();
}