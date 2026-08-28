using System.ComponentModel;
using System.IO.Compression;
using System.Text.Json;
using System.Text.RegularExpressions;
using Etoile.Lite.Data;
using Etoile.Lite.Parser;
using Etoile.Lite.Parser.Utility;
using Etoile.Lite.Utility;
using Moonad;
using Spectre.Console;
using Spectre.Console.Cli;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using ValidationResult = Spectre.Console.ValidationResult;

namespace Etoile.Lite;

internal static partial class Program
{
    private static readonly HashSet<string> ActivePartialFiles     = new();
    private static readonly object          ActivePartialFilesLock = new();

    static Program()
    {
        Console.CancelKeyPress += (_,              _) => CleanupPartialFiles();
        AppDomain.CurrentDomain.ProcessExit += (_, _) => CleanupPartialFiles();
    }

    private static void RegisterPartialFile(string path)
    {
        lock (ActivePartialFilesLock)
        {
            ActivePartialFiles.Add(path);
        }
    }

    private static void UnregisterPartialFile(string path)
    {
        lock (ActivePartialFilesLock)
        {
            ActivePartialFiles.Remove(path);
        }
    }

    private static void CleanupPartialFiles()
    {
        lock (ActivePartialFilesLock)
        {
            foreach (var p in new List<string>(ActivePartialFiles))
            {
                try
                {
                    if (File.Exists(p)) File.Delete(p);
                }
                catch
                {
                    AnsiConsole.MarkupLine("[red]Error:[/] Failed to cleanup partial files.");
                }
            }

            ActivePartialFiles.Clear();
        }
    }

    private static int Main(string[] args)
    {
        var app = new CommandApp<ProgramCommand>();
        return app.Run(args);
    }

    private sealed class ProgramSettings : CommandSettings
    {
        [CommandArgument(0, "<songlist>")]
        [Description("Path to the songlist file defines song metadata")]
        public required FileInfo Songlist { get; init; }

        [CommandOption("-s|--songs")]
        [Description("Identifiers of the songs to convert")]
        public required string[] Songs { get; init; } = [];

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

        [CommandOption("-o|--output")]
        [Description("Path to the result files parent dir")]
        public string Output { get; init; } = "./results";

        [CommandOption("-j|--jobs|--parallel")]
        [DefaultValue(0)]
        [Description("Number of parallelism")]
        public int ParallelJobs { get; init; }

        public override ValidationResult Validate()
        {
            if (!Songlist.Exists || Songlist.Directory?.FullName is null)
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

            try
            {
                Directory.CreateDirectory(Output);
            }
            catch (Exception)
            {
                return ValidationResult.Error($"Failed to create the output dir: {Output}");
            }

            if (RegexMatch)
            {
                foreach (var songPattern in Songs)
                {
                    try
                    {
                        _ = new Regex(songPattern);
                    }
                    catch (ArgumentException ex)
                    {
                        return ValidationResult.Error($"The song pattern '{songPattern}' is not a valid regular expression: {ex.Message}");
                    }
                }
            }

            return ValidationResult.Success();
        }
    }

    private sealed class ProgramCommand : Command<ProgramSettings>
    {
        private static Result<Songlist, Exception> GetSonglist(ProgramSettings settings)
        {
            try
            {
                return settings.Songlist.FullName
                               .Next(File.ReadAllText)
                               .Next(x => JsonSerializer.Deserialize<Songlist>(x)!);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private static readonly Func<SonglistEntry, bool> DefaultFilter = x => !x.Deleted.HasValue || !x.Deleted.Value;

        private static Func<SonglistEntry, bool> GetSonglistEntryFilter(ProgramSettings settings)
        {
            if (settings.Songs.Length == 0)
            {
                // defaults to pack all
                return DefaultFilter;
            }

            if (settings.RegexMatch)
            {
                return entry => DefaultFilter(entry) && settings.Songs.Any(songPattern => Regex.IsMatch(entry.Id, songPattern, RegexOptions.CultureInvariant));
            }

            return entry => DefaultFilter(entry) && settings.Songs.Contains(entry.Id, StringComparer.OrdinalIgnoreCase);
        }

        private const string CharterLowiro = "© lowiro";

        private static readonly ISerializer YamlSerializer =
            new SerializerBuilder()
               .WithNamingConvention(CamelCaseNamingConvention.Instance)
               .Build();

        protected override int Execute(CommandContext context, ProgramSettings settings, CancellationToken cancellationToken)
        {
            if (!GetSonglist(settings).TryUnwrap(out var songlist, out Exception slstEx))
            {
                if (slstEx is JsonException jsonEx)
                {
                    jsonEx.LineNumber?.Next(line =>
                                                DiagnosticPrinter.PrintContext(
                                                    settings.Songlist.FullName,
                                                    (int)line + 1,
                                                    message: slstEx.Message.Split("Path: $")[0],
                                                    reason: "Unable to deserialize the songlist."
                                                ));

                    Environment.Exit(1);
                }

                AnsiConsole.WriteException(slstEx);
                AnsiConsole.Markup("[red]Error:[/] Unexpected error occurred while reading songlist.");
            }

            AnsiConsole
               .Progress()
               .AutoClear(true)
               .Columns(new TaskDescriptionColumn(),
                        new ProgressBarColumn
                        {
                            CompletedStyle = new Style(Color.Green),
                            FinishedStyle = new Style(Color.Lime),
                            RemainingStyle = new Style(Color.Grey),
                            Width = Console.BufferWidth
                        },
                        new ValueColumn(),
                        new SpinnerColumn(Spinner.Known.Dots))
               .Start(ctx =>
                    {
                        var validSonglistEntries = songlist.Songs.Where(x => GetSonglistEntryFilter(settings)(x)).ToList();
                        var task = ctx.AddTask("Packing", maxValue: validSonglistEntries.Count);

                        foreach (var songlistEntry in validSonglistEntries)
                        {
                            var qualifiedIdentifier = $"{settings.Prefix}.{songlistEntry.Id}";
                            var packageFile = Path.Combine(settings.Output, $"{qualifiedIdentifier}.arcpkg");
                            if (!settings.Overwrite && File.Exists(packageFile))
                            {
                                task.Increment(1);
                                continue;
                            }

                            var songDir = Path.Combine(settings.Songlist.Directory!.FullName, songlistEntry.Id);

                            List<string> backgroundSearchLocations =
                            [
                                // @formatter:off
                                songDir,
                                Path.Combine(songDir, "..", "..", "img", "bg", "1080"),
                                Path.Combine(songDir, "..", "..", "img", "bg"),
                                // @formatter:on
                            ];

                            List<string> referencedFiles = [];

                            var difficulties =
                                songlistEntry.Difficulties.Where(x =>
                                                                     x.RatingClass >= 0 &&
                                                                     Path.Combine(songDir, $"{x.RatingClass}.aff").Next(File.Exists))
                                             .ToList();

                            if (difficulties.Count == 0)
                            {
                                AnsiConsole.Markup($"[red]Error:[/] Failed to find any chart for song `{songlistEntry.Id}`");
                                Environment.Exit(1);
                            }

                            var chartEntries =
                                difficulties
                                   .Select(difficultyEntry =>
                                    {
                                        #region Preparation

                                        /* jacket */
                                        var jacketLowRes = difficultyEntry.JacketOverride is true
                                            ? $"{difficultyEntry.RatingClass}.jpg"
                                            : "base.jpg";
                                        var jacketPath = Path.Combine(songDir, $"1080_{jacketLowRes}").Next(File.Exists)
                                            ? $"1080_{jacketLowRes}"
                                            : jacketLowRes; // search high-res

                                        /* background */
                                        var backgroundName = difficultyEntry.Background ?? songlistEntry.Background;
                                        string? backgroundPath;
                                        var location = backgroundSearchLocations
                                           .Find(loc => Directory.EnumerateFiles(loc)
                                                                 .Any(p => Path.GetFileName(p) == $"{backgroundName}.jpg"));

                                        if (location is not null)
                                        {
                                            backgroundPath = $"{backgroundName}.jpg";
                                            referencedFiles.Add(Path.Combine(location, backgroundPath).Next(Path.GetFullPath));
                                        }
                                        else
                                        {
                                            backgroundPath = null;
                                        }

                                        #endregion

                                        var chartSettings = new ChartSettings
                                        {
                                            ChartPath = $"{difficultyEntry.RatingClass}.aff",
                                            AudioPath = difficultyEntry.AudioOverride is true ? $"{difficultyEntry.RatingClass}.ogg" : "base.ogg",
                                            JacketPath = jacketPath,
                                            BaseBpm = difficultyEntry.BpmBase ?? songlistEntry.BpmBase,
                                            BpmText = difficultyEntry.BpmText ?? songlistEntry.BpmText,
                                            BackgroundPath = backgroundPath,
                                            Title = difficultyEntry.Title?["en"] ?? songlistEntry.Title["en"],
                                            Composer = difficultyEntry.Artist    ?? songlistEntry.Artist,
                                            Charter = settings.Prefix == "lowiro"
                                                ? CharterLowiro
                                                : settings.Prefix,
                                            Alias = difficultyEntry.ChartDesigner,
                                            Illustrator = difficultyEntry.JacketDesigner,
                                            Difficulty = ArcCreatePackHelper.GetDifficultyString(difficultyEntry.RatingClass,
                                                                                                 difficultyEntry.Rating,
                                                                                                 difficultyEntry.RatingPlus,
                                                                                                 difficultyEntry.RatingClassAlias),
                                            ChartConstant = difficultyEntry.Rating + (difficultyEntry.RatingPlus is true ? 0.7 : 0),
                                            DifficultyColor = ArcCreatePackHelper.GetDifficultyColor(difficultyEntry.RatingClass,
                                                                                                     difficultyEntry.RatingClassAlias),
                                            Skin = ArcCreatePackHelper.GetSkinSettings(songlistEntry.Side,
                                                                                       songlistEntry.Id,
                                                                                       songlistEntry.PackName,
                                                                                       backgroundName)
                                                                      .FlattenForSerialization(),
                                            SearchTags = songlistEntry.Title.Values
                                                                      .ConcatIfNotNull(songlistEntry.SearchTitle?.Values.SelectMany(x => x))
                                                                      .ConcatIfNotNull(songlistEntry.SearchArtist?.Values.SelectMany(x => x))
                                                                      .Distinct()
                                                                      .Next(x => string.Join('\n', x))
                                        };

                                        return chartSettings;
                                    })
                                   .ToList();

                            List<((string ChartPath, string ChartResult), (string ScenePath, string ScenecontrolSerializationResult))> serializationResult =
                                chartEntries
                                   .Select(chartEntry =>
                                    {
                                        var reader = new ArcaeaChartReader(File.ReadLines(Path.Combine(songDir, chartEntry.ChartPath)).ToArray());
                                        reader.Parse();

                                        var errorListener = reader.ErrorListener;
                                        if (errorListener.Errors.Count != 0)
                                        {
                                            var error = errorListener.Errors[0];

                                            DiagnosticPrinter.PrintContext(
                                                chartEntry.ChartPath,
                                                error.LineNumber,
                                                message: error.Message,
                                                reason: "Unable to parse the chart."
                                            );

                                            Environment.Exit(2);
                                        }

                                        referencedFiles.AddRange(reader.GetReferencedFiles());
                                        referencedFiles.Add(chartEntry.AudioPath);
                                        referencedFiles.Add(chartEntry.JacketPath);

                                        var serializer = new ArcCreateChartSerializer(reader.Visitor.AudioOffset,
                                                                                      reader.Visitor.TimingPointDensityFactor,
                                                                                      reader.Visitor.Events,
                                                                                      reader.Visitor.TimingGroups);
                                        var serResult = serializer.Serialize();
                                        if (serResult.IsError)
                                        {
                                            var (ex, ev) = serResult.ErrorValue;

                                            if (ev == null)
                                            {
                                                AnsiConsole.Markup("[red]Error:[/] Unknown error occurred during scene serialization.");
                                                Environment.Exit(2);
                                            }

                                            DiagnosticPrinter.PrintContext(
                                                chartEntry.ChartPath,
                                                ev.LineNumber,
                                                message: ex.Message,
                                                reason: "Unable to serialize the scene."
                                            );

                                            Environment.Exit(2);
                                        }

                                        return ((chartEntry.ChartPath, serializer.ChartResult),
                                                (Path.ChangeExtension(chartEntry.ChartPath, ".sc.json"), serializer.ScenecontrolSerializationResult));
                                    })
                                   .ToList();

                            var partialFile = packageFile + ".partial";
                            RegisterPartialFile(partialFile);
                            try
                            {
                                using var fos = new FileStream(partialFile, FileMode.Create, FileAccess.Write);
                                using (var archive = new ZipArchive(fos, ZipArchiveMode.Create))
                                {
                                    /* index */
                                    using (var es = archive.CreateEntry(ImportInformation.FileName).Open())
                                    using (var wrt = new StreamWriter(es))
                                    {
                                        wrt.Write(YamlSerializer.Serialize(new List<ImportInformation>([
                                            new ImportInformation
                                            {
                                                Directory = songlistEntry.Id,
                                                Identifier = qualifiedIdentifier,
                                                SettingsFile = "etoile.arcproj",
                                                Type = ImportInformation.LevelType
                                            }
                                        ])));
                                    }

                                    /* project settings */
                                    using (var es = archive.CreateEntry($"{songlistEntry.Id}/etoile.arcproj").Open())
                                    using (var wrt = new StreamWriter(es))
                                    {
                                        wrt.Write(YamlSerializer.Serialize(new ProjectSettings
                                        {
                                            LastOpenedChartPath = ArcCreatePackHelper.GetLastOpenedChartPath(difficulties),
                                            Charts = chartEntries,
                                            EditorSettings = new EditorProjectSettings
                                            {
                                                LastUsedPublisher = settings.Prefix,
                                                LastUsedPackageName = songlistEntry.Id
                                            }
                                        }));
                                    }

                                    /* chart files */
                                    foreach (var ser in serializationResult)
                                    {
                                        using (var es = archive.CreateEntry($"{songlistEntry.Id}/{ser.Item1.ChartPath}").Open())
                                        using (var wrt = new StreamWriter(es))
                                        {
                                            wrt.Write(ser.Item1.ChartResult);
                                        }

                                        if (!string.IsNullOrEmpty(ser.Item2.ScenecontrolSerializationResult))
                                        {
                                            using var es = archive.CreateEntry($"{songlistEntry.Id}/{ser.Item2.ScenePath}").Open();
                                            using var wrt = new StreamWriter(es);
                                            wrt.Write(ser.Item2.ScenecontrolSerializationResult);
                                        }
                                    }

                                    /* referenced files */
                                    foreach (var referencedFile in referencedFiles.Distinct())
                                    {
                                        using var es = archive.CreateEntry($"{songlistEntry.Id}/{Path.GetFileName(referencedFile)}").Open();
                                        using var fs = File.OpenRead(Path.Combine(songDir, referencedFile));
                                        fs.CopyTo(es);
                                    }
                                }

                                // move partial into final file name
                                try
                                {
                                    if (File.Exists(packageFile)) File.Delete(packageFile);
                                    File.Move(partialFile, packageFile);
                                }
                                catch (Exception)
                                {
                                    try
                                    {
                                        if (File.Exists(partialFile)) File.Delete(partialFile);
                                    }
                                    catch
                                    {
                                    }

                                    UnregisterPartialFile(partialFile);
                                    throw;
                                }

                                UnregisterPartialFile(partialFile);
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    if (File.Exists(partialFile)) File.Delete(partialFile);
                                }
                                catch
                                {
                                }

                                UnregisterPartialFile(partialFile);
                                throw;
                            }

                            AnsiConsole.MarkupLine(
                                $"[lime]Ok:[/] [white]Packed successfully to `{Path.GetRelativePath(Environment.CurrentDirectory, packageFile)}`.[/]");
                            task.Increment(1);
                        }
                    }
                );

            return 0;
        }
    }

    [GeneratedRegex("^[a-zA-Z0-9]*$")]
    private static partial Regex OptionPrefixValidatorRegex();
}