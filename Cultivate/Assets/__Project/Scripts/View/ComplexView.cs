
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComplexView : MonoBehaviour
{
    [SerializeField] public AddressBehaviour AddressBehaviour;
    [SerializeField] protected ItemBehaviour ItemBehaviour;
    [SerializeField] protected InteractBehaviour InteractBehaviour;
    [SerializeField] protected AnimateBehaviour AnimateBehaviour;
    [SerializeField] protected PivotBehaviour PivotBehaviour;
    [SerializeField] protected SelectBehaviour SelectBehaviour;

    public ItemBehaviour GetItemBehaviour() => ItemBehaviour;
    public InteractBehaviour GetInteractBehaviour() => InteractBehaviour;
    public AnimateBehaviour GetAnimateBehaviour() => AnimateBehaviour;
    public SelectBehaviour GetSelectBehaviour() => SelectBehaviour;
    public RectTransform GetDisplayTransform() => AddressBehaviour.RectTransform;

    public void SetDisplayTransform(RectTransform pivot)
    {
        AddressBehaviour.RectTransform.position = pivot.position;
        AddressBehaviour.RectTransform.localScale = pivot.localScale;
    }

    public void RefreshPivots()
        => AnimateBehaviour.AnimateToIdle();

    public Neuron<InteractBehaviour, PointerEventData> HoverNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> UnhoverNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerMoveNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> BeginDragNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> EndDragNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> DragNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> LeftClickNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> RightClickNeuron = new();
    public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DropNeuron = new();
}
