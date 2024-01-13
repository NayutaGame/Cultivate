
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class CanvasManager : Singleton<CanvasManager>, Addressable
{
    public AppCanvas AppCanvas;
    public RunCanvas RunCanvas;
    public StageCanvas StageCanvas;

    [Header("Annotations")]
    public AnnotationView SkillAnnotation;
    public AnnotationView BuffAnnotation;
    public AnnotationView FormationAnnotation;
    // public AnnotationView MechAnnotation;
    public TextHint TextHint;

    [Header("Ghosts")]
    public GhostView SkillGhost;
    public GhostView MechGhost;

    [Header("Curtain")]
    public Curtain Curtain;

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

    public Sprite[] CardFaces;

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
            { "MechGhost", () => MechGhost },
        };

        SkillAnnotation.Awake();
        BuffAnnotation.Awake();
        FormationAnnotation.Awake();
        SkillGhost.Awake();
        MechGhost.Awake();
    }
}
