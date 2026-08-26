using Etoile.Lite.Parser.Scenecontrol.Channels;
using Etoile.Lite.Parser.Scenecontrol.Channels.EffectChannels;
using Etoile.Lite.Parser.Scenecontrol.Channels.MathChannels;
using Etoile.Lite.Parser.Types;

namespace Etoile.Lite.Parser.Scenecontrol.Handler;

public class EnwidenLanesType(ScenecontrolService scenecontrolService) : IScenecontrolHandler
{
    private bool       setup;
    private KeyChannel enwidenLaneFactor;

    public string Typename => "enwidenlanes";

    public void ExecuteCommand(RawScenecontrol ev)
    {
        if (!setup)
        {
            SetupData();
            setup = true;
        }

        int timing = ev.Timing;
        int duration = Convert.ToInt32(ev.Arguments[0]);
        float toggle = ev.Arguments[1] >= 0.5f ? 1 : 0;

        enwidenLaneFactor.AddKey(timing, enwidenLaneFactor.ValueAt(timing));
        enwidenLaneFactor.AddKey(timing + duration, toggle);
    }

    private void SetupData()
    {
        enwidenLaneFactor = new KeyChannel().SetDefaultEasing("l").AddKey(-999999, 0);
        var beatline = scenecontrolService.Scene.Beatlines;

        ValueChannel posY = -100 * (1 - enwidenLaneFactor);
        ValueChannel alpha = 1 - enwidenLaneFactor;

        var track = scenecontrolService.Scene.Track;
        track.ExtraL.Active = new ConstantChannel(1);
        track.ExtraR.Active = new ConstantChannel(1);
        track.CriticalLine0.Active = new ConstantChannel(1);
        track.CriticalLine5.Active = new ConstantChannel(1);
        track.DivideLine01.Active = new ConstantChannel(1);
        track.DivideLine45.Active = new ConstantChannel(1);
        track.EdgeExtraL.Active = new ConstantChannel(1);
        track.EdgeExtraR.Active = new ConstantChannel(1);

        track.EdgeExtraL.ColorA *= enwidenLaneFactor;
        track.ExtraL.ColorA *= enwidenLaneFactor;
        track.ExtraL.TranslationY += posY;
        track.CriticalLine0.ColorA *= enwidenLaneFactor;
        track.DivideLine01.ColorA *= enwidenLaneFactor;
        track.EdgeLAlpha *= alpha;
        track.EdgeRAlpha *= alpha;
        track.DivideLine45.ColorA *= enwidenLaneFactor;
        track.CriticalLine5.ColorA *= enwidenLaneFactor;
        track.ExtraR.TranslationY += posY;
        track.ExtraR.ColorA *= enwidenLaneFactor;
        track.EdgeExtraR.ColorA *= enwidenLaneFactor;

        scenecontrolService.Context.LaneTo += enwidenLaneFactor;
        scenecontrolService.Context.LaneFrom -= enwidenLaneFactor;

        beatline.ScaleX *= (enwidenLaneFactor * 0.5f) + 1;
    }
}