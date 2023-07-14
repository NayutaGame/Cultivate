
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageSkillView : SkillView
{
    public override ISkillModel GetSkillModel()
        => DataManager.Get<ISkillModel>(GetIndexPath());

    public Tween GetExpandTween()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(_rectTransform.DOScale(1, 0.6f).SetEase(Ease.InOutQuad));
        return seq;
    }

    public Tween GetShrinkTween()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(_rectTransform.DOScale(0.5f, 0.6f).SetEase(Ease.InOutQuad));
        return seq;
    }

    public override void OnPointerClick(PointerEventData eventData) { }
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnDrop(PointerEventData eventData) { }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        StageCanvas.Instance.SetIndexPathForPreview(GetIndexPath());
        StageManager.Instance.Pause();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        StageCanvas.Instance.SetIndexPathForPreview(null);
        StageManager.Instance.Resume();
    }

    public override void OnPointerMove(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        StageCanvas.Instance.UpdateMousePosForPreview(eventData.position);
    }
}
