
using DG.Tweening;

public class CurtainPanel : Panel
{
    public override Tween ShowAnimation()
    {
        return DOTween.Sequence().SetAutoKill()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(CanvasManager.Instance.CurtainHide());
    }

    public override Tween HideAnimation()
    {
        return DOTween.Sequence().SetAutoKill()
            .AppendCallback(() => gameObject.SetActive(false))
            .Append(CanvasManager.Instance.CurtainShow());
    }
}
