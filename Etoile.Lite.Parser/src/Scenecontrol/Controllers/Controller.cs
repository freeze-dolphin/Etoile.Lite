using Etoile.Lite.Parser.Scenecontrol.Channels;
using Etoile.Lite.Parser.Scenecontrol.Channels.MathChannels;
using Etoile.Lite.Parser.Scenecontrol.Channels.StringChannels;
using Etoile.Lite.Parser.Scenecontrol.IO;
using Etoile.Lite.Parser.Utility;

namespace Etoile.Lite.Parser.Scenecontrol.Controllers;

public abstract class Controller : IController, ISceneController
{
    public required string SerializedType { get; init; }

    public void CleanController()
    {
        Reset();
    }

    public          ValueChannel Active        { get; set; }
    public required bool         DefaultActive { get; init; }

    public void Reset()
    {
        Active = new ConstantChannel(DefaultActive ? 1 : 0);
        if (this is IPositionController pos)
        {
            pos.TranslationX = new ConstantChannel(pos.DefaultTranslation.X);
            pos.TranslationY = new ConstantChannel(pos.DefaultTranslation.Y);
            pos.TranslationZ = new ConstantChannel(pos.DefaultTranslation.Z);
            pos.RotationX = new ConstantChannel(pos.DefaultRotation.ToEulerAngles().X);
            pos.RotationY = new ConstantChannel(pos.DefaultRotation.ToEulerAngles().Y);
            pos.RotationZ = new ConstantChannel(pos.DefaultRotation.ToEulerAngles().Z);
            pos.ScaleX = new ConstantChannel(pos.DefaultScale.X);
            pos.ScaleY = new ConstantChannel(pos.DefaultScale.Y);
            pos.ScaleZ = new ConstantChannel(pos.DefaultScale.Z);
            pos.EnablePositionModule = false;
        }

        if (this is IColorController col)
        {
            col.ColorR = new ConstantChannel(col.DefaultColor.R);
            col.ColorG = new ConstantChannel(col.DefaultColor.G);
            col.ColorB = new ConstantChannel(col.DefaultColor.B);
            col.ColorA = new ConstantChannel(col.DefaultColor.A);
            col.ColorH = new ConstantChannel(0);
            col.ColorS = new ConstantChannel(0);
            col.ColorV = new ConstantChannel(0);
            col.EnableColorModule = false;
        }

        if (this is ILayerController lyr)
        {
            lyr.Layer = StringChannelBuilder.Constant(lyr.DefaultLayer);
            lyr.Sort = new ConstantChannel(lyr.DefaultSort);
            lyr.Alpha = new ConstantChannel(lyr.DefaultAlpha * 255f);
            lyr.EnableLayerModule = false;
        }

        /*
         * ITextController is not referenced by Arcaea
         */

        if (this is INoteGroupController tg)
        {
            tg.AngleX = new ConstantChannel(0);
            tg.AngleY = new ConstantChannel(0);
            tg.RotationIndividualX = new ConstantChannel(0);
            tg.RotationIndividualY = new ConstantChannel(0);
            tg.RotationIndividualZ = new ConstantChannel(0);
            tg.ScaleIndividualX = new ConstantChannel(1);
            tg.ScaleIndividualY = new ConstantChannel(1);
            tg.ScaleIndividualZ = new ConstantChannel(1);
            tg.EnableNoteGroupModule = false;
        }

        if (this is ICameraController cam)
        {
            cam.FieldOfView = new ConstantChannel(cam.DefaultFieldOfView);
            cam.TiltFactor = new ConstantChannel(1);
            cam.EnableCameraModule = false;
        }

        /*
         * IRectController is not referenced by Arcaea
         */

        if (this is ITextureController txtr)
        {
            txtr.TextureOffsetX = new ConstantChannel(txtr.DefaultTextureOffset.X);
            txtr.TextureOffsetY = new ConstantChannel(txtr.DefaultTextureOffset.Y);
            txtr.TextureScaleX = new ConstantChannel(txtr.DefaultTextureScale.X);
            txtr.TextureScaleY = new ConstantChannel(txtr.DefaultTextureScale.Y);
            txtr.EnableTextureModule = false;
        }

        if (this is ITrackController trk)
        {
            trk.EdgeLAlpha = new ConstantChannel(255);
            trk.EdgeRAlpha = new ConstantChannel(255);
            trk.Lane1Alpha = new ConstantChannel(255);
            trk.Lane2Alpha = new ConstantChannel(255);
            trk.Lane3Alpha = new ConstantChannel(255);
            trk.Lane4Alpha = new ConstantChannel(255);
            trk.EnableTrackModule = false;
        }
    }

    public List<object> SerializeProperties(ScenecontrolSerialization serialization)
    {
        var result = new List<object?>
        {
            null, // leave a null for `CustomParent` field
            serialization.AddUnitAndGetId(Active)
        };

        if (this is IPositionController pos)
        {
            result.Add(pos.EnablePositionModule);
            result.Add(serialization.AddUnitAndGetId(pos.TranslationX));
            result.Add(serialization.AddUnitAndGetId(pos.TranslationY));
            result.Add(serialization.AddUnitAndGetId(pos.TranslationZ));
            result.Add(serialization.AddUnitAndGetId(pos.RotationX));
            result.Add(serialization.AddUnitAndGetId(pos.RotationY));
            result.Add(serialization.AddUnitAndGetId(pos.RotationZ));
            result.Add(serialization.AddUnitAndGetId(pos.ScaleX));
            result.Add(serialization.AddUnitAndGetId(pos.ScaleY));
            result.Add(serialization.AddUnitAndGetId(pos.ScaleZ));
        }

        if (this is IColorController col)
        {
            result.Add(col.EnableColorModule);
            result.Add(serialization.AddUnitAndGetId(col.ColorR));
            result.Add(serialization.AddUnitAndGetId(col.ColorG));
            result.Add(serialization.AddUnitAndGetId(col.ColorB));
            result.Add(serialization.AddUnitAndGetId(col.ColorA));
            result.Add(serialization.AddUnitAndGetId(col.ColorH));
            result.Add(serialization.AddUnitAndGetId(col.ColorS));
            result.Add(serialization.AddUnitAndGetId(col.ColorV));
        }

        if (this is ILayerController lyr)
        {
            result.Add(lyr.EnableLayerModule);
            result.Add(serialization.AddUnitAndGetId(lyr.Layer));
            result.Add(serialization.AddUnitAndGetId(lyr.Sort));
            result.Add(serialization.AddUnitAndGetId(lyr.Alpha));
        }

        /*
         * ITextController is not referenced by Arcaea
         */

        if (this is INoteGroupController tg)
        {
            result.Add(tg.EnableNoteGroupModule);
            result.Add(serialization.AddUnitAndGetId(tg.AngleX));
            result.Add(serialization.AddUnitAndGetId(tg.AngleY));
            result.Add(serialization.AddUnitAndGetId(tg.RotationIndividualX));
            result.Add(serialization.AddUnitAndGetId(tg.RotationIndividualY));
            result.Add(serialization.AddUnitAndGetId(tg.RotationIndividualZ));
            result.Add(serialization.AddUnitAndGetId(tg.ScaleIndividualX));
            result.Add(serialization.AddUnitAndGetId(tg.ScaleIndividualY));
            result.Add(serialization.AddUnitAndGetId(tg.ScaleIndividualZ));
        }

        if (this is ICameraController cam)
        {
            result.Add(cam.EnableCameraModule);
            result.Add(serialization.AddUnitAndGetId(cam.FieldOfView));
            result.Add(serialization.AddUnitAndGetId(cam.TiltFactor));
        }

        /*
         * IRectController is not referenced by Arcaea
         */

        if (this is ITextureController txtr)
        {
            result.Add(txtr.EnableTextureModule);
            result.Add(serialization.AddUnitAndGetId(txtr.TextureOffsetX));
            result.Add(serialization.AddUnitAndGetId(txtr.TextureOffsetY));
            result.Add(serialization.AddUnitAndGetId(txtr.TextureScaleX));
            result.Add(serialization.AddUnitAndGetId(txtr.TextureScaleY));
        }

        if (this is ITrackController track)
        {
            result.Add(track.EnableTrackModule);
            result.Add(serialization.AddUnitAndGetId(track.EdgeLAlpha));
            result.Add(serialization.AddUnitAndGetId(track.EdgeRAlpha));
            result.Add(serialization.AddUnitAndGetId(track.Lane1Alpha));
            result.Add(serialization.AddUnitAndGetId(track.Lane2Alpha));
            result.Add(serialization.AddUnitAndGetId(track.Lane3Alpha));
            result.Add(serialization.AddUnitAndGetId(track.Lane4Alpha));
            result.Add(null); // leave a null for `CustomSkin` field
        }

        return result;
    }
}