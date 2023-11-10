
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : Singleton<CanvasManager>
{
    public AppCanvas AppCanvas;
    public RunCanvas RunCanvas;
    public StageCanvas StageCanvas;
    public Image Curtain;

    public SkillPreview SkillPreview;
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

    public Tween CurtainShow()
    {
        return DOTween.Sequence()
            .AppendCallback(() => Curtain.gameObject.SetActive(true))
            .Append(Curtain.DOFade(1, 0.3f).SetEase(Ease.OutQuad));
    }

    public Tween CurtainHide()
    {
        return DOTween.Sequence()
            .Append(Curtain.DOFade(0, 0.3f).SetEase(Ease.InQuad))
            .AppendCallback(() => Curtain.gameObject.SetActive(false));
    }
}
