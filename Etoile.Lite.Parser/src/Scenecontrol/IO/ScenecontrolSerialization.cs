using Etoile.Lite.Parser.Scenecontrol.Channels.ContextChannels;
using Etoile.Lite.Parser.Scenecontrol.Channels.EffectChannels;
using Etoile.Lite.Parser.Scenecontrol.Channels.MathChannels;
using Etoile.Lite.Parser.Scenecontrol.Channels.StringChannels;

namespace Etoile.Lite.Parser.Scenecontrol.IO;

public class ScenecontrolSerialization
{
    private readonly List<ISerializableUnit>            units    = [];
    private readonly Dictionary<ISerializableUnit, int> idLookup = new();

    public List<SerializedUnit> Result { get; }

    public ScenecontrolSerialization()
    {
        var versioning = new ScenecontrolVersioning(EnabledFeatures.All);
        units.Add(versioning);
        idLookup.Add(versioning, 0);

#if ENABLE_GENERATOR_INFO
        var generatorInfo = StringChannelBuilder.Create()
                                                .AddKey(int.MinValue, "__GeneratorInfo__")
                                                .AddKey(0, "**This file is created by Etoile.Lite**")
                                                .AddKey(1, "https://github.com/freeze-dolphin/Etoile.Lite")
                                                .AddKey(int.MaxValue, $"GeneratedAt_{DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()}");
        units.Add(generatorInfo);
        idLookup.Add(generatorInfo, 1);
#endif

        Result =
        [
            new SerializedUnit
            {
                Type = GetTypeFromUnit(versioning),
                Properties = versioning.SerializeProperties(this)
            },
#if ENABLE_GENERATOR_INFO
            new SerializedUnit
            {
                Type = GetTypeFromUnit(generatorInfo),
                Properties = generatorInfo.SerializeProperties(this)
            }
#endif
        ];
    }

    public int? AddUnitAndGetId(ISerializableUnit unit)
    {
        if (unit == null)
        {
            return null;
        }

        if (idLookup.TryGetValue(unit, out int id))
        {
            return id;
        }

        units.Add(unit);
        id = units.Count - 1;
        idLookup.Add(unit, id);
        SerializedUnit serialized = default;
        Result.Add(serialized);
        Result[id] = new SerializedUnit
        {
            Type = GetTypeFromUnit(unit),
            Properties = unit.SerializeProperties(this),
        };

        return id;
    }

    public string GetTypeFromUnit(ISerializableUnit unit)
    {
        switch (unit)
        {
            case ScenecontrolVersioning versioning:
                return "versioning";
            case Context context:
                return "context";

            // Channels
            case KeyChannel key:
                return "channel.key";
            case ConstantChannel constant:
                return "channel.const";
            case InverseChannel inverse:
                return "channel.inverse";
            case NegateChannel negate:
                return "channel.negate";
            case ProductChannel product:
                return "channel.product";
            case ModuloChannel moduluo:
                return "channel.modulo";
            case SumChannel sum:
                return "channel.sum";

            // String channels
            case KeyStringChannel keystring:
                return "channel.string.key";

            // Contexts
            case ScreenIs16By9Channel is16by9:
                return "channel.context.is16by9";

            default:
                if (unit is ISceneController controller)
                {
                    string name = controller?.SerializedType;
                    if (!string.IsNullOrEmpty(name))
                    {
                        return name;
                    }
                }

                throw new Exception($"Could not get type of object: {unit.GetType().Name}");
        }
    }
}