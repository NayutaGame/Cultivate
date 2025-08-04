
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
    [TabGroup("General")] public AnnotationManager AnnotationManager;
    [TabGroup("General")] [SerializeField] private Camera Camera;
    [TabGroup("General")] [SerializeField] private GraphicRaycaster Raycaster;
    [TabGroup("General")] [SerializeField] private ConsolePanel ConsolePanel;
    
    [TabGroup("Others")]public Grabber Grabber;
    [TabGroup("Others")]public MergePreresultView MergePreresultView;
    [TabGroup("Others")]public GuideView GuideView;
    [TabGroup("Others")]public Curtain Curtain;
    [TabGroup("Others")]public TMP_FontAsset ArmorFontAsset;
    [TabGroup("Others")]public TMP_FontAsset FragileFontAsset;
    [TabGroup("Others")]public DialogWindow DialogWindow;
    
    [TabGroup("List")] [ColorPalette("JingJie")] public Color[] JingJieColors;
    [TabGroup("List")] public Sprite[] JingJieSprites;
    [TabGroup("List")] [ColorPalette("WuXing")] public Color[] WuXingColors;
    [TabGroup("List")] public Color[] CostColors;
    [TabGroup("List")] public Sprite[] CostIconSprites;

    public Camera GetCamera()
        => Camera;

    public Grabber GetGrabber()
        => Grabber;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "AnnotationManager",            thisObject => ((CanvasManager)thisObject).GetAnnotationManager() },
    };
    public object Get(string s) => Accessor[s](this);
    protected override void AwakeFunction()
    {
        base.AwakeFunction();

        _volume.profile.TryGet(out _vignette);
        
        _results = new();

        MergePreresultView.CheckAwake();
        AnnotationManager.CheckAwake();
        ConsolePanel.CheckAwake();
        ConsolePanel.gameObject.SetActive(!AppManager.Instance.AudienceIsPlayer());
        
        GuideView.SetAddress(new Address("Run.Environment.ActivePanel.Guide"));
        
        AppCanvas.Configure();
        Curtain.CheckAwake();
    }

    public void RefreshGuide()
    {
        GuideView.Refresh();
    }

    public AnnotationManager GetAnnotationManager()
        => AnnotationManager;

    public void CloseAnnotation(InteractBehaviour ib, PointerEventData d)
        => CloseAnnotation();

    public void CloseAnnotation()
    {
    }

    public void ShowDialog(string message, Action onConfirm)
    {
        DialogWindow.ShowDialog(message, onConfirm);
        DialogWindow.gameObject.SetActive(true);
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
        return GetFullPath(_results[0].gameObject);
    }

    private string GetFullPath(GameObject gao)
    {
        if (gao == null)
            return "null";
            
        string path = gao.name;
        Transform parent = gao.transform.parent;
        
        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }
        
        return path;
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
