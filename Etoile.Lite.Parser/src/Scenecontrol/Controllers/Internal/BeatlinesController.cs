using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Etoile.Lite.Parser.Scenecontrol.Channels;

namespace Etoile.Lite.Parser.Scenecontrol.Controllers;

public class BeatlinesController : Controller, IPositionController
{
    [SetsRequiredMembers]
    public BeatlinesController(string serializedType, bool defaultActive, Vector3 defaultTranslation, Quaternion defaultRotation, Vector3 defaultScale)
    {
        SerializedType = serializedType;
        DefaultActive = defaultActive;
        DefaultTranslation = defaultTranslation;
        DefaultRotation = defaultRotation;
        DefaultScale = defaultScale;
        
        Reset();
    }

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

    public required Vector3    DefaultTranslation { get; init; }
    public required Quaternion DefaultRotation    { get; init; }
    public required Vector3    DefaultScale       { get; init; }

    #endregion
}