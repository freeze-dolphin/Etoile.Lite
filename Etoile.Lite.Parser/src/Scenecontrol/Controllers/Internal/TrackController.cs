using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;
using Etoile.Lite.Parser.Scenecontrol.Channels;

namespace Etoile.Lite.Parser.Scenecontrol.Controllers;

public class TrackController : SpriteController, ITrackController
{
    private readonly ScenecontrolService scenecontrolService;

    [SetsRequiredMembers]
    public TrackController(ScenecontrolService scenecontrolService)
        : base(
            serializedType: "track",
            defaultActive: true,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(0f, 0f, 53.5f),
            defaultRotation: new Quaternion(-0.7071068f, 0f, 0f, 0.7071068f),
            defaultScale: new Vector3(1.7896f, 1f, 1f),
            defaultLayer: "Track",
            defaultSort: 0,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f))
    {
        this.scenecontrolService = scenecontrolService;
        
        Reset();
    }

    public bool EnableTrackModule { get; set; }

    public SpriteController DivideLine01
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "divline01",
            defaultActive: true,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(-4.76f, 0f, 0f),
            defaultRotation: new Quaternion(0f, 0f, 0f, 1f),
            defaultScale: new Vector3(0.5587685f, 12.43724f, 1f),
            defaultLayer: "Track",
            defaultSort: 2,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    public SpriteController DivideLine12
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "divline12",
            defaultActive: true,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(2.38f, 0f, 0f),
            defaultRotation: new Quaternion(0f, 0f, 0f, 1f),
            defaultScale: new Vector3(0.5587685f, 12.43724f, 1f),
            defaultLayer: "Track",
            defaultSort: 2,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    public SpriteController DivideLine23
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "divline23",
            defaultActive: true,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(0f, 0f, 0f),
            defaultRotation: new Quaternion(0f, 0f, 0f, 1f),
            defaultScale: new Vector3(0.5587685f, 12.43724f, 1f),
            defaultLayer: "Track",
            defaultSort: 2,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    public SpriteController DivideLine34
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "divline34",
            defaultActive: true,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(-2.38f, 0f, 0f),
            defaultRotation: new Quaternion(0f, 0f, 0f, 1f),
            defaultScale: new Vector3(0.5587685f, 12.43724f, 1f),
            defaultLayer: "Track",
            defaultSort: 2,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    public SpriteController DivideLine45
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "divline45",
            defaultActive: false,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(4.76f, 0f, 0f),
            defaultRotation: new Quaternion(0f, 0f, 0f, 1f),
            defaultScale: new Vector3(0.5587685f, 12.43724f, 1f),
            defaultLayer: "Track",
            defaultSort: 2,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    public SpriteController CriticalLine0
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "critline0",
            defaultActive: false,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(5.96f, 54f, 0f),
            defaultRotation: new Quaternion(1f, 0f, 0f, 0f),
            defaultScale: new Vector3(239f, 3.7f, 1f),
            defaultLayer: "Track",
            defaultSort: 2,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    public SpriteController CriticalLine1
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "critline1",
            defaultActive: true,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(3.565f, 54f, 0f),
            defaultRotation: new Quaternion(1f, 0f, 0f, 0f),
            defaultScale: new Vector3(239f, 3.7f, 1f),
            defaultLayer: "Track",
            defaultSort: 2,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    public SpriteController CriticalLine2
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "critline2",
            defaultActive: true,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(1.185f, 54f, 0f),
            defaultRotation: new Quaternion(1f, 0f, 0f, 0f),
            defaultScale: new Vector3(239f, 3.7f, 1f),
            defaultLayer: "Track",
            defaultSort: 2,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    public SpriteController CriticalLine3
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "critline3",
            defaultActive: true,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(-1.185f, 54f, 0f),
            defaultRotation: new Quaternion(1f, 0f, 0f, 0f),
            defaultScale: new Vector3(239f, 3.7f, 1f),
            defaultLayer: "Track",
            defaultSort: 2,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    public SpriteController CriticalLine4
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "critline4",
            defaultActive: true,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(-3.565f, 54f, 0f),
            defaultRotation: new Quaternion(1f, 0f, 0f, 0f),
            defaultScale: new Vector3(239f, 3.7f, 1f),
            defaultLayer: "Track",
            defaultSort: 2,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    public SpriteController CriticalLine5
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "critline5",
            defaultActive: false,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(-5.96f, 54f, 0f),
            defaultRotation: new Quaternion(1f, 0f, 0f, 0f),
            defaultScale: new Vector3(239f, 3.7f, 1f),
            defaultLayer: "Track",
            defaultSort: 2,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    public SpriteController ExtraL
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "extraL",
            defaultActive: false,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(5.96f, 0f, 0f),
            defaultRotation: new Quaternion(0f, 0f, 0f, 1f),
            defaultScale: new Vector3(239f, 15.35f, 1f),
            defaultLayer: "Track",
            defaultSort: 0,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    public SpriteController ExtraR
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "extraR",
            defaultActive: false,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(-5.96f, 0f, 0f),
            defaultRotation: new Quaternion(0f, 0f, 0f, 1f),
            defaultScale: new Vector3(239f, 15.35f, 1f),
            defaultLayer: "Track",
            defaultSort: 0,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    public SpriteController EdgeExtraL
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "edgeextraL",
            defaultActive: true,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(7.33f, 0f, 0f),
            defaultRotation: new Quaternion(0f, 0f, 0f, 1f),
            defaultScale: new Vector3(1f, 1f, 1f),
            defaultLayer: "Track",
            defaultSort: 0,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    public SpriteController EdgeExtraR
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "edgeextraR",
            defaultActive: false,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(-7.33f, 0f, 0f),
            defaultRotation: new Quaternion(0f, 0f, 0f, 1f),
            defaultScale: new Vector3(1f, 1f, 1f),
            defaultLayer: "Track",
            defaultSort: 0,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    public ValueChannel EdgeLAlpha
    {
        get;
        set
        {
            field = value;
            EnableTrackModule = true;
        }
    }

    public ValueChannel EdgeRAlpha
    {
        get;
        set
        {
            field = value;
            EnableTrackModule = true;
        }
    }

    public ValueChannel Lane1Alpha
    {
        get;
        set
        {
            field = value;
            EnableTrackModule = true;
        }
    }

    public ValueChannel Lane2Alpha
    {
        get;
        set
        {
            field = value;
            EnableTrackModule = true;
        }
    }

    public ValueChannel Lane3Alpha
    {
        get;
        set
        {
            field = value;
            EnableTrackModule = true;
        }
    }

    public ValueChannel Lane4Alpha
    {
        get;
        set
        {
            field = value;
            EnableTrackModule = true;
        }
    }

    public IEnumerable<SpriteController> GetChildren() =>
    [
        EdgeExtraL,
        ExtraL,
        CriticalLine0,
        CriticalLine1,
        CriticalLine2,
        CriticalLine3,
        CriticalLine4,
        CriticalLine5,
        ExtraR,
        EdgeExtraR,

        DivideLine01,
        DivideLine12,
        DivideLine23,
        DivideLine34,
        DivideLine45
    ];
}