
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : Singleton<CanvasManager>
{
    public AppCanvas AppCanvas;
    public RunCanvas RunCanvas;
    public StageCanvas StageCanvas;

    [Header("Annotations")]
    public SkillAnnotation SkillAnnotation;
    public BuffAnnotation BuffAnnotation;
    public FormationAnnotation FormationAnnotation;
    public TextHint TextHint;

    [Header("Ghosts")]
    public HandSkillView HandSkillGhost;
    public FieldSlotView FieldSlotGhost;
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
}
