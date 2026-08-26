using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;
using Etoile.Lite.Parser.Scenecontrol.Channels;

namespace Etoile.Lite.Parser.Scenecontrol.Controllers;

public class NoteGroupController : Controller, IPositionController, INoteGroupController, IColorController
{
    [SetsRequiredMembers]
    public NoteGroupController(string serializedType, bool defaultActive, Vector3 defaultTranslation, Quaternion defaultRotation, Vector3 defaultScale, Color defaultColor)
    {
        SerializedType = serializedType;
        DefaultActive = defaultActive;
        DefaultTranslation = defaultTranslation;
        DefaultRotation = defaultRotation;
        DefaultScale = defaultScale;
        DefaultColor = defaultColor;
        
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

    #region NoteGroup

    public bool EnableNoteGroupModule { get; set; }

    public ValueChannel AngleX
    {
        get;
        set
        {
            field = value;
            EnableNoteGroupModule = true;
        }
    }

    public ValueChannel AngleY
    {
        get;
        set
        {
            field = value;
            EnableNoteGroupModule = true;
        }
    }

    public ValueChannel RotationIndividualX
    {
        get;
        set
        {
            field = value;
            EnableNoteGroupModule = true;
        }
    }

    public ValueChannel RotationIndividualY
    {
        get;
        set
        {
            field = value;
            EnableNoteGroupModule = true;
        }
    }

    public ValueChannel RotationIndividualZ
    {
        get;
        set
        {
            field = value;
            EnableNoteGroupModule = true;
        }
    }

    public ValueChannel ScaleIndividualX
    {
        get;
        set
        {
            field = value;
            EnableNoteGroupModule = true;
        }
    }

    public ValueChannel ScaleIndividualY
    {
        get;
        set
        {
            field = value;
            EnableNoteGroupModule = true;
        }
    }

    public ValueChannel ScaleIndividualZ
    {
        get;
        set
        {
            field = value;
            EnableNoteGroupModule = true;
        }
    }

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