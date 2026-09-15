using System.Numerics;
using System.Text.RegularExpressions;
using Antlr4.Runtime.Tree;
using ArcCreate.ChartFormat.Grammar;
using Etoile.Lite.Parser.Types;
using Etoile.Lite.Parser.Utility;
using Moonad;

namespace Etoile.Lite.Parser;

public partial class ArcaeaChartVisitor : ArcaeaAffChartBaseVisitor<object>
{
    private int CurrentTimingGroup => TimingGroups.Count - 1;

    private int    _audioOffset              = 0;
    private double _timingPointDensityFactor = 1;

    public int    AudioOffset              => _audioOffset;
    public double TimingPointDensityFactor => _timingPointDensityFactor;

    public  List<RawEvent>       Events       { get; } = [];
    public  List<RawTimingGroup> TimingGroups { get; } = [new()];
    private List<RawArcTap>      ArcTaps      { get; } = [];

#pragma warning disable CS8603 // Possible null reference return.
    public override object VisitHeader(ArcaeaAffChartParser.HeaderContext context)
    {
        switch (context.Word().GetText())
        {
            case "AudioOffset":
                _audioOffset = context.Int().ParseInt();
                break;

            case "TimingPointDensityFactor":
                _timingPointDensityFactor = context.Int() == null ? context.Float().ParseFloat() : context.Int().ParseInt();
                break;
        }

        return null;
    }

    public override object VisitChart(ArcaeaAffChartParser.ChartContext context)
    {
        foreach (var h in context.header()) VisitHeader(h);
        VisitBody(context.body());

        return null;
    }

    public override object VisitEvent(ArcaeaAffChartParser.EventContext context)
    {
        VisitChildren(context);

        return null;
    }

    public override object VisitEventTiming(ArcaeaAffChartParser.EventTimingContext context)
    {
        var tick = context.Int().ParseInt();
        var bpm = context.Float(0).ParseFloat();
        var divisor = context.Float(1).ParseFloat();

        Events.Add(new RawTiming
        {
            Type = RawEventType.Timing,
            Timing = tick,
            Divisor = divisor,
            Bpm = bpm,
            TimingGroup = CurrentTimingGroup,
            LineNumber = context.Start.Line,
        });

        return null;
    }

    public override object VisitEventTap(ArcaeaAffChartParser.EventTapContext context)
    {
        var tick = context.Int(0).ParseInt();
        double lane = ParseKeyboardLaneFromNode(context.Int(1), context.Float());

        Events.Add(new RawTap
        {
            Type = RawEventType.Tap,
            Timing = tick,
            Lane = lane,
            TimingGroup = CurrentTimingGroup,
            LineNumber = context.Start.Line,
        });

        return null;
    }

    public override object VisitEventHold(ArcaeaAffChartParser.EventHoldContext context)
    {
        var tick = context.Int(0).ParseInt();
        var endTick = context.Int(1).ParseInt();
        double lane = ParseKeyboardLaneFromNode(context.Int(2), context.Float());

        Events.Add(new RawHold
        {
            Type = RawEventType.Hold,
            Timing = tick,
            EndTiming = endTick,
            Lane = lane,
            TimingGroup = CurrentTimingGroup,
            LineNumber = context.Start.Line,
        });

        return null;
    }

    private static double ParseKeyboardLaneFromNode(ITerminalNode? intNode, ITerminalNode? floatNode)
    {
        if (intNode != null)
        {
            return intNode.ParseInt();
        }

        return ParsingFormula.ArcaeaFloatedLaneToLane(floatNode!.ParseFloat());
    }

    public override object VisitEventArc(ArcaeaAffChartParser.EventArcContext context)
    {
        var tick = context.Int(0).ParseInt();
        var endTick = context.Int(1).ParseInt();
        var xStart = context.Float(0).ParseFloat();
        var xEnd = context.Float(1).ParseFloat();
        var lineType = context.Word(0).GetText();
        var yStart = context.Float(2).ParseFloat();
        var yEnd = context.Float(3).ParseFloat();
        var color = context.Int(2).ParseInt();
        var hitSound = context.Word(1).GetText();
        var arcType = context.Word(2).GetText();

        if (arcType is "designant")
        {
            // drop the designant arc notes
            return null;
        }

        double arcRes = 1.0;
        if (context.Float(4) != null)
        {
            arcRes = context.Float(4).ParseFloat();
        }

        bool isTrace = arcType is "true" or "designant";

        RawArc arc;

        ArcTaps.Clear();
        context.subEvents()?.Next(VisitSubEvents);

        if (tick == endTick            &&
            yStart.Approximately(yEnd) &&
            color == 3)
        {
            // var-len arctap

            var xCenter = (float)(xStart + xEnd) / 2;
            var yCenter = (float)((yStart + yEnd) / 2);

            var width = Math.Abs(xStart - xEnd) * 2;

            arc = new RawArc
            {
                Type = RawEventType.Arc,
                Timing = tick,
                EndTiming = tick + 1,
                XStart = xCenter,
                XEnd = xCenter,
                LineType = lineType,
                YStart = yCenter,
                YEnd = yCenter,
                Color = color,
                IsTrace = true,
                ArcTaps =
                [
                    new RawArcTap
                    {
                        Type = RawEventType.ArcTap,
                        Timing = tick,
                        TimingGroup = CurrentTimingGroup,
                        Width = width,
                        LineNumber = context.Start.Line,
                    }
                ],
                Sfx = hitSound,
                TimingGroup = CurrentTimingGroup,
                LineNumber = context.Start.Line,
            };
        }
        else
        {
            // normal arc

            arc = new RawArc
            {
                Type = RawEventType.Arc,
                Timing = tick,
                EndTiming = endTick,
                XStart = xStart,
                XEnd = xEnd,
                LineType = lineType,
                YStart = yStart,
                YEnd = yEnd,
                Color = color,
                IsTrace = isTrace,
                ArcTaps = ArcTaps.ToList(),
                Sfx = hitSound,
                TimingGroup = CurrentTimingGroup,
                ArcResolutionMultiplier = arcRes,
                LineNumber = context.Start.Line,
            };
        }

        Events.Add(arc);

        return null;
    }

    public override object VisitEventArcTap(ArcaeaAffChartParser.EventArcTapContext context)
    {
        var tick = context.Int().ParseInt();

        ArcTaps.Add(new RawArcTap
        {
            Type = RawEventType.ArcTap,
            Timing = tick,
            TimingGroup = CurrentTimingGroup,
            Width = 1,
            LineNumber = context.Start.Line,
        });

        return null;
    }

    public override object VisitEventCamera(ArcaeaAffChartParser.EventCameraContext context)
    {
        var tick = context.Int(0).ParseInt();
        var mx = context.Float(0).ParseFloat();
        var my = context.Float(1).ParseFloat();
        var mz = context.Float(2).ParseFloat();
        var rx = context.Float(3).ParseFloat();
        var ry = context.Float(4).ParseFloat();
        var rz = context.Float(5).ParseFloat();
        var type = context.Word().GetText();
        var duration = context.Int(1).ParseInt();

        Events.Add(new RawCamera
        {
            Type = RawEventType.Camera,
            TimingGroup = CurrentTimingGroup,
            Timing = tick,
            Duration = duration,
            Move = new Vector3((float)mx,
                               (float)my,
                               (float)mz),
            Rotate = new Vector3((float)rx,
                                 (float)ry,
                                 (float)rz),
            CameraType = type,
            LineNumber = context.Start.Line,
        });

        return null;
    }

    public override object VisitEventScenecontrol(ArcaeaAffChartParser.EventScenecontrolContext context)
    {
        var tick = context.Int(0).ParseInt();
        var type = context.Word().GetText()!;

        var param1 = context.Float();
        var param2 = context.Int(1);

        RawScenecontrol sc;

        // parameter-less
        if (param1 == null || param2 == null)
        {
            switch (type.ToLower())
            {
                // https://github.com/freeze-dolphin/aff-compose/blob/17d0948c3f3726336661df4b68b0e5e2a86e3ef6/src/commonMain/kotlin/com/tairitsu/compose/filter/ShimFilter.kt#L29
                case "trackhide":
                    sc = new RawScenecontrol
                    {
                        Type = RawEventType.Scenecontrol,
                        Timing = tick,
                        Arguments =
                        [
                            1000f,
                            0f
                        ],
                        ScenecontrolTypeName = "trackdisplay",
                        TimingGroup = CurrentTimingGroup,
                        LineNumber = context.Start.Line,
                    };
                    break;
                // https://github.com/freeze-dolphin/aff-compose/blob/17d0948c3f3726336661df4b68b0e5e2a86e3ef6/src/commonMain/kotlin/com/tairitsu/compose/filter/ShimFilter.kt#L30
                case "trackshow":
                    sc = new RawScenecontrol
                    {
                        Type = RawEventType.Scenecontrol,
                        Timing = tick,
                        Arguments =
                        [
                            1000f,
                            255f
                        ],
                        ScenecontrolTypeName = "trackdisplay",
                        TimingGroup = CurrentTimingGroup,
                        LineNumber = context.Start.Line,
                    };
                    break;
                case "hidegroup":
                    sc = new RawScenecontrol
                    {
                        Type = RawEventType.Scenecontrol,
                        Timing = tick,
                        Arguments =
                        [
                            0f,
                            1f
                        ],
                        ScenecontrolTypeName = "hidegroup",
                        TimingGroup = CurrentTimingGroup,
                        LineNumber = context.Start.Line,
                    };
                    break;
                default:
                    sc = new RawScenecontrol
                    {
                        Type = RawEventType.Scenecontrol,
                        Timing = tick,
                        Arguments =
                        [
                        ],
                        ScenecontrolTypeName = type,
                        TimingGroup = CurrentTimingGroup,
                        LineNumber = context.Start.Line,
                    };
                    break;
            }
        }
        else
        {
            switch (type.ToLower())
            {
                // https://github.com/freeze-dolphin/aff-compose/blob/17d0948c3f3726336661df4b68b0e5e2a86e3ef6/src/commonMain/kotlin/com/tairitsu/compose/filter/ShimFilter.kt#L32-L36
                case "trackdisplay":
                    var durationInSec = param1.ParseFloat();
                    var alpha = param2.ParseInt();

                    var duration = 1000 *
                                   (durationInSec.Approximately(0)
                                       ? 1 // defaults to 1 sec
                                       : durationInSec);

                    sc = new RawScenecontrol
                    {
                        Timing = tick,
                        Type = RawEventType.Scenecontrol,
                        Arguments =
                        [
                            (float)duration,
                            Math.Clamp(alpha,
                                       0,
                                       255)
                        ],
                        ScenecontrolTypeName = "trackdisplay",
                        TimingGroup = CurrentTimingGroup,
                        LineNumber = context.Start.Line,
                    };
                    break;
                default:
                    sc = new RawScenecontrol
                    {
                        Type = RawEventType.Scenecontrol,
                        Timing = tick,
                        Arguments =
                        [
                            (float)param1.ParseFloat(),
                            param2.ParseInt()
                        ],
                        ScenecontrolTypeName = type,
                        TimingGroup = CurrentTimingGroup,
                        LineNumber = context.Start.Line,
                    };
                    break;
            }
        }

        Events.Add(sc);

        return null;
    }

    public override object VisitEventTimingGroup(ArcaeaAffChartParser.EventTimingGroupContext context)
    {
        TimingGroups.Add(ParseTimingGroupProperties());
        VisitSegment(context.segment());

        return null;

        RawTimingGroup ParseTimingGroupProperties()
        {
            var propDict = new Dictionary<string, string?>();

            if (context.Word() != null)
            {
                foreach (var propRaw in context.Word().GetText().Split("_"))
                {
                    var match = TimingGroupPropertyRegex().Match(propRaw);
                    if (!match.Success) continue;

                    string name = match.Groups[1].Value;
                    string? value = match.Groups[2].Success ? match.Groups[2].Value : null;

                    propDict.Add(name, value);
                }
            }

            var prop = new RawTimingGroup();

            foreach (var (type, value) in propDict)
            {
                if (value != null)
                {
                    bool valid;
                    double val;
                    switch (type.ToLower())
                    {
                        // https://github.com/freeze-dolphin/aff-compose/blob/17d0948c3f3726336661df4b68b0e5e2a86e3ef6/src/commonMain/kotlin/com/tairitsu/compose/filter/ShimFilter.kt#L41-L45
                        case "anglex":
                            valid = double.TryParse(value, out val);
                            prop.AngleX = valid ? val / 10 : 0;
                            break;
                        case "angley":
                            valid = double.TryParse(value, out val);
                            prop.AngleY = valid ? val / -10 : 0;
                            break;

                        // don't throw exceptions to allow user add other identifiers for tg (but we don't parse them)
                        /*
                        default:
                            throw new ChartReaderException(raw, RawEventType.TimingGroup, evt,
                                ChartError.Kind.TimingGroupPropertiesInvalid);
                        */
                    }
                }
                else
                {
                    switch (type.ToLower())
                    {
                        case "noinput":
                            prop.NoInput = true;
                            break;
                        case "fadingholds":
                            prop.FadingHolds = true;
                            break;

                        // don't throw exceptions to allow user add other identifiers for tg (but we don't parse them)
                        /*
                        default:
                            throw new ChartReaderException(raw, RawEventType.TimingGroup, evt,
                                ChartError.Kind.TimingGroupPropertiesInvalid);
                        */
                    }
                }
            }

            return prop;
        }
    }

    public override object VisitItem(ArcaeaAffChartParser.ItemContext context)
    {
        VisitEvent(context.@event());

        return null;
    }

    public override object VisitSubEvents(ArcaeaAffChartParser.SubEventsContext context)
    {
        foreach (var evt in context.@event())
        {
            VisitEvent(evt);
        }

        return null;
    }

    public override object VisitSegment(ArcaeaAffChartParser.SegmentContext context)
    {
        VisitBody(context.body());

        return null;
    }

    public override object VisitBody(ArcaeaAffChartParser.BodyContext context)
    {
        foreach (var item in context.item())
        {
            VisitItem(item);
        }

        return null;
    }
#pragma warning restore CS8603 // Possible null reference return.

    /// <summary>
    /// https://regex101.com/r/wTAqy8/2
    /// </summary>
    [GeneratedRegex(@"([a-zA-Z]+)(-?(0|([1-9][0-9]*))(\.\d+)?)?", RegexOptions.Compiled)]
    private static partial Regex TimingGroupPropertyRegex();
}