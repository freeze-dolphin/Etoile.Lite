using Etoile.Lite.Parser.Scenecontrol.Channels.EffectChannels;
using Etoile.Lite.Parser.Scenecontrol.Channels.MathChannels;
using Etoile.Lite.Parser.Types;
using Etoile.Lite.Parser.Utility;

namespace Etoile.Lite.Parser.Scenecontrol.Handler;

public class TrackDisplayType(ScenecontrolService scenecontrolService) : IScenecontrolCommandHandler
{
    private bool       setup;
    private KeyChannel trackAlphaFactor;
    private KeyChannel darkenAlphaFactor;

    public string Typename => "trackdisplay";

    public void ExecuteCommand(RawScenecontrol ev)
    {
        if (!setup)
        {
            SetupData();
            setup = true;
        }

        int timing = ev.Timing;
        int duration = Convert.ToInt32(ev.Arguments[0]);
        double alpha = ev.Arguments[1];
        float darkenAlpha = alpha.Approximately(255) ? 0 : 255;
        trackAlphaFactor.AddKey(timing, trackAlphaFactor.ValueAt(timing));
        trackAlphaFactor.AddKey(timing + duration, (float)(alpha / 255));
        darkenAlphaFactor.AddKey(timing, darkenAlphaFactor.ValueAt(timing));
        darkenAlphaFactor.AddKey(timing + 250, darkenAlpha / 255f);
    }

    private void SetupData()
    {
        trackAlphaFactor = new KeyChannel().SetDefaultEasing("l").AddKey(-999999, 1);
        darkenAlphaFactor = new KeyChannel().SetDefaultEasing("l").AddKey(-999999, 0);

        var track = scenecontrolService.Scene.Track;
        track.ColorA *= trackAlphaFactor;
        track.ExtraL.ColorA *= trackAlphaFactor;
        track.ExtraR.ColorA *= trackAlphaFactor;
        track.EdgeExtraL.ColorA *= trackAlphaFactor;
        track.EdgeExtraR.ColorA *= trackAlphaFactor;

        foreach (var obj in track.GetChildren())
        {
            obj.ColorA *= trackAlphaFactor;
        }

        var darken = scenecontrolService.Scene.Darken;
        darken.Active = new ConstantChannel(1);
        darken.ColorA *= darkenAlphaFactor;
    }
}