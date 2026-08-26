using Antlr4.Runtime;
using ArcCreate.ChartFormat.Grammar;
using Etoile.Lite.Parser.Types;

namespace Etoile.Lite.Parser;

public class ArcaeaChartReader(string[] lines)
{
    private string[] Lines { get; } = lines;

    public ArcaeaChartVisitor       Visitor       { get; } = new();
    public ArcaeaChartErrorListener ErrorListener { get; } = new();

    public void Parse()
    {
        var antlrInput = new AntlrInputStream(string.Join("\n", Lines));
        var lexer = new ArcaeaAffChartLexer(antlrInput);
        var tokens = new CommonTokenStream(lexer);
        var parser = new ArcaeaAffChartParser(tokens);

        parser.RemoveErrorListeners();
        parser.AddErrorListener(ErrorListener);

        Visitor.VisitChart(parser.chart());
    }

    public IEnumerable<string> GetReferencedFiles()
    {
        HashSet<string> files = [];

        foreach (var ev in Visitor.Events)
        {
            if (ev is RawArc a && !string.IsNullOrWhiteSpace(a.Sfx) && a.Sfx != "none")
            {
                string sfx = a.Sfx;
                if (sfx.EndsWith("_wav"))
                {
                    sfx = sfx[..^"_wav".Length] + ".wav";
                }

                /*
                 * this behavior is ArcCreate exclusive, commented out
                 
                if (!sfx.EndsWith(".wav"))
                {
                    sfx = sfx + ".wav";
                }
                */

                files.Add(sfx);
            }
        }

        return files;
    }
}