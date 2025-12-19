
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractBehaviour : MonoBehaviour
{
    private XView _view;
    public XView GetView() => _view;
    public void SetView(XView view) => _view = view;

    private bool _isAwoken;

    public InteractNeuronBundle NeuronBundle = new();

    public void CheckAwake()
    {
        if (_isAwoken)
            return;

        _isAwoken = true;
        AwakeFunction();
    }
    
    public void AwakeFunction()
    {
        // AppendDebugLog();
    }
    
    public T Get<T>() where T : class => _view.Get<T>();
    public virtual Address GetAddress() => _view.GetAddress();
    
    public void SetInteractable(bool value)
    {
        NeuronBundle.PointerEnterNeuron.Active = value;
        NeuronBundle.PointerExitNeuron.Active = value;
        NeuronBundle.PointerMoveNeuron.Active = value;
        NeuronBundle.BeginDragNeuron.Active = value;
        NeuronBundle.EndDragNeuron.Active = value;
        NeuronBundle.DragNeuron.Active = value;
        NeuronBundle.LeftClickNeuron.Active = value;
        NeuronBundle.RightClickNeuron.Active = value;
        NeuronBundle.DroppingNeuron.Active = value;
        NeuronBundle.DropNeuron.Active = value;
        NeuronBundle.DraggingEnterNeuron.Active = value;
        NeuronBundle.DraggingExitNeuron.Active = value;
        NeuronBundle.DraggingMoveNeuron.Active = value;
        NeuronBundle.PointerDownNeuron.Active = value;
        NeuronBundle.PointerUpNeuron.Active = value;
    }

    private void AppendDebugLog()
    {
        NeuronBundle.PointerEnterNeuron.Join(PointerEnterLog);
        NeuronBundle.PointerExitNeuron.Join(PointerExitLog);
        // NeuronBundle.PointerMoveNeuron.Join(PointerMoveLog);
        NeuronBundle.BeginDragNeuron.Join(BeginDragLog);
        NeuronBundle.EndDragNeuron.Join(EndDragLog);
        // NeuronBundle.DragNeuron.Join(DragLog);
        NeuronBundle.LeftClickNeuron.Join(LeftClickLog);
        NeuronBundle.RightClickNeuron.Join(RightClickLog);
        NeuronBundle.DroppingNeuron.Join(DroppingLog);
        NeuronBundle.DropNeuron.Join(DropLog);
        NeuronBundle.DraggingEnterNeuron.Join(DraggingEnterLog);
        NeuronBundle.DraggingExitNeuron.Join(DraggingExitLog);
        // NeuronBundle.DraggingMoveNeuron.Join(DraggingMoveLog);
        NeuronBundle.PointerDownNeuron.Join(PointerDownLog);
        NeuronBundle.PointerUpNeuron.Join(PointerUpLog);
    }
    
    private void PointerEnterLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetView().name} PointerEnter");
    private void PointerExitLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetView().name} PointerExit");
    private void PointerMoveLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetView().name} PointerMove");
    private void BeginDragLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetView().name} BeginDrag");
    private void EndDragLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetView().name} EndDrag");
    private void DragLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetView().name} Drag");
    private void LeftClickLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetView().name} LeftClick");
    private void RightClickLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetView().name} RightClick");
    private void DroppingLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetView().name} Dropping");
    private void DropLog(InteractBehaviour from, InteractBehaviour to, PointerEventData d) => Debug.Log($"{GetView().name} Drop");
    private void DraggingEnterLog(InteractBehaviour from, InteractBehaviour to, PointerEventData d) => Debug.Log($"{GetView().name} DraggingEnter");
    private void DraggingExitLog(InteractBehaviour from, InteractBehaviour to, PointerEventData d) => Debug.Log($"{GetView().name} DraggingExit");
    private void DraggingMoveLog(InteractBehaviour from, InteractBehaviour to, PointerEventData d) => Debug.Log($"{GetView().name} DraggingMove");
    private void PointerDownLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetView().name} PointerDown");
    private void PointerUpLog(InteractBehaviour ib, PointerEventData d) => Debug.Log($"{GetView().name} PointerUp");

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            NeuronBundle.PointerEnterNeuron.Invoke(this, eventData);
            return;
        }
        
        if (eventData.pointerDrag == gameObject)
            return;

        InteractBehaviour dragging = eventData.pointerDrag.GetComponent<InteractBehaviour>();
        if (dragging == null)
            return;

        NeuronBundle.DraggingEnterNeuron.Invoke(dragging, this, eventData);
    }

    public virtual void OnPointerExit (PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            NeuronBundle.PointerExitNeuron.Invoke(this, eventData);
            return;
        }

        // if (eventData.pointerDrag == gameObject)
        //     return;

        InteractBehaviour dragging = eventData.pointerDrag.GetComponent<InteractBehaviour>();
        if (dragging == null)
            return;

        NeuronBundle.DraggingExitNeuron.Invoke(dragging, this, eventData);
    }

    public virtual void OnPointerMove (PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            NeuronBundle.PointerMoveNeuron.Invoke(this, eventData);
            return;
        }
        
        if (eventData.pointerDrag == gameObject)
            return;

        InteractBehaviour dragging = eventData.pointerDrag.GetComponent<InteractBehaviour>();
        if (dragging == null)
            return;

        NeuronBundle.DraggingMoveNeuron.Invoke(dragging, this, eventData);
    }

    public virtual void OnBeginDrag   (PointerEventData eventData) => NeuronBundle.BeginDragNeuron   .Invoke(this, eventData);
    public virtual void OnEndDrag     (PointerEventData eventData) => NeuronBundle.EndDragNeuron     .Invoke(this, eventData);
    public virtual void OnDrag        (PointerEventData eventData) => NeuronBundle.DragNeuron        .Invoke(this, eventData);
    public virtual void OnDrop        (PointerEventData eventData)
    {
        if (eventData.pointerDrag == gameObject)
            return;

        InteractBehaviour dragging = eventData.pointerDrag.GetComponent<InteractBehaviour>();
        if (dragging == null)
            return;

        dragging.NeuronBundle.DroppingNeuron.Invoke(dragging, eventData);
        NeuronBundle.DropNeuron.Invoke(dragging, this, eventData);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            NeuronBundle.LeftClickNeuron.Invoke(this, eventData);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            NeuronBundle.RightClickNeuron.Invoke(this, eventData);
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        NeuronBundle.PointerDownNeuron.Invoke(this, eventData);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        NeuronBundle.PointerUpNeuron.Invoke(this, eventData);
    }
}
