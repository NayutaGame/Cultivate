
using System;
using System.Collections.Generic;
using CLLibrary;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class CanvasManager : Singleton<CanvasManager>, Addressable
{
    [TabGroup("General")] public AppCanvas AppCanvas;
    [TabGroup("General")] public RunCanvas RunCanvas;
    [TabGroup("General")] public StageCanvas StageCanvas;
    [TabGroup("General")] [SerializeField] private Camera Camera;
    [TabGroup("General")] [SerializeField] private GraphicRaycaster Raycaster;

    [TabGroup("Annotations")] public AnnotationView CharacterAnnotation;
    [TabGroup("Annotations")] public AnnotationView SkillAnnotation;
    [TabGroup("Annotations")] public AnnotationView BuffAnnotation;
    [TabGroup("Annotations")] public AnnotationView FormationAnnotation;
    [TabGroup("Annotations")] public AnnotationView RoomAnnotation;
    [TabGroup("Annotations")] public TextHint TextHint;
    
    [TabGroup("Others")]public PinAnchor PinAnchor;
    [TabGroup("Others")]public MergePreresultView MergePreresultView;
    [TabGroup("Others")]public GuideView GuideView;
    [TabGroup("Others")]public Curtain Curtain;
    [TabGroup("Others")]public TMP_FontAsset ArmorFontAsset;
    [TabGroup("Others")]public TMP_FontAsset FragileFontAsset;
    
    [TabGroup("List")] [ColorPalette("JingJie")] public Color[] JingJieColors;
    [TabGroup("List")] public Sprite[] JingJieSprites;
    [TabGroup("List")] [ColorPalette("WuXing")] public Color[] WuXingColors;
    [TabGroup("List")] public Color[] CostColors;
    [TabGroup("List")] public Sprite[] CostIconSprites;

    public Color GetWuXingColor(WuXing? wuXing)
    {
        if (wuXing == null)
            return WuXingColors[^1];

        return WuXingColors[wuXing.Value._index];
    }

    public RectTransform GetPinAnchorRect()
        => PinAnchor.GetRect();

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public override void DidAwake()
    {
        base.DidAwake();

        _accessors = new Dictionary<string, Func<object>>()
        {
            { "CharacterAnnotation", () => CharacterAnnotation },
            { "SkillAnnotation", () => SkillAnnotation },
            { "BuffAnnotation", () => BuffAnnotation },
            { "FormationAnnotation", () => FormationAnnotation },
            { "RoomAnnotation", () => RoomAnnotation },
        };

        _volume.profile.TryGet(out _vignette);
        
        _results = new();

        CharacterAnnotation.Awake();
        SkillAnnotation.Awake();
        BuffAnnotation.Awake();
        FormationAnnotation.Awake();
        RoomAnnotation.Awake();
        
        MergePreresultView.CheckAwake();
        
        GuideView.SetAddress(new Address("Run.Environment.ActivePanel.Guide"));
        
        AppCanvas.Configure();
        Curtain.AwakeFunction();
    }

    public void RefreshGuide()
    {
        GuideView.Refresh();
    }

    private List<RaycastResult> _results;

    public bool RayCastIsHit(PointerEventData d)
    {
        _results.Clear();
        Raycaster.Raycast(d, _results);
        return _results.Count >= 1 && _results[0].gameObject.GetComponent<LegacyInteractBehaviour>() != null;
    }

    public LegacyInteractBehaviour FirstRayCastHit(PointerEventData d)
    {
        _results.Clear();
        Raycaster.Raycast(d, _results);
        if (_results.Count < 1)
            return null;
        
        return _results[0].gameObject.GetComponent<LegacyInteractBehaviour>();
    }

    public Vector3 UI2World(Vector2 screenPosition)
    {
        return Camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10));
    }

    public Vector3 World2UI(Vector3 worldPosition)
    {
        return Camera.WorldToScreenPoint(worldPosition);
    }

    public Vector3 ScreenCenterInWorld()
    {
        return Camera.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));
    }

    public string GetGraphicRaycastResult()
    {
        var d = new PointerEventData(null);
        d.position = Input.mousePosition;
        
        _results.Clear();
        Raycaster.Raycast(d, _results);
        if (_results.Count == 0)
            return "no result";
        return _results[0].gameObject.name;
    }

    [TabGroup("Effect")] [SerializeField] private Volume _volume;

    #region RedFlash
    
    private Vignette _vignette;
    private Tween _redFlashHandle;

    public void RedFlashAnimation()
    {
        _redFlashHandle?.Kill();
        _redFlashHandle = DOTween.Sequence()
            .AppendCallback(() => _vignette.active = true)
            .Append(DOTween.To(GetIntensity, SetIntensity, 0.2f, 0.1f).SetEase(Ease.OutQuad))
            .Append(DOTween.To(GetIntensity, SetIntensity, 0, 0.1f).SetEase(Ease.InQuad))
            .AppendCallback(() => _vignette.active = false);
        _redFlashHandle.SetAutoKill().Restart();
    }

    private float GetIntensity()
        => _vignette.intensity.value;

    private void SetIntensity(float value)
        => _vignette.intensity.value = value;

    #endregion

    #region CanvasShake

    [TabGroup("Effect")] [SerializeField] private RectTransform _shakeTransform;
    private Tween _shakeHandle;

    public void CanvasShakeAnimation()
    {
        _shakeHandle?.Kill();
        _shakeHandle = DOTween.Sequence()
            .Append(_shakeTransform.DOShakePosition(0.1f, 5f, 100, 90, randomnessMode: ShakeRandomnessMode.Full))
            .Append(_shakeTransform.DOMove(Vector3.zero, 0.05f));
        _shakeHandle.SetAutoKill().Restart();
    }

    #endregion
    
    #region FloatText

    [TabGroup("Effect")] [SerializeField] private Transform VFXPoolTransform;
    [TabGroup("Effect")] [SerializeField] private GameObject FloatTextVFXPrefab;

    public void UIFloatTextVFX(string context, Color color)
    {
        GameObject gao = Instantiate(FloatTextVFXPrefab, UI2World(new Vector2(Screen.width / 2, Screen.height / 2)),
            Quaternion.identity, VFXPoolTransform);
    
        TMP_Text text = gao.GetComponent<UIFloatTextVFX>().Text;
        text.text = context;
        text.color = color;
    }
    
    #endregion
}
