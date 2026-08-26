using Antlr4.Runtime;

namespace Etoile.Lite.Parser;

public class ArcaeaChartErrorListener : BaseErrorListener
{
    public List<ChartFileError> Errors { get; } = [];

    public override void SyntaxError(TextWriter           output,
                                     IRecognizer          recognizer,
                                     IToken               offendingSymbol,
                                     int                  line,
                                     int                  charPositionInLine,
                                     string               msg,
                                     RecognitionException e)
    {
        Errors.Add(new ChartFileError(msg, offendingSymbol));
    }
}