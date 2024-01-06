
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
    public ComplexView ComplexView;

    // [SerializeField] private Image Image;
    //
    // public void SetInteractable(bool interactable)
    // {
    //     Image.raycastTarget = interactable;
    // }

    private void OnEnable()
    {
        SetInteractable(true);

        PointerEnterNeuron.Add(ComplexView.HoverNeuron);
        PointerExitNeuron.Add(ComplexView.UnhoverNeuron);

        BeginDragNeuron.Add(ComplexView.BeginDragNeuron);
        EndDragNeuron.Add(ComplexView.EndDragNeuron);
        DragNeuron.Add(ComplexView.DragNeuron);
    }

    private void OnDisable()
    {
        PointerEnterNeuron.Remove(ComplexView.HoverNeuron);
        PointerExitNeuron.Remove(ComplexView.UnhoverNeuron);

        BeginDragNeuron.Remove(ComplexView.BeginDragNeuron);
        EndDragNeuron.Remove(ComplexView.EndDragNeuron);
        DragNeuron.Remove(ComplexView.DragNeuron);
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

        if (dragged != null)
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
