using System.Text;
using Etoile.Lite.Parser.Scenecontrol;
using Etoile.Lite.Parser.Types;
using Etoile.Lite.Parser.Utility;
using Moonad;

namespace Etoile.Lite.Parser;

public class ArcCreateChartSerializer(
    int                         audioOffset,
    double                      density,
    IEnumerable<RawEvent>       events,
    IEnumerable<RawTimingGroup> timingGroups
)
{
    private readonly StringBuilder chart        = new();
    private readonly StringBuilder scenecontrol = new();

    public string ChartResult                     => chart.ToString();
    public string ScenecontrolSerializationResult => scenecontrol.ToString();

    public Result<(Exception, RawScenecontrol?)> Serialize()
    {
        IEnumerable<(RawTimingGroup, IEnumerable<RawEvent>)> groups = timingGroups.Select((rawTg, tg) => (rawTg, events.Where(e => e.TimingGroup == tg)));

        bool baseGroup = true;
        foreach (var (properties, evts) in groups)
        {
            if (!baseGroup)
            {
                SerializeTimingGroupStart(properties);
            }
            else
            {
                SerializeChartSettings();
            }

            foreach (var e in evts)
            {
                SerializeEvent(e, !baseGroup);
            }

            if (!baseGroup)
            {
                SerializeTimingGroupEnd();
            }

            baseGroup = false;
        }

        var sc = new ScenecontrolService();
        var env = new ScenecontrolEnvironment(sc);
        var scBuildResult = env.Rebuild(events.OfType<RawScenecontrol>());
        if (scBuildResult.IsOk)
        {
            sc.Export()?.Next(scenecontrol.AppendLfLine);
            return Result<(Exception, RawScenecontrol?)>.Ok();
        }

        return scBuildResult;
    }

    private void SerializeTimingGroupStart(RawTimingGroup properties)
    {
        chart.AppendLfLine($"timinggroup({properties.ToString()}){{");
    }

    private void SerializeChartSettings()
    {
        chart.AppendLfLine($"AudioOffset:{audioOffset}");
        if (!density.Approximately(1))
        {
            chart.AppendLfLine($"TimingPointDensityFactor:{density:f1}");
        }

        chart.AppendLfLine("-");
    }

    private static bool HasDecimal(double value, float epsilon = 0.001f)
    {
        return Math.Abs(value % 1) > epsilon;
    }

    private void SerializeEvent(RawEvent affEvent, bool doesIndent = false)
    {
        string indent = doesIndent ? "  " : "";
        switch (affEvent.Type)
        {
            case RawEventType.Timing:
                RawTiming timing = affEvent as RawTiming;
                chart.AppendLfLine($"{indent}timing({timing.Timing},{timing.Bpm:f2},{timing.Divisor:f2});");
                break;

            case RawEventType.Tap:
                RawTap tap = affEvent as RawTap;
                if (HasDecimal(tap.Lane))
                {
                    chart.AppendLfLine($"{indent}({tap.Timing},{tap.Lane:f3});");
                    break;
                }

                chart.AppendLfLine($"{indent}({tap.Timing},{tap.Lane:N0});");
                break;

            case RawEventType.Hold:
                RawHold hold = affEvent as RawHold;
                if (HasDecimal(hold.Lane))
                {
                    chart.AppendLfLine($"{indent}hold({hold.Timing},{hold.EndTiming},{hold.Lane:f3});");
                    break;
                }

                chart.AppendLfLine($"{indent}hold({hold.Timing},{hold.EndTiming},{hold.Lane:N0});");
                break;

            case RawEventType.Arc:
                RawArc arc = affEvent as RawArc;
                string arcStr =
                    $"{indent}arc({arc.Timing},{arc.EndTiming},{arc.XStart:f2},{arc.XEnd:f2}," +
                    $"{arc.LineType},{arc.YStart:f2},{arc.YEnd:f2},"                           +
                    $"{arc.Color},{arc.Sfx ?? "none"},{(arc.IsTrace ? "true" : "false")}";
                if (arc.ArcResolutionMultiplier.Approximately(1.0f))
                {
                    arcStr += ")";
                }
                else
                {
                    arcStr += $",{arc.ArcResolutionMultiplier:f2})";
                }

                if (arc.ArcTaps.Count != 0)
                {
                    arcStr += "[";
                    for (int i = 0; i < arc.ArcTaps.Count; ++i)
                    {
                        if (arc.ArcTaps[i].Width != 1)
                        {
                            arcStr += $"arctap({arc.ArcTaps[i].Timing},{arc.ArcTaps[i].Width:f2})";
                        }
                        else
                        {
                            arcStr += $"arctap({arc.ArcTaps[i].Timing})";
                        }

                        if (i != arc.ArcTaps.Count - 1)
                        {
                            arcStr += ",";
                        }
                    }

                    arcStr += "]";
                }

                arcStr += ";";
                chart.AppendLfLine(arcStr);
                break;

            case RawEventType.Camera:
                RawCamera cam = affEvent as RawCamera;
                string camStr =
                    $"{indent}camera({cam.Timing},{cam.Move.X:f2},{cam.Move.Y:f2},{cam.Move.Z:f2}," +
                    $"{cam.Rotate.X:f2},{cam.Rotate.Y:f2},{cam.Rotate.Z:f2},"                       +
                    $"{cam.CameraType},{cam.Duration});";
                chart.AppendLfLine(camStr);
                break;

            case RawEventType.Scenecontrol:
                RawScenecontrol scc = affEvent as RawScenecontrol;

                if (scc.Arguments.Count == 0)
                {
                    chart.AppendLfLine($"{indent}scenecontrol({scc.Timing},{scc.ScenecontrolTypeName});");
                }
                else
                {
                    string parameterString = ",";

                    parameterString += scc.Arguments[0].ToString("0.00") + ",";
                    parameterString += scc.Arguments[1].ToString("0")    + ",";

                    parameterString = parameterString.Remove(parameterString.Length - 1);
                    chart.AppendLfLine($"{indent}scenecontrol({scc.Timing},{scc.ScenecontrolTypeName}{parameterString});");
                }

                break;
        }
    }

    private void SerializeTimingGroupEnd()
    {
        chart.AppendLfLine("};");
    }
}