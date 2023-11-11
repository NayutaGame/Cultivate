
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : Singleton<CanvasManager>
{
    public AppCanvas AppCanvas;
    public RunCanvas RunCanvas;
    public StageCanvas StageCanvas;

    public Curtain Curtain;

    public SkillAnnotation SkillAnnotation;
    public FormationPreview FormationPreview;
    public MechGhost MechGhost;

    public Color[] JingJieColors;
    public Sprite[] JingJieSprites;
    [SerializeField] private Sprite[] WuXingSprites;

    public Color TechColorGreen;
    public Color TechColorYellow;
    public Color TechColorRed;

    public Sprite GetWuXingSprite(WuXing? wuXing)
    {
        if (wuXing == null)
            return null;

        return WuXingSprites[wuXing.Value._index];
    }

    public Sprite[] CardFaces;
}
