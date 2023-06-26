
using CLLibrary;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : Singleton<CanvasManager>
{
    public RunCanvas RunCanvas;
    public StageCanvas StageCanvas;

    // public RectTransform GhostHolder;

    public Color[] JingJieColors;
    public Sprite[] JingJieSprites;
    [SerializeField] private Sprite[] WuXingSprites;

    public Sprite GetWuXingSprite(WuXing? wuXing)
    {
        if (wuXing == null)
            return null;

        return WuXingSprites[wuXing.Value._index];
    }


    public Sprite[] CardFaces;
}
