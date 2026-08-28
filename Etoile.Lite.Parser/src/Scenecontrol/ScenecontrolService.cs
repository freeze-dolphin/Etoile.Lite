using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Etoile.Lite.Parser.Scenecontrol.Controllers;
using Etoile.Lite.Parser.Scenecontrol.IO;
using Etoile.Lite.Parser.Utility;

namespace Etoile.Lite.Parser.Scenecontrol;

public class ScenecontrolService
{
    private readonly List<ISceneController> referencedControllers = [];

    public Scene   Scene   { get; }
    public Context Context { get; }

    public ScenecontrolService()
    {
        Scene = new Scene(this);
        Context = new Context(this);
    }

    public void Clean()
    {
        Scene.ClearCache();
        foreach (var c in referencedControllers)
        {
            c.CleanController();
        }

        referencedControllers.Clear();
    }

    public void AddReferencedController(ISceneController c)
    {
        if (!referencedControllers.Contains(c))
        {
            referencedControllers.Add(c);
        }
    }

    public string? Export()
    {
        var serialization = new ScenecontrolSerialization();
        if (referencedControllers.Count == 0)
        {
            return null;
        }

        foreach (var c in referencedControllers)
        {
            serialization.AddUnitAndGetId(c);
        }

        return JsonSerializer.Serialize(serialization.Result, Options);
    }

    private static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        IncludeFields = true,
        Converters = { new DoubleConverter(), new FloatConverter() }
    };

    private sealed class DoubleConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(
            Utf8JsonWriter        writer,
            double                value,
            JsonSerializerOptions options)
        {
            if (value.Approximately(0)) value = Math.Abs(value);
            writer.WriteRawValue(value.ToString("0.0###", CultureInfo.InvariantCulture));
        }
    }

    private sealed class FloatConverter : JsonConverter<float>
    {
        public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(
            Utf8JsonWriter        writer,
            float                 value,
            JsonSerializerOptions options)
        {
            if (value.Approximately(0)) value = Math.Abs(value);
            writer.WriteRawValue(value.ToString("0.0###", CultureInfo.InvariantCulture));
        }
    }
}