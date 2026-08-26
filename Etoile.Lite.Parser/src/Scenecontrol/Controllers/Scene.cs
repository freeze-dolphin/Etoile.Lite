using System.Drawing;
using System.Numerics;

namespace Etoile.Lite.Parser.Scenecontrol.Controllers;

public class Scene(ScenecontrolService scenecontrolService)
{
    public CameraController GameplayCamera
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "camera",
            defaultActive: true,
            defaultFieldOfView: 0,
            defaultTranslation: new Vector3(0f, 0f, 0f),
            defaultRotation: new Quaternion(0f, 0f, 0f, 1f),
            defaultScale: new Vector3(1f, 1f, 1f));

    public TrackController Track
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(scenecontrolService);

    public SpriteController SingleLineL
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "singlelinel",
            defaultActive: true,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(4f, 7.15f, 50f),
            defaultRotation: new Quaternion(-0.6272114f, -0.3265055f, 0.3265055f, 0.6272114f),
            defaultScale: new Vector3(1f, 1f, 1f),
            defaultLayer: "Foreground",
            defaultSort: 0,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    public SpriteController SingleLineR
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "singleliner",
            defaultActive: true,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(-4f, 7.15f, 50f),
            defaultRotation: new Quaternion(0.3265056f, 0.6272114f, -0.6272114f, -0.3265056f),
            defaultScale: new Vector3(1f, 1f, 1f),
            defaultLayer: "Foreground",
            defaultSort: 0,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    public SpriteController SkyInputLine
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "skyinputline",
            defaultActive: true,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(0f, 5.5f, 0f),
            defaultRotation: new Quaternion(0f, 0f, 0f, 1f),
            defaultScale: new Vector3(5000f, 1f, 1f),
            defaultLayer: "UI",
            defaultSort: 0,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    public SpriteController SkyInputLabel
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "skyinputlabel",
            defaultActive: true,
            defaultColor: Color.FromArgb(255, 255, 255, 255),
            defaultTranslation: new Vector3(-7.1f, 5.65f, 0f),
            defaultRotation: new Quaternion(0f, 0f, 0f, 1f),
            defaultScale: new Vector3(1f, 1f, 1f),
            defaultLayer: "UI",
            defaultSort: -1,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    public BeatlinesController Beatlines
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "beatlines",
            defaultActive: true,
            defaultTranslation: new Vector3(0f, 0f, 0f),
            defaultRotation: new Quaternion(0f, 0f, 0f, 1f),
            defaultScale: new Vector3(1f, 1f, 1f));

    public SpriteController Darken
    {
        get
        {
            scenecontrolService.AddReferencedController(field);
            return field;
        }
    } = new(serializedType: "darken",
            defaultActive: false,
            defaultColor: Color.FromArgb(255, 0, 0, 0),
            defaultTranslation: new Vector3(0f, 0f, 0f),
            defaultRotation: new Quaternion(0f, 0f, 0f, 1f),
            defaultScale: new Vector3(180f, 180f, 0f),
            defaultLayer: "Background",
            defaultSort: 2,
            defaultAlpha: 1,
            defaultTextureOffset: new Vector2(0f, 0f),
            defaultTextureScale: new Vector2(1f, 1f));

    private readonly Dictionary<int, NoteGroupController> _noteGroupControllerCache = new();

    public NoteGroupController GetNoteGroup(int tg)
    {
        if (_noteGroupControllerCache.TryGetValue(tg, out NoteGroupController? noteGroupController))
        {
            return noteGroupController;
        }

        var c = new NoteGroupController(serializedType: $"tg.{tg}",
                                        defaultActive: true,
                                        defaultTranslation: new Vector3(0f, 0f, 0f),
                                        defaultRotation: new Quaternion(0f, 0f, 0f, 1f),
                                        defaultScale: new Vector3(1f, 1f, 1f),
                                        defaultColor: Color.FromArgb(255, 255, 255, 255));
        c.Reset();
        scenecontrolService.AddReferencedController(c);
        _noteGroupControllerCache.Add(tg, c);

        return c;
    }

    public void ClearCache()
    {
        _noteGroupControllerCache.Clear();
    }
}