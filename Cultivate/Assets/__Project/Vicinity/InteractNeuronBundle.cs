
using CLLibrary;
using UnityEngine.EventSystems;

public class InteractNeuronBundle
{
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
    public Neuron<InteractBehaviour, PointerEventData> PointerDownNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerUpNeuron = new();
    
    public void Add(InteractNeuronBundle neuronBundle)
    {
        PointerEnterNeuron.Add(neuronBundle.PointerEnterNeuron);
        PointerExitNeuron.Add(neuronBundle.PointerExitNeuron);
        PointerMoveNeuron.Add(neuronBundle.PointerMoveNeuron);
        BeginDragNeuron.Add(neuronBundle.BeginDragNeuron);
        EndDragNeuron.Add(neuronBundle.EndDragNeuron);
        DragNeuron.Add(neuronBundle.DragNeuron);
        LeftClickNeuron.Add(neuronBundle.LeftClickNeuron);
        RightClickNeuron.Add(neuronBundle.RightClickNeuron);
        DroppingNeuron.Add(neuronBundle.DroppingNeuron);
        DropNeuron.Add(neuronBundle.DropNeuron);
        DraggingEnterNeuron.Add(neuronBundle.DraggingEnterNeuron);
        DraggingExitNeuron.Add(neuronBundle.DraggingExitNeuron);
        DraggingMoveNeuron.Add(neuronBundle.DraggingMoveNeuron);
        PointerDownNeuron.Add(neuronBundle.PointerDownNeuron);
        PointerUpNeuron.Add(neuronBundle.PointerUpNeuron);
    }
    
    public void Join(InteractNeuronBundle neuronBundle)
    {
        PointerEnterNeuron.Join(neuronBundle.PointerEnterNeuron);
        PointerExitNeuron.Join(neuronBundle.PointerExitNeuron);
        PointerMoveNeuron.Join(neuronBundle.PointerMoveNeuron);
        BeginDragNeuron.Join(neuronBundle.BeginDragNeuron);
        EndDragNeuron.Join(neuronBundle.EndDragNeuron);
        DragNeuron.Join(neuronBundle.DragNeuron);
        LeftClickNeuron.Join(neuronBundle.LeftClickNeuron);
        RightClickNeuron.Join(neuronBundle.RightClickNeuron);
        DroppingNeuron.Join(neuronBundle.DroppingNeuron);
        DropNeuron.Join(neuronBundle.DropNeuron);
        DraggingEnterNeuron.Join(neuronBundle.DraggingEnterNeuron);
        DraggingExitNeuron.Join(neuronBundle.DraggingExitNeuron);
        DraggingMoveNeuron.Join(neuronBundle.DraggingMoveNeuron);
        PointerDownNeuron.Join(neuronBundle.PointerDownNeuron);
        PointerUpNeuron.Join(neuronBundle.PointerUpNeuron);
    }
    
    public void Remove(InteractNeuronBundle neuronBundle)
    {
        PointerEnterNeuron.Remove(neuronBundle.PointerEnterNeuron);
        PointerExitNeuron.Remove(neuronBundle.PointerExitNeuron);
        PointerMoveNeuron.Remove(neuronBundle.PointerMoveNeuron);
        BeginDragNeuron.Remove(neuronBundle.BeginDragNeuron);
        EndDragNeuron.Remove(neuronBundle.EndDragNeuron);
        DragNeuron.Remove(neuronBundle.DragNeuron);
        LeftClickNeuron.Remove(neuronBundle.LeftClickNeuron);
        RightClickNeuron.Remove(neuronBundle.RightClickNeuron);
        DroppingNeuron.Remove(neuronBundle.DroppingNeuron);
        DropNeuron.Remove(neuronBundle.DropNeuron);
        DraggingEnterNeuron.Remove(neuronBundle.DraggingEnterNeuron);
        DraggingExitNeuron.Remove(neuronBundle.DraggingExitNeuron);
        DraggingMoveNeuron.Remove(neuronBundle.DraggingMoveNeuron);
        PointerDownNeuron.Remove(neuronBundle.PointerDownNeuron);
        PointerUpNeuron.Remove(neuronBundle.PointerUpNeuron);
    }
}
