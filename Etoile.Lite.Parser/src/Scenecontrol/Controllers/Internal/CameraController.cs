using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Etoile.Lite.Parser.Scenecontrol.Channels;

namespace Etoile.Lite.Parser.Scenecontrol.Controllers;

public class CameraController : Controller, ICameraController, IPositionController
{
    [SetsRequiredMembers]
    public CameraController(string serializedType, bool defaultActive, float defaultFieldOfView, Vector3 defaultTranslation, Quaternion defaultRotation, Vector3 defaultScale)
    {
        SerializedType = serializedType;
        DefaultActive = defaultActive;
        DefaultFieldOfView = defaultFieldOfView;
        DefaultTranslation = defaultTranslation;
        DefaultRotation = defaultRotation;
        DefaultScale = defaultScale;
        
        Reset();
    }

    #region Camera

    public bool EnableCameraModule { get; set; }

    public ValueChannel FieldOfView
    {
        get;
        set
        {
            field = value;
            EnableCameraModule = true;
        }
    }

    public ValueChannel TiltFactor
    {
        get;
        set
        {
            field = value;
            EnableCameraModule = true;
        }
    }

    public required float DefaultFieldOfView { get; init; } = 0;

    #endregion

    #region Position

    public bool EnablePositionModule { get; set; }

    public ValueChannel TranslationX
    {
        get;
        set
        {
            field = value;
            EnablePositionModule = true;
        }
    }

    public ValueChannel TranslationY
    {
        get;
        set
        {
            field = value;
            EnablePositionModule = true;
        }
    }

    public ValueChannel TranslationZ
    {
        get;
        set
        {
            field = value;
            EnablePositionModule = true;
        }
    }

    public ValueChannel RotationX
    {
        get;
        set
        {
            field = value;
            EnablePositionModule = true;
        }
    }

    public ValueChannel RotationY
    {
        get;
        set
        {
            field = value;
            EnablePositionModule = true;
        }
    }

    public ValueChannel RotationZ
    {
        get;
        set
        {
            field = value;
            EnablePositionModule = true;
        }
    }

    public ValueChannel ScaleX
    {
        get;
        set
        {
            field = value;
            EnablePositionModule = true;
        }
    }

    public ValueChannel ScaleY
    {
        get;
        set
        {
            field = value;
            EnablePositionModule = true;
        }
    }

    public ValueChannel ScaleZ
    {
        get;
        set
        {
            field = value;
            EnablePositionModule = true;
        }
    }

    public required Vector3    DefaultTranslation { get; init; } = Vector3.Zero;
    public required Quaternion DefaultRotation    { get; init; } = Quaternion.Identity;
    public required Vector3    DefaultScale       { get; init; } = Vector3.One;

    #endregion
}