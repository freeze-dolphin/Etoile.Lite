using System.Text;
using Etoile.Lite.Parser.Types;
using Etoile.Lite.Parser.Utility;

namespace Etoile.Lite.Parser;

public class ArcCreateChartSerializer(
    int                                                                    audioOffset,
    double                                                                 density,
    IEnumerable<(RawTimingGroup properties, IEnumerable<RawEvent> events)> groups
)
{
    private readonly StringBuilder _chart        = new();
    private readonly StringBuilder _scenecontrol = new();
    
    public string ChartResult => _chart.ToString();
    public string ScenecontrolSerializationResult => _scenecontrol.ToString();

    public void Serialize()
    {
        bool baseGroup = true;
        foreach (var (properties, events) in groups)
        {
            if (!baseGroup)
            {
                SerializeTimingGroupStart(properties);
            }
            else
            {
                SerializeChartSettings();
            }

            foreach (var e in events)
            {
                SerializeEvent(e, !baseGroup);
            }

            if (!baseGroup)
            {
                SerializeTimingGroupEnd();
            }

            baseGroup = false;
        }
    }

    private void SerializeTimingGroupStart(RawTimingGroup properties)
    {
        _chart.AppendLfLine($"timinggroup({properties.ToString()}){{");
    }

    private void SerializeChartSettings()
    {
        _chart.AppendLfLine($"AudioOffset:{audioOffset}");
        if (!density.Approximately(1))
        {
            _chart.AppendLfLine($"TimingPointDensityFactor:{density:f1}");
        }

        _chart.AppendLfLine("-");
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
                _chart.AppendLfLine($"{indent}timing({timing.Timing},{timing.Bpm:f2},{timing.Divisor:f2});");
                break;

            case RawEventType.Tap:
                RawTap tap = affEvent as RawTap;
                if (HasDecimal(tap.Lane))
                {
                    _chart.AppendLfLine($"{indent}({tap.Timing},{tap.Lane:f3});");
                    break;
                }

                _chart.AppendLfLine($"{indent}({tap.Timing},{tap.Lane:N0});");
                break;

            case RawEventType.Hold:
                RawHold hold = affEvent as RawHold;
                if (HasDecimal(hold.Lane))
                {
                    _chart.AppendLfLine($"{indent}hold({hold.Timing},{hold.EndTiming},{hold.Lane:f3});");
                    break;
                }

                _chart.AppendLfLine($"{indent}hold({hold.Timing},{hold.EndTiming},{hold.Lane:N0});");
                break;

            case RawEventType.Arc:
                RawArc arc = affEvent as RawArc;
                string arcStr =
                    $"{indent}arc({arc.Timing},{arc.EndTiming},{arc.XStart:f2},{arc.XEnd:f2}," +
                    $"{arc.LineType},{arc.YStart:f2},{arc.YEnd:f2},"                           +
                    $"{arc.Color},{arc.Sfx ?? "none"},{(arc.IsTrace ? "true" : "false")}";
                if (!arc.ArcResolutionMultiplier.Approximately(1.0f))
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
                _chart.AppendLfLine(arcStr);
                break;

            case RawEventType.Camera:
                RawCamera cam = affEvent as RawCamera;
                string camStr =
                    $"{indent}camera({cam.Timing},{cam.Move.X:f2},{cam.Move.Y:f2},{cam.Move.Z:f2}," +
                    $"{cam.Rotate.X:f2},{cam.Rotate.Y:f2},{cam.Rotate.Z:f2},"                       +
                    $"{cam.CameraType},{cam.Duration});";
                _chart.AppendLfLine(camStr);
                break;

            case RawEventType.Scenecontrol:
                RawScenecontrol scc = affEvent as RawScenecontrol;

                if (scc.Arguments.Count == 0)
                {
                    _chart.AppendLfLine($"{indent}scenecontrol({scc.Timing},{scc.ScenecontrolTypeName});");
                }
                else
                {
                    string parameterString = ",";
                    
                    parameterString += scc.Arguments[0].ToString("0.00") + ",";
                    parameterString += scc.Arguments[1].ToString("0") + ",";

                    parameterString = parameterString.Remove(parameterString.Length - 1);
                    _chart.AppendLfLine($"{indent}scenecontrol({scc.Timing},{scc.ScenecontrolTypeName}{parameterString});");
                }

                break;
        }
    }

    private void SerializeTimingGroupEnd()
    {
        _chart.AppendLfLine("};");
    }
}