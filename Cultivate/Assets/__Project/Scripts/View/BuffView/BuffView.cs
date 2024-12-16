
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffView : XView
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text StackText;
    [SerializeField] private Image IconImage;

    public override void Refresh()
    {
        base.Refresh();

        Buff b = Get<Buff>();
        StackText.text = b.Stack.ToString();
        IconImage.sprite = b.GetEntry().GetSprite();

        NameText.text = IconImage.sprite == Encyclopedia.SpriteCategory.MissingBuffIcon().Sprite ? b.GetName() : "";
    }

    private void PingAnimation()
    {
        // LegacyPivotBehaviour pivotBehaviour = GetComponent<LegacyPivotBehaviour>();
        // if (pivotBehaviour != null)
        //     pivotBehaviour.PlayPingAnimation();
    }
    
    // private Tween PingTween()
    //     => GetDisplayTransform().DOScale(1.5f, 0.075f).SetEase(Ease.OutQuad).SetLoops(2, loopType: LoopType.Yoyo);
}
