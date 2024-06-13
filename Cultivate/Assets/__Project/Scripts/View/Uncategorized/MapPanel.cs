
using DG.Tweening;
using UnityEngine;

public class MapPanel : Panel
{
    [SerializeField] private ListView NodeListView;
    [SerializeField] private RectTransform NodeListTransform;
    [SerializeField] private RectTransform NodeList1Pivot;
    [SerializeField] private RectTransform NodeList2Pivot;
    [SerializeField] private RectTransform NodeList3Pivot;
    [SerializeField] private CanvasGroup CanvasGroup;

    private Address _address;

    public override Tween ShowTween()
    {
        return DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .AppendCallback(NodeListView.Sync)
            .AppendCallback(PlaySFX)
            .Append(NodeListTransform.DOAnchorPos(NodeList2Pivot.anchoredPosition, 0.15f).From(NodeList1Pivot.anchoredPosition)).SetEase(Ease.OutQuad)
            .Join(CanvasGroup.DOFade(1f, 0.15f));
    }

    public override Tween HideTween()
    {
        return DOTween.Sequence()
            .AppendCallback(PlaySFX)
            .Append(NodeListTransform.DOAnchorPos(NodeList3Pivot.anchoredPosition, 0.15f).From(NodeList2Pivot.anchoredPosition)).SetEase(Ease.InQuad)
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
