
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LegacyInteractBehaviour : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IBeginDragHandler, IEndDragHandler, IDragHandler,
    IDropHandler,
    IPointerClickHandler
{
    private LegacyView _xView;
    public LegacyView GetCLView() => _xView;
    public LegacySimpleView GetSimpleView() => _xView.GetView();

    private Image Image;
    private CanvasGroup CanvasGroup;

    public void Init(LegacyView xView)
    {
        _xView = xView;

        Image ??= GetComponent<Image>();
        CanvasGroup ??= GetComponent<CanvasGroup>();

        // AppendDebugLog();
    }
    
    public void SetInteractable(bool value)
    {
        if (Image != null)
            Image.raycastTarget = value;

        if (CanvasGroup != null)
            CanvasGroup.interactable = value;
    }

    private void AppendDebugLog()
    {
        PointerEnterNeuron.Join(PointerEnterLog);
        PointerExitNeuron.Join(PointerExitLog);
        // PointerMoveNeuron.Join(PointerMoveLog);
        BeginDragNeuron.Join(BeginDragLog);
        EndDragNeuron.Join(EndDragLog);
        // DragNeuron.Join(DragLog);
        LeftClickNeuron.Join(LeftClickLog);
        RightClickNeuron.Join(RightClickLog);
        DroppingNeuron.Join(DroppingLog);
        DropNeuron.Join(DropLog);
        DraggingEnterNeuron.Join(DraggingEnterLog);
        DraggingExitNeuron.Join(DraggingExitLog);
        // DraggingMoveNeuron.Join(DraggingMoveLog);
    }
    
    private void PointerEnterLog(LegacyInteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetSimpleView().name} PointerEnter");
    private void PointerExitLog(LegacyInteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetSimpleView().name} PointerExit");
    private void PointerMoveLog(LegacyInteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetSimpleView().name} PointerMove");
    private void BeginDragLog(LegacyInteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetSimpleView().name} BeginDrag");
    private void EndDragLog(LegacyInteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetSimpleView().name} EndDrag");
    private void DragLog(LegacyInteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetSimpleView().name} Drag");
    private void LeftClickLog(LegacyInteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetSimpleView().name} LeftClick");
    private void RightClickLog(LegacyInteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetSimpleView().name} RightClick");
    private void DroppingLog(LegacyInteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetSimpleView().name} Dropping");
    private void DropLog(LegacyInteractBehaviour from, LegacyInteractBehaviour to, PointerEventData d) => Debug.Log($"{GetSimpleView().name} Drop");
    private void DraggingEnterLog(LegacyInteractBehaviour from, LegacyInteractBehaviour to, PointerEventData d) => Debug.Log($"{GetSimpleView().name} DraggingEnter");
    private void DraggingExitLog(LegacyInteractBehaviour from, LegacyInteractBehaviour to, PointerEventData d) => Debug.Log($"{GetSimpleView().name} DraggingExit");
    private void DraggingMoveLog(LegacyInteractBehaviour from, LegacyInteractBehaviour to, PointerEventData d) => Debug.Log($"{GetSimpleView().name} DraggingMove");

    public Neuron<LegacyInteractBehaviour, PointerEventData> PointerEnterNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> PointerExitNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> PointerMoveNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> BeginDragNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> EndDragNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> DragNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> LeftClickNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> RightClickNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> DroppingNeuron = new();
    public Neuron<LegacyInteractBehaviour, LegacyInteractBehaviour, PointerEventData> DropNeuron = new();

    public Neuron<LegacyInteractBehaviour, LegacyInteractBehaviour, PointerEventData> DraggingEnterNeuron = new();
    public Neuron<LegacyInteractBehaviour, LegacyInteractBehaviour, PointerEventData> DraggingExitNeuron = new();
    public Neuron<LegacyInteractBehaviour, LegacyInteractBehaviour, PointerEventData> DraggingMoveNeuron = new();

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            PointerEnterNeuron.Invoke(this, eventData);
            return;
        }
        
        if (eventData.pointerDrag == gameObject)
            return;

        LegacyInteractBehaviour dragging = eventData.pointerDrag.GetComponent<LegacyInteractBehaviour>();
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

        LegacyInteractBehaviour dragging = eventData.pointerDrag.GetComponent<LegacyInteractBehaviour>();
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

        LegacyInteractBehaviour dragging = eventData.pointerDrag.GetComponent<LegacyInteractBehaviour>();
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

        LegacyInteractBehaviour dragging = eventData.pointerDrag.GetComponent<LegacyInteractBehaviour>();
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
