
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComplexView : MonoBehaviour
{
    #region Address

    [SerializeField] public AddressBehaviour AddressBehaviour;

    // state
    // private Address _address;

    // get
    public Address GetAddress() => AddressBehaviour.GetAddress();
    public T Get<T>() => AddressBehaviour.Get<T>();
    public void Refresh() => AddressBehaviour.Refresh();

    // set
    public void SetAddress(Address address) => AddressBehaviour.SetAddress(address);

    #endregion

    #region Address Rect Transform

    // SetParent
    // SetName
    // SetSiblingIndex
    // gao.SetActive

    public RectTransform GetDisplayTransform() => AddressBehaviour.Base;

    public void SetDisplayTransform(RectTransform pivot)
    {
        AddressBehaviour.Base.position = pivot.position;
        AddressBehaviour.Base.localScale = pivot.localScale;
    }

    #endregion



    [SerializeField] private ItemBehaviour ItemBehaviour;
    [SerializeField] private InteractBehaviour InteractBehaviour;
    [SerializeField] private AnimateBehaviour AnimateBehaviour;
    [SerializeField] private PivotBehaviour PivotBehaviour;
    [SerializeField] private SelectBehaviour SelectBehaviour;

    public ItemBehaviour GetItemBehaviour() => ItemBehaviour;
    public InteractBehaviour GetInteractBehaviour() => InteractBehaviour;
    public AnimateBehaviour GetAnimateBehaviour() => AnimateBehaviour;
    public SelectBehaviour GetSelectBehaviour() => SelectBehaviour;
    public PivotBehaviour GetPivotBehaviour() => PivotBehaviour;

    public void RefreshPivots()
        => AnimateBehaviour.AnimateToIdle();

    private bool _visible;
    public bool IsVisible() => _visible;
    public void SetVisible(bool visible)
    {
        _visible = visible;

        // AddressBehaviour.SetVisible(_visible);
    }

    public void SetVisibleToTrue(InteractBehaviour ib, PointerEventData eventData)
        => SetVisible(true);

    public void SetVisibleToFalse(InteractBehaviour ib, PointerEventData eventData)
        => SetVisible(false);

    private bool _interactable;
    public virtual bool IsInteractable() => _interactable;
    public virtual void SetInteractable(bool interactable)
    {
        _interactable = interactable;

        // if (InteractBehaviour != null)
        //     InteractBehaviour.SetInteractable(_interactable);
    }

    public void SetInteractableToTrue(InteractBehaviour ib, PointerEventData eventData)
        => SetInteractable(true);

    public void SetInteractableToFalse(InteractBehaviour ib, PointerEventData eventData)
        => SetInteractable(false);

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
