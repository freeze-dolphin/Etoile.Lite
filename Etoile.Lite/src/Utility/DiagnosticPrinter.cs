using Spectre.Console;

namespace Etoile.Lite.Utility;

public static class DiagnosticPrinter
{
    public static void PrintContext(
        string  filePath,
        int     errorLine,
        int     contextLines = 3,
        string? reason       = null,
        string? message      = null)
    {
        int startLine = Math.Max(1, errorLine - contextLines);
        int endLine = errorLine + contextLines;

        var buffer = new Queue<(int Number, string Text)>();

        using var reader = File.OpenText(filePath);

        int currentLine = 0;

        while (reader.ReadLine() is { } line)
        {
            currentLine++;

            if (currentLine >= startLine)
            {
                buffer.Enqueue((currentLine, line));
            }

            if (currentLine > endLine)
            {
                break;
            }
        }

        AnsiConsole.WriteLine();
        
        if (reason != null)
        {
            AnsiConsole.MarkupLine($"[red bold]Error:[/] {Markup.Escape(reason)}");

            if (message != null) AnsiConsole.MarkupLine($"{new string(' ', 7)}[yellow]{Markup.Escape(message)}[/]");
        }

        AnsiConsole.WriteLine();

        int width = endLine.ToString().Length;

        foreach (var (number, text) in buffer)
        {
            string content = Markup.Escape(text);

            AnsiConsole.MarkupLine(
                number == errorLine
                    ? $"[red]> {number.ToString().PadLeft(width)} │ {content}[/]"
                    : $"[grey]  {number.ToString().PadLeft(width)} │[/] {content}"
            );
        }
    }
}