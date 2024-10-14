
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

        // AppendDebugLog();
    }

    private void AppendDebugLog()
    {
        PointerEnterNeuron.Join(PointerEnterLog);
        PointerExitNeuron.Join(PointerExitLog);
        PointerMoveNeuron.Join(PointerMoveLog);
        BeginDragNeuron.Join(BeginDragLog);
        EndDragNeuron.Join(EndDragLog);
        DragNeuron.Join(DragLog);
        LeftClickNeuron.Join(LeftClickLog);
        RightClickNeuron.Join(RightClickLog);
        DroppingNeuron.Join(DroppingLog);
        DropNeuron.Join(DropLog);
        DraggingEnterNeuron.Join(DraggingEnterLog);
        DraggingExitNeuron.Join(DraggingExitLog);
        DraggingMoveNeuron.Join(DraggingMoveLog);
    }
    
    private void PointerEnterLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"PointerEnter");
    private void PointerExitLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"PointerExit");
    private void PointerMoveLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"PointerMove");
    private void BeginDragLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"BeginDrag");
    private void EndDragLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"EndDrag");
    private void DragLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"Drag");
    private void LeftClickLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"LeftClick");
    private void RightClickLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"RightClick");
    private void DroppingLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"Dropping");
    private void DropLog(InteractBehaviour from, InteractBehaviour to, PointerEventData d) => Debug.Log($"Drop");
    private void DraggingEnterLog(InteractBehaviour from, InteractBehaviour to, PointerEventData d) => Debug.Log($"DraggingEnter");
    private void DraggingExitLog(InteractBehaviour from, InteractBehaviour to, PointerEventData d) => Debug.Log($"DraggingExit");
    private void DraggingMoveLog(InteractBehaviour from, InteractBehaviour to, PointerEventData d) => Debug.Log($"DraggingMove");

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
    public Neuron<InteractBehaviour, PointerEventData> DroppingNeuron = new();
    public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DropNeuron = new();

    public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DraggingEnterNeuron = new();
    public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DraggingExitNeuron = new();
    public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DraggingMoveNeuron = new();

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            PointerEnterNeuron.Invoke(this, eventData);
            return;
        }
        
        if (eventData.pointerDrag == gameObject)
            return;

        InteractBehaviour dragging = eventData.pointerDrag.GetComponent<InteractBehaviour>();
        if (dragging == null)
            return;

        DraggingEnterNeuron.Invoke(dragging, this, eventData);
    }

    public virtual void OnPointerExit (PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            PointerExitNeuron.Invoke(this, eventData);
            return;
        }
        
        if (eventData.pointerDrag == gameObject)
            return;

        InteractBehaviour dragging = eventData.pointerDrag.GetComponent<InteractBehaviour>();
        if (dragging == null)
            return;

        DraggingExitNeuron.Invoke(dragging, this, eventData);
    }

    public virtual void OnPointerMove (PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            PointerMoveNeuron.Invoke(this, eventData);
            return;
        }
        
        if (eventData.pointerDrag == gameObject)
            return;

        InteractBehaviour dragging = eventData.pointerDrag.GetComponent<InteractBehaviour>();
        if (dragging == null)
            return;

        DraggingMoveNeuron.Invoke(dragging, this, eventData);
    }

    public virtual void OnBeginDrag   (PointerEventData eventData) => BeginDragNeuron   .Invoke(this, eventData);
    public virtual void OnEndDrag     (PointerEventData eventData) => EndDragNeuron     .Invoke(this, eventData);
    public virtual void OnDrag        (PointerEventData eventData) => DragNeuron        .Invoke(this, eventData);
    public virtual void OnDrop        (PointerEventData eventData)
    {
        if (eventData.pointerDrag == gameObject)
            return;

        InteractBehaviour dragging = eventData.pointerDrag.GetComponent<InteractBehaviour>();
        if (dragging == null)
            return;

        dragging.DroppingNeuron.Invoke(dragging, eventData);
        DropNeuron.Invoke(dragging, this, eventData);
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
