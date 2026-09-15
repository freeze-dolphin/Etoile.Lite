using Etoile.Lite.Parser.Scenecontrol.Channels;
using Etoile.Lite.Parser.Scenecontrol.Channels.EffectChannels;
using Etoile.Lite.Parser.Types;

namespace Etoile.Lite.Parser.Scenecontrol.Handler;

public class EnwidenCameraType(ScenecontrolService scenecontrolService) : IScenecontrolCommandHandler
{
    private bool       setup;
    private KeyChannel enwidenCameraFactor;

    public string Typename => "enwidencamera";

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

        enwidenCameraFactor.AddKey(timing, enwidenCameraFactor.ValueAt(timing));
        enwidenCameraFactor.AddKey(timing + duration, toggle);
    }

    private void SetupData()
    {
        enwidenCameraFactor = new KeyChannel().SetDefaultEasing("l").AddKey(-999999, 0);

        var camera = scenecontrolService.Scene.GameplayCamera;
        var skyline = scenecontrolService.Scene.SkyInputLine;
        var skylabel = scenecontrolService.Scene.SkyInputLabel;
        var singleL = scenecontrolService.Scene.SingleLineL;
        var singleR = scenecontrolService.Scene.SingleLineR;

        ValueChannel ypos = (Context.Is16By9 * 1.5f) + 3;
        float skyDeltaY = 2.745f;
        float singleDeltaX = 5;
        camera.TranslationY += enwidenCameraFactor   * 4.5f;
        camera.TranslationZ += enwidenCameraFactor   * ypos;
        skyline.TranslationY += enwidenCameraFactor  * skyDeltaY;
        skylabel.TranslationY += enwidenCameraFactor * skyDeltaY;
        singleL.TranslationX += enwidenCameraFactor  * singleDeltaX;
        singleR.TranslationX -= enwidenCameraFactor  * singleDeltaX;
    }
}