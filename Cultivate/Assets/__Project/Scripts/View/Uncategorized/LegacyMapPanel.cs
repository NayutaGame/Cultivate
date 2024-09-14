
using DG.Tweening;
using UnityEngine;

public class LegacyMapPanel : Panel
{
    [SerializeField] private ListView NodeListView;
    [SerializeField] private RectTransform NodeListTransform;
    [SerializeField] private RectTransform NodeListIdlePivot;
    [SerializeField] private CanvasGroup CanvasGroup;

    private Address _address;

    public override void Configure()
    {
        base.Configure();
        // _address = new Address("Run.Environment.Map");
        // NodeListView.SetAddress(_address.Append(".StepItem.Nodes"));
    }

    public override void Refresh()
    {
        base.Refresh();
        // NodeListView.Refresh();
    }

    private void PlaySFX()
    {
    }

    // public override Tween ShowTween()
    // {
    //     return DOTween.Sequence()
    //         .AppendCallback(() => gameObject.SetActive(true))
    //         .AppendCallback(NodeListView.Sync)
    //         .AppendCallback(PlaySFX)
    //         .Append(TweenAnimation.Show(NodeListTransform, NodeListIdlePivot.anchoredPosition, CanvasGroup));
    // }
    //
    // public override Tween HideTween()
    // {
    //     return DOTween.Sequence()
    //         .AppendCallback(PlaySFX)
    //         .Append(TweenAnimation.Hide(NodeListTransform, NodeListIdlePivot.anchoredPosition, CanvasGroup))
    //         .AppendCallback(() => gameObject.SetActive(false));
    // }
}
