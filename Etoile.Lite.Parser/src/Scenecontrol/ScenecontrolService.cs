using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Etoile.Lite.Parser.Scenecontrol.Controllers;
using Etoile.Lite.Parser.Scenecontrol.IO;
using Etoile.Lite.Parser.Utility;
using Newtonsoft.Json;

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

    public string Export()
    {
        var serialization = new ScenecontrolSerialization();
        if (referencedControllers.Count == 0)
        {
            return "[]";
        }

        foreach (var c in referencedControllers)
        {
            serialization.AddUnitAndGetId(c);
        }

        return JsonConvert.SerializeObject(serialization.Result);
    }

    public void AddReferencedController(ISceneController c)
    {
        if (!referencedControllers.Contains(c))
        {
            referencedControllers.Add(c);
        }
    }
}