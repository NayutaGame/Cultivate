
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class FormationView : ItemView, IInteractable,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    protected RectTransform _rectTransform;

    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text JingJieText;
    [SerializeField] private TMP_Text ConditionText;
    [SerializeField] private TMP_Text RewardText;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        _rectTransform = GetComponent<RectTransform>();
    }

    public override void Refresh()
    {
        base.Refresh();
        FormationEntry e = Get<FormationEntry>();
        if (e == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        if (NameText != null)
            NameText.text = e.GetName();

        if (JingJieText != null)
            JingJieText.text = e.GetJingJie().ToString();

        if (ConditionText != null)
            ConditionText.text = e.GetConditionDescription();

        if (RewardText != null)
            RewardText.text = e.GetRewardDescription();
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
        RunCanvas.Instance.SetIndexPathForSubFormationPreview(view.GetAddress());
    }

    public void PointerExit(IInteractable view, PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.SetIndexPathForSubFormationPreview(null);
    }

    public void PointerMove(IInteractable view, PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.UpdateMousePosForSubFormationPreview(eventData.position);
    }
}
