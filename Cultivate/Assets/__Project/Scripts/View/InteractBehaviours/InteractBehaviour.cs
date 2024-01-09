
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractBehaviour : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IBeginDragHandler, IEndDragHandler, IDragHandler,
    IDropHandler,
    IPointerClickHandler
{
    private AddressBehaviour AddressBehaviour;

    public Neuron<AddressBehaviour, PointerEventData> PointerEnterNeuron = new();
    public Neuron<AddressBehaviour, PointerEventData> PointerExitNeuron = new();
    public Neuron<AddressBehaviour, PointerEventData> PointerMoveNeuron = new();
    public Neuron<AddressBehaviour, PointerEventData> BeginDragNeuron = new();
    public Neuron<AddressBehaviour, PointerEventData> EndDragNeuron = new();
    public Neuron<AddressBehaviour, PointerEventData> DragNeuron = new();
    public Neuron<AddressBehaviour, PointerEventData> LeftClickNeuron = new();
    public Neuron<AddressBehaviour, PointerEventData> RightClickNeuron = new();
    public Neuron<AddressBehaviour, AddressBehaviour, PointerEventData> DropNeuron = new();

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.dragging)
            PointerEnterNeuron.Invoke(AddressBehaviour, eventData);
    }

    public virtual void OnPointerExit (PointerEventData eventData)
    {
        if (!eventData.dragging)
            PointerExitNeuron.Invoke(AddressBehaviour, eventData);
    }

    public virtual void OnPointerMove (PointerEventData eventData)
    {
        if (!eventData.dragging)
            PointerMoveNeuron.Invoke(AddressBehaviour, eventData);
    }

    public virtual void OnBeginDrag   (PointerEventData eventData) => BeginDragNeuron   .Invoke(AddressBehaviour, eventData);
    public virtual void OnEndDrag     (PointerEventData eventData) => EndDragNeuron     .Invoke(AddressBehaviour, eventData);
    public virtual void OnDrag        (PointerEventData eventData) => DragNeuron        .Invoke(AddressBehaviour, eventData);
    public virtual void OnDrop        (PointerEventData eventData)
    {
        if (eventData.pointerDrag == gameObject)
            return;

        AddressBehaviour dragged = eventData.pointerDrag.GetComponent<InteractBehaviour>().AddressBehaviour;

        if (dragged != null)
            DropNeuron.Invoke(dragged, AddressBehaviour, eventData);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) {
            LeftClickNeuron.Invoke(AddressBehaviour, eventData);
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            RightClickNeuron.Invoke(AddressBehaviour, eventData);
        }
    }
}
