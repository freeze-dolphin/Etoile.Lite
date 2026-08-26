using System.ComponentModel;
using System.Text.Json;
using System.Text.RegularExpressions;
using Etoile.Lite.Data;
using Etoile.Lite.Parser;
using Etoile.Lite.Parser.Scenecontrol;
using Etoile.Lite.Parser.Types;
using Etoile.Lite.Utility;
using Spectre.Console.Cli;
using static Moonad.NextExtensions;
using ValidationResult = Spectre.Console.ValidationResult;

namespace Etoile.Lite;

internal static partial class Program
{
    private static int Main(string[] args)
    {
        var app = new CommandApp<ProgramCommand>();
        return app.Run(args);
    }

    private sealed class ProgramSettings : CommandSettings
    {
        [CommandArgument(0, "<songlist>")]
        [Description("The songlist file defines song metadata")]
        public required FileInfo Songlist { get; init; }

        [CommandOption("-s|--songs")]
        [Description("Identifiers of the songs to convert")]
        public required string[] Songs { get; init; }

        [CommandOption("-r|--regex")]
        [Description("Whether to enable regex matching for song identifiers")]
        public bool RegexMatch { get; init; }

        [CommandOption("-p|--prefix")]
        [DefaultValue("etoile")]
        [Description("The prefix of the package identifier")]
        public string Prefix { get; init; } = "etoile";

        [CommandOption("-w|--overwrite")]
        [Description("Whether to overwrite existing package files")]
        public bool Overwrite { get; init; }

        [CommandOption("-j|--jobs|--parallel")]
        [DefaultValue(0)]
        [Description("Number of parallelism")]
        public int ParallelJobs { get; init; }

        public override ValidationResult Validate()
        {
            if (!Songlist.Exists)
            {
                return ValidationResult.Error("Cannot resolve the songlist file");
            }

            if (string.IsNullOrEmpty(Prefix))
            {
                return ValidationResult.Error("The prefix cannot be empty");
            }

            if (!OptionPrefixValidatorRegex().IsMatch(Prefix))
            {
                return ValidationResult.Error("The prefix must contain only letters and numbers");
            }

            if (Songs.Length == 0)
            {
                return ValidationResult.Error("At least one song identifier is required");
            }

            return ValidationResult.Success();
        }
    }

    private sealed class ProgramCommand : Command<ProgramSettings>
    {
        protected override int Execute(CommandContext context, ProgramSettings settings, CancellationToken cancellationToken)
        {
            Songlist? songlist;

            try
            {
                songlist = settings.Songlist.FullName
                                   .Next(File.ReadAllText)
                                   .Next(x => JsonSerializer.Deserialize<Songlist>(x));
            }
            catch (JsonException ex)
            {
                ex.LineNumber?.Next(line =>
                                        DiagnosticPrinter.PrintContext(
                                            settings.Songlist.FullName,
                                            (int)line + 1,
                                            message: ex.Message.Split("Path: $")[0],
                                            reason: "Unable to deserialize the songlist."
                                        ));
                return 1;
            }

            const string pathLfdy = @"Z:\Workspace\arc\fragments-category\songs\testify\3.aff";
            var chartLfdy = File.ReadLines(pathLfdy).ToArray();
            var reader = new ArcaeaChartReader(chartLfdy);
            reader.Parse();
            var errorListener = reader.ErrorListener;

            if (errorListener.Errors.Count != 0)
            {
                var error = errorListener.Errors[0];

                DiagnosticPrinter.PrintContext(
                    pathLfdy,
                    error.LineNumber,
                    message: error.Message,
                    reason: "Unable to parse the chart."
                );

                return 2;
            }

            List<(RawTimingGroup properties, IEnumerable<RawEvent> events)> groups = [];
            groups.AddRange(reader.Visitor.TimingGroups.Select((rawTg, tg) => (rawTg, reader.Visitor.Events.Where(e => e.TimingGroup == tg))));

            var serializer = new ArcCreateChartSerializer(reader.Visitor.AudioOffset, reader.Visitor.TimingPointDensityFactor, groups);
            serializer.Serialize();

            File.WriteAllText(@"C:\Users\Administrator\Downloads\lowiro.testify\testify\3.aff", serializer.ChartResult);

            var sc = new ScenecontrolService();
            var env = new ScenecontrolEnvironment(sc);
            var rebuiltResult = env.Rebuild(reader.Visitor.Events.OfType<RawScenecontrol>());
            if (rebuiltResult.IsOk)
            {
                File.WriteAllText(@"C:\Users\Administrator\Downloads\lowiro.testify\testify\3.sc_etoile.json", sc.Export());
            }
            else
            {
                Console.WriteLine(rebuiltResult.ErrorValue.Item2);
                throw rebuiltResult.ErrorValue.Item1;
            }

            return 0;
        }
    }

    [GeneratedRegex("^[a-zA-Z0-9]*$")]
    private static partial Regex OptionPrefixValidatorRegex();
}