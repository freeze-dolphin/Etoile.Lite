using System.Drawing;
using System.Numerics;
using Etoile.Lite.Parser.Scenecontrol.Channels;
using Etoile.Lite.Parser.Scenecontrol.Channels.StringChannels;

#pragma warning disable
namespace Etoile.Lite.Parser.Scenecontrol.Controllers;

public interface IController
{
    ValueChannel Active { get; set; }

    bool DefaultActive { get; init; }
}

public interface IPositionController : IController
{
    bool EnablePositionModule { get; set; }

    ValueChannel TranslationX { get; set; }
    ValueChannel TranslationY { get; set; }
    ValueChannel TranslationZ { get; set; }
    ValueChannel RotationX    { get; set; }
    ValueChannel RotationY    { get; set; }
    ValueChannel RotationZ    { get; set; }
    ValueChannel ScaleX       { get; set; }
    ValueChannel ScaleY       { get; set; }
    ValueChannel ScaleZ       { get; set; }

    Vector3    DefaultTranslation { get; init; }
    Quaternion DefaultRotation    { get; init; }
    Vector3    DefaultScale       { get; init; }
}

public interface IColorController : IController
{
    bool EnableColorModule { get; set; }

    ValueChannel ColorR { get; set; }
    ValueChannel ColorG { get; set; }
    ValueChannel ColorB { get; set; }
    ValueChannel ColorH { get; set; }
    ValueChannel ColorS { get; set; }
    ValueChannel ColorV { get; set; }
    ValueChannel ColorA { get; set; }

    Color DefaultColor { get; init; }
}

public interface ILayerController : IController
{
    bool EnableLayerModule { get; set; }

    StringChannel Layer { get; set; }
    ValueChannel  Sort  { get; set; }
    ValueChannel  Alpha { get; set; }

    string DefaultLayer { get; init; }
    int    DefaultSort  { get; init; }
    float  DefaultAlpha { get; init; }
}

public interface INoteGroupController : IController
{
    bool EnableNoteGroupModule { get; set; }

    ValueChannel AngleX              { get; set; }
    ValueChannel AngleY              { get; set; }
    ValueChannel RotationIndividualX { get; set; }
    ValueChannel RotationIndividualY { get; set; }
    ValueChannel RotationIndividualZ { get; set; }
    ValueChannel ScaleIndividualX    { get; set; }
    ValueChannel ScaleIndividualY    { get; set; }
    ValueChannel ScaleIndividualZ    { get; set; }
}

public interface ICameraController : IController
{
    bool EnableCameraModule { get; set; }

    ValueChannel FieldOfView { get; set; }
    ValueChannel TiltFactor  { get; set; }

    float DefaultFieldOfView { get; init; }
}

public interface ITextureController : IController
{
    bool EnableTextureModule { get; set; }

    ValueChannel TextureOffsetX { get; set; }
    ValueChannel TextureOffsetY { get; set; }
    ValueChannel TextureScaleX  { get; set; }
    ValueChannel TextureScaleY  { get; set; }

    Vector2 DefaultTextureOffset { get; init; }
    Vector2 DefaultTextureScale  { get; init; }
}

public interface ITrackController : IController
{
    bool EnableTrackModule { get; set; }

    ValueChannel EdgeLAlpha { get; set; }
    ValueChannel EdgeRAlpha { get; set; }
    ValueChannel Lane1Alpha { get; set; }
    ValueChannel Lane2Alpha { get; set; }
    ValueChannel Lane3Alpha { get; set; }
    ValueChannel Lane4Alpha { get; set; }
}