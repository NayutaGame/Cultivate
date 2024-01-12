
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasManager : Singleton<CanvasManager>
{
    public AppCanvas AppCanvas;
    public RunCanvas RunCanvas;
    public StageCanvas StageCanvas;

    [Header("Annotations")]
    public SkillAnnotation SkillAnnotation;
    public BuffAnnotation BuffAnnotation;
    public FormationAnnotation FormationAnnotation;
    // public MechAnnotation MechAnnotation;
    public TextHint TextHint;

    [Header("Ghosts")]
    public GhostView SkillGhost;
    public MechView MechGhost;

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

    public override void DidAwake()
    {
        base.DidAwake();

        SkillAnnotation.Awake();
        BuffAnnotation.Awake();
        FormationAnnotation.Awake();
        SkillGhost.GetComponent<SimpleView>().Awake();
        MechGhost.Awake();
    }

    public void ClearAnnotations(InteractBehaviour ib, PointerEventData d)
    {
        SkillAnnotation.SetAddressToNull(ib, d);
        SkillAnnotation.SetAddressToNull(ib, d);
        BuffAnnotation.SetAddressToNull(ib, d);
        FormationAnnotation.SetAddressToNull(ib, d);
        // MechAnnotation.SetAddressToNull(ib, d);
        // TextHint.SetAddressToNull(ib, d);
    }
}
