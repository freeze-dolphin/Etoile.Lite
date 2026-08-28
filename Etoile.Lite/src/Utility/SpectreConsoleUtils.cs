using Spectre.Console;
using Spectre.Console.Rendering;

namespace Etoile.Lite.Utility;

public sealed class ValueColumn : ProgressColumn
{
    public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
    {
        return new Markup($"[white]{task.Value:N0}[/] / [grey]{task.MaxValue:N0}[/]");
    }
}