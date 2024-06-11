
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasManager : Singleton<CanvasManager>, Addressable
{
    public AppCanvas AppCanvas;
    public RunCanvas RunCanvas;
    public StageCanvas StageCanvas;
    
    [SerializeField] private Camera Camera;
    [SerializeField] private GraphicRaycaster Raycaster;

    [Header("Annotations")]
    public AnnotationView SkillAnnotation;
    public AnnotationView BuffAnnotation;
    public AnnotationView FormationAnnotation;
    public TextHint TextHint;

    [Header("Ghosts")]
    public GhostView SkillGhost;
    public GhostView SlotGhost;

    [Header("Curtain")]
    public Curtain Curtain;

    [Header("Guide")]
    public GuideView GuideView;

    public Color[] JingJieColors;
    public Sprite[] JingJieSprites;
    [SerializeField] private Sprite[] WuXingSprites;

    public Color TechColorGreen;
    public Color TechColorYellow;
    public Color TechColorRed;

    public Color[] ManaCostColors;

    public Sprite GetWuXingSprite(WuXing? wuXing)
    {
        if (wuXing == null)
            return null;

        return WuXingSprites[wuXing.Value._index];
    }

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public override void DidAwake()
    {
        base.DidAwake();

        _accessors = new Dictionary<string, Func<object>>()
        {
            { "SkillAnnotation", () => SkillAnnotation },
            { "BuffAnnotation", () => BuffAnnotation },
            { "FormationAnnotation", () => FormationAnnotation },
            { "SkillGhost", () => SkillGhost },
            { "SlotGhost", () => SlotGhost },
        };

        _results = new();

        SkillAnnotation.Awake();
        BuffAnnotation.Awake();
        FormationAnnotation.Awake();
        SkillGhost.Awake();
        SlotGhost.Awake();
        
        GuideView.SetAddress(new Address("Run.Environment.ActivePanel.Guide"));
        
        AppCanvas.Configure();
        Curtain.Configure();
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
        return _results.Count >= 1 && _results[0].gameObject.GetComponent<InteractBehaviour>() != null;
    }

    public Vector3 UI2World(Vector2 mousePosition)
    {
        return Camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10));
    }
}
