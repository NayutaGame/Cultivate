
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSkillView : SkillView, IInteractable,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public RectTransform _rectTransform;

    [SerializeField] private Image CounterImage;

    public override void Refresh()
    {
        base.Refresh();

        if (!gameObject.activeSelf)
            return;

        ISkillModel skill = Get<ISkillModel>();
        SetCounter(skill.GetCurrCounter(), skill.GetMaxCounter());
    }

    private void SetCounter(int currCounter, int maxCounter)
    {
        CounterImage.fillAmount = (float)currCounter / maxCounter;
    }

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

    #region IInteractable

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;
    public void SetDelegate(InteractDelegate interactDelegate) => InteractDelegate = interactDelegate;

    public virtual void OnPointerEnter(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.POINTER_ENTER, this, eventData);
    public virtual void OnPointerExit(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.POINTER_EXIT, this, eventData);
    public virtual void OnPointerMove(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.POINTER_MOVE, this, eventData);

    #endregion

    public void PointerEnter(IInteractable view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        StageCanvas.Instance.SkillPreview.SetAddress(GetAddress());
        StageCanvas.Instance.SkillPreview.Refresh();
        StageManager.Instance.Pause();
    }

    public void PointerExit(IInteractable view, PointerEventData eventData)
    {
        if (eventData.dragging) return;
        StageCanvas.Instance.SkillPreview.SetAddress(null);
        StageCanvas.Instance.SkillPreview.Refresh();
        StageManager.Instance.Resume();
    }

    public void PointerMove(IInteractable view, PointerEventData eventData)
    {
        if (eventData.dragging) return;
        StageCanvas.Instance.SkillPreview.UpdateMousePos(eventData.position);
    }
}
