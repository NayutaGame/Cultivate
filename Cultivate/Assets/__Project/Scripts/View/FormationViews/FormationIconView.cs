
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class FormationIconView : ItemView, IInteractable,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text CountText;

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

        CanvasManager.Instance.FormationPreview.SetAddress(view.GetAddress());
        CanvasManager.Instance.FormationPreview.Refresh();
    }

    public void PointerExit(IInteractable view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.FormationPreview.SetAddress(null);
        CanvasManager.Instance.FormationPreview.Refresh();
    }

    public void PointerMove(IInteractable view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.FormationPreview.UpdateMousePos(eventData.position);
    }
}
