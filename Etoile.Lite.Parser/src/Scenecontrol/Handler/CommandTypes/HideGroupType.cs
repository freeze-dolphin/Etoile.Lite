using Etoile.Lite.Parser.Scenecontrol.Channels;
using Etoile.Lite.Parser.Scenecontrol.Channels.EffectChannels;
using Etoile.Lite.Parser.Scenecontrol.Controllers;
using Etoile.Lite.Parser.Types;
using Moonad;

namespace Etoile.Lite.Parser.Scenecontrol.Handler;

public class HideGroupType(ScenecontrolService scenecontrolService) : IScenecontrolHandler
{
    public string Typename => "hidegroup";

    public void ExecuteCommand(RawScenecontrol ev)
    {
        int timing = ev.Timing;
        float hidden = ev.Arguments[1];

        NoteGroupController noteGroup = scenecontrolService.Scene.GetNoteGroup(ev.TimingGroup);

        ValueChannel channel = noteGroup.Active.Find("internal");
        if (channel == null)
        {
            channel = new KeyChannel().SetDefaultEasing("cnsti").AddKey(-999999, 1);
            channel.Name = "internal";
            noteGroup.Active *= channel;
        }

        KeyChannel c = channel as KeyChannel;

        c.AddKey(timing, 1 - hidden);
    }
}