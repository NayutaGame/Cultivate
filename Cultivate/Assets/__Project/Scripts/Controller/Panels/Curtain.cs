
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Curtain : Panel
{
    [SerializeField] private Image Image;

    public override Tween EnterIdle()
    {
        return DOTween.Sequence()
            .AppendCallback(() => Image.gameObject.SetActive(true))
            .Append(Image.DOFade(1, 0.3f).SetEase(Ease.OutQuad));
    }

    public override Tween EnterHide()
    {
        return DOTween.Sequence()
            .Append(Image.DOFade(0, 0.3f).SetEase(Ease.InQuad))
            .AppendCallback(() => Image.gameObject.SetActive(false));
    }
}
