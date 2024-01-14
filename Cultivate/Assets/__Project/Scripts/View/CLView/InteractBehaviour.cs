
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractBehaviour : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IBeginDragHandler, IEndDragHandler, IDragHandler,
    IDropHandler,
    IPointerClickHandler
{
    private CLView CLView;
    public CLView GetCLView() => CLView;
    public SimpleView GetSimpleView() => CLView.GetSimpleView();

    public void Init(CLView clView)
    {
        CLView = clView;

        Image ??= GetComponent<Image>();
        CanvasGroup ??= GetComponent<CanvasGroup>();
    }

    private Image Image;
    private CanvasGroup CanvasGroup;

    public void SetInteractable(bool value)
    {
        if (Image != null)
            Image.raycastTarget = value;

        if (CanvasGroup != null)
            CanvasGroup.interactable = value;
    }

    public Neuron<InteractBehaviour, PointerEventData> PointerEnterNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerExitNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerMoveNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> BeginDragNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> EndDragNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> DragNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> LeftClickNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> RightClickNeuron = new();
    public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DropNeuron = new();

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.dragging)
            PointerEnterNeuron.Invoke(this, eventData);
    }

    public virtual void OnPointerExit (PointerEventData eventData)
    {
        if (!eventData.dragging)
            PointerExitNeuron.Invoke(this, eventData);
    }

    public virtual void OnPointerMove (PointerEventData eventData)
    {
        if (!eventData.dragging)
            PointerMoveNeuron.Invoke(this, eventData);
    }

    public virtual void OnBeginDrag   (PointerEventData eventData) => BeginDragNeuron   .Invoke(this, eventData);
    public virtual void OnEndDrag     (PointerEventData eventData) => EndDragNeuron     .Invoke(this, eventData);
    public virtual void OnDrag        (PointerEventData eventData) => DragNeuron        .Invoke(this, eventData);
    public virtual void OnDrop        (PointerEventData eventData)
    {
        if (eventData.pointerDrag == gameObject)
            return;

        InteractBehaviour dragged = eventData.pointerDrag.GetComponent<InteractBehaviour>();
        if (dragged == null)
            return;

        DropNeuron.Invoke(dragged, this, eventData);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) {
            LeftClickNeuron.Invoke(this, eventData);
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            RightClickNeuron.Invoke(this, eventData);
        }
    }
}
