using Etoile.Lite.Parser.Scenecontrol.Channels;
using Etoile.Lite.Parser.Scenecontrol.Channels.EffectChannels;
using Etoile.Lite.Parser.Scenecontrol.Controllers;
using Etoile.Lite.Parser.Types;

namespace Etoile.Lite.Parser.Scenecontrol.Handler;

public class GroupAlphaType(ScenecontrolService scenecontrolService) : IScenecontrolCommandHandler
{
    public string Typename { get; } = "groupalpha";

    public void ExecuteCommand(RawScenecontrol ev)
    {
        int timing = ev.Timing;
        int duration = Convert.ToInt32(ev.Arguments[0]);
        float alpha = ev.Arguments[1];

        NoteGroupController noteGroup = scenecontrolService.Scene.GetNoteGroup(ev.TimingGroup);

        ValueChannel channel = noteGroup.ColorA.Find("internal");
        if (channel == null)
        {
            channel = new KeyChannel().SetDefaultEasing("l").AddKey(-999999, 1);
            channel.Name = "internal";
            noteGroup.ColorA *= channel;
        }

        KeyChannel c = channel as KeyChannel;

        c.AddKey(timing, c.ValueAt(timing));
        c.AddKey(timing + duration, alpha / 255f);
    }
}