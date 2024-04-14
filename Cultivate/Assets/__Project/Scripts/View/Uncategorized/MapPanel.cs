
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MapPanel : Panel
{
    [SerializeField] private ListView NodeListView;
    [SerializeField] private CanvasGroup CanvasGroup;

    private Address _address;

    public override Tween ShowAnimation()
    {
        return DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .AppendCallback(Refresh)
            .AppendCallback(PlaySFX)
            .Append(RectTransform.DOScale(1f, 0.15f).SetEase(Ease.OutQuad))
            .Join(CanvasGroup.DOFade(1f, 0.15f));
    }

    public override Tween HideAnimation()
    {
        return DOTween.Sequence()
            .AppendCallback(PlaySFX)
            .Append(RectTransform.DOScale(1.2f, 0.15f).SetEase(Ease.OutQuad))
            .Join(CanvasGroup.DOFade(0f, 0.15f))
            .AppendCallback(() => gameObject.SetActive(false));
    }

    public override void Configure()
    {
        base.Configure();
        _address = new Address("Run.Environment.Map");
        NodeListView.SetAddress(_address.Append(".StepItem.Nodes"));
    }

    public override void Refresh()
    {
        base.Refresh();
        NodeListView.Refresh();
    }

    private void PlaySFX()
    {
    }
}
