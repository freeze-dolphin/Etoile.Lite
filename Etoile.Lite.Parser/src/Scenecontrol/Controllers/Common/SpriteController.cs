using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;
using Etoile.Lite.Parser.Scenecontrol.Channels;
using Etoile.Lite.Parser.Scenecontrol.Channels.StringChannels;

namespace Etoile.Lite.Parser.Scenecontrol.Controllers;

public class SpriteController : Controller, IPositionController, ILayerController, ITextureController, IColorController
{
    [SetsRequiredMembers]
    public SpriteController(string serializedType, bool defaultActive, Color defaultColor, Vector3 defaultTranslation, Quaternion defaultRotation, Vector3 defaultScale, string defaultLayer, int defaultSort, float defaultAlpha, Vector2 defaultTextureOffset, Vector2 defaultTextureScale)
    {
        SerializedType = serializedType;
        DefaultActive = defaultActive;
        DefaultColor = defaultColor;
        DefaultTranslation = defaultTranslation;
        DefaultRotation = defaultRotation;
        DefaultScale = defaultScale;
        DefaultLayer = defaultLayer;
        DefaultSort = defaultSort;
        DefaultAlpha = defaultAlpha;
        DefaultTextureOffset = defaultTextureOffset;
        DefaultTextureScale = defaultTextureScale;
        
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

    #region Layer

    public bool EnableLayerModule { get; set; }

    public StringChannel Layer
    {
        get;
        set
        {
            field = value;
            EnableLayerModule = true;
        }
    }

    public ValueChannel Sort
    {
        get;
        set
        {
            field = value;
            EnableLayerModule = true;
        }
    }

    public ValueChannel Alpha
    {
        get;
        set
        {
            field = value;
            EnableLayerModule = true;
        }
    }

    public required string DefaultLayer { get; init; }
    public required int    DefaultSort  { get; init; }
    public required float  DefaultAlpha { get; init; }

    #endregion

    #region Texture

    public bool EnableTextureModule { get; set; }

    public ValueChannel TextureOffsetX
    {
        get;
        set
        {
            field = value;
            EnableTextureModule = true;
        }
    }

    public ValueChannel TextureOffsetY
    {
        get;
        set
        {
            field = value;
            EnableTextureModule = true;
        }
    }

    public ValueChannel TextureScaleX
    {
        get;
        set
        {
            field = value;
            EnableTextureModule = true;
        }
    }

    public ValueChannel TextureScaleY
    {
        get;
        set
        {
            field = value;
            EnableTextureModule = true;
        }
    }

    public required Vector2 DefaultTextureOffset { get; init; }
    public required Vector2 DefaultTextureScale  { get; init; }

    #endregion

    #region Color

    public bool EnableColorModule { get; set; }

    public ValueChannel ColorR
    {
        get;
        set
        {
            field = value;
            EnableColorModule = true;
        }
    }

    public ValueChannel ColorG
    {
        get;
        set
        {
            field = value;
            EnableColorModule = true;
        }
    }

    public ValueChannel ColorB
    {
        get;
        set
        {
            field = value;
            EnableColorModule = true;
        }
    }

    public ValueChannel ColorH
    {
        get;
        set
        {
            field = value;
            EnableColorModule = true;
        }
    }

    public ValueChannel ColorS
    {
        get;
        set
        {
            field = value;
            EnableColorModule = true;
        }
    }

    public ValueChannel ColorV
    {
        get;
        set
        {
            field = value;
            EnableColorModule = true;
        }
    }

    public ValueChannel ColorA
    {
        get;
        set
        {
            field = value;
            EnableColorModule = true;
        }
    }

    public required Color DefaultColor { get; init; }

    #endregion
}