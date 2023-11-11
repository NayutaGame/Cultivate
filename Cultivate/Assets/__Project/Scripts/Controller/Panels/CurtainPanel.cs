
using DG.Tweening;

public class CurtainPanel : Panel
{
    public override Tween ShowAnimation()
    {
        return DOTween.Sequence().SetAutoKill()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(CanvasManager.Instance.Curtain.HideAnimation());
    }

    public override Tween HideAnimation()
    {
        return DOTween.Sequence().SetAutoKill()
            .Append(CanvasManager.Instance.Curtain.ShowAnimation())
            .AppendCallback(() => gameObject.SetActive(false));
    }
}
