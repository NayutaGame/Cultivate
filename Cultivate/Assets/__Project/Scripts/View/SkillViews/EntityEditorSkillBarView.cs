
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;

[SelectionBase]
public class EntityEditorSkillBarView : ItemView, IInteractable,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IPointerClickHandler,
    IBeginDragHandler, IEndDragHandler, IDragHandler,
    IDropHandler
{
    [SerializeField] private TMP_Text NameText;

    public override void Refresh()
    {
        base.Refresh();

        ISkillModel skill = Get<ISkillModel>();
        NameText.text = skill.GetName();
    }

    // [SerializeField] protected RectTransform _rectTransform;
    // [SerializeField] protected Image _image;
    // [SerializeField] protected CanvasGroup _canvasGroup;

    // [SerializeField] private TMP_Text AnnotationText;

    // [SerializeField] private Image CounterImage;
    // [SerializeField] private Image SelectionImage;
    //
    // private bool _selected;
    // public virtual bool IsSelected() => _selected;
    // public virtual void SetSelected(bool selected)
    // {
    //     _selected = selected;
    //     if (SelectionImage != null)
    //         SelectionImage.color = new Color(1, 1, 1, selected ? 1 : 0);
    // }
    //
    // public void ClearIsManaShortage() => IsManaShortageDelegate = null;
    // public event Func<bool> IsManaShortageDelegate;

    // public virtual void SetManaCostColor()
    // {
    //     if (ManaCostText == null)
    //         return;
    //
    //     Color color = Color.white;
    //
    //     if (IsManaShortageDelegate != null && IsManaShortageDelegate())
    //         color = Color.red;
    //
    //     ManaCostText.color = color;
    // }

    // public virtual void SetCounter(int currCounter, int maxCounter)
    // {
    //     if (CounterImage == null)
    //         return;
    //
    //     CounterImage.fillAmount = (float)currCounter / maxCounter;
    // }

    // public virtual void SetAnnotationText(string annotationText)
    // {
    //     if (AnnotationText == null)
    //         return;
    //
    //     AnnotationText.text = annotationText;
    // }

    // public virtual void SetColor(Color color)
    // {
    //     // _image.color = color;
    // }
    //
    // public virtual void SetCardFace(Sprite cardFace)
    // {
    //     // _image.sprite = cardFace;
    // }

    #region IInteractable

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;
    public void SetDelegate(InteractDelegate interactDelegate) => InteractDelegate = interactDelegate;

    public virtual void OnPointerEnter(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.POINTER_ENTER, this, eventData);
    public virtual void OnPointerExit(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.POINTER_EXIT, this, eventData);
    public virtual void OnPointerMove(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.POINTER_MOVE, this, eventData);
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        int? gestureId = null;

        if (eventData.button == PointerEventData.InputButton.Left) {
            gestureId = InteractDelegate.POINTER_LEFT_CLICK;
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            gestureId = InteractDelegate.POINTER_RIGHT_CLICK;
        }

        if (gestureId.HasValue)
            GetDelegate()?.Handle(gestureId.Value, this, eventData);
    }

    public virtual void OnBeginDrag(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.BEGIN_DRAG, this, eventData);
    public virtual void OnEndDrag(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.END_DRAG, this, eventData);
    public virtual void OnDrag(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.DRAG, this, eventData);
    public virtual void OnDrop(PointerEventData eventData) => GetDelegate()?.DragDrop(eventData.pointerDrag.GetComponent<IInteractable>(), this);

    #endregion
}
