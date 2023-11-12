
using TMPro;
using UnityEngine.EventSystems;

public class BuffView : ItemView, IInteractable,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public TMP_Text NameText;
    public TMP_Text StackText;

    public override void Refresh()
    {
        Buff b = Get<Buff>();

        gameObject.SetActive(b != null);
        if (b == null) return;

        NameText.text = $"{b.GetName()}";
        StackText.text = $"{b.Stack}";
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

        CanvasManager.Instance.BuffAnnotation.SetAddress(GetAddress());
        CanvasManager.Instance.BuffAnnotation.Refresh();
        StageManager.Instance.Pause();
    }

    public void PointerExit(IInteractable view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.BuffAnnotation.SetAddress(null);
        CanvasManager.Instance.BuffAnnotation.Refresh();
        StageManager.Instance.Resume();
    }

    public void PointerMove(IInteractable view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.BuffAnnotation.UpdateMousePos(eventData.position);
    }
}
