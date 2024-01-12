
using UnityEngine;

public class ComplexView : MonoBehaviour, CLView
{
    [SerializeField] public SimpleView SimpleView;
    public SimpleView GetSimpleView() => SimpleView.GetSimpleView();

    public RectTransform GetDisplayTransform() => SimpleView.GetDisplayTransform();

    private InteractBehaviour InteractBehaviour;
    public InteractBehaviour GetInteractBehaviour() => InteractBehaviour;

    private ItemBehaviour ItemBehaviour;
    public ItemBehaviour GetItemBehaviour() => ItemBehaviour;

    private SelectBehaviour SelectBehaviour;
    public SelectBehaviour GetSelectBehaviour() => SelectBehaviour;

    private StateBehaviour StateBehaviour;
    public StateBehaviour GetStateBehaviour() => StateBehaviour;

    public void SetBaseTransform(RectTransform pivot)
    {
        SimpleView.GetDisplayTransform().position = pivot.position;
        SimpleView.GetDisplayTransform().localScale = pivot.localScale;
    }

    public Address GetAddress() => SimpleView.GetAddress();
    public T Get<T>() => SimpleView.Get<T>();
    public void SetAddress(Address address) => SimpleView.SetAddress(address);
    public void Refresh() => SimpleView.Refresh();

    public void Awake()
    {
        if (InteractBehaviour != null)
            InteractBehaviour.Init(this);

        if (ItemBehaviour != null)
            ItemBehaviour.Init(this);

        if (SelectBehaviour != null)
            SelectBehaviour.Init(this);
    }














    // private void OnEnable()
    // {
    //     InteractBehaviour ib = ComplexView.GetInteractBehaviour();
    //     ib.HoverNeuron.Add(AnimateToHover);
    //     ib.UnhoverNeuron.Add(AnimateToIdle);
    //
    //     ib.BeginDragNeuron.Add(ComplexView.SetInteractableToFalse);
    //     ib.BeginDragNeuron.Add(ComplexView.SetVisibleToFalse);
    //     ib.BeginDragNeuron.Add(CanvasManager.Instance.SkillAnnotation.SetAddressToNull);
    //     // ComplexView.AddressBehaviour.GetAddress().Append(".Skill")
    //     ib.BeginDragNeuron.Add(CanvasManager.Instance.SkillGhost.SetAddressFromIB);
    //     ib.BeginDragNeuron.Add(CanvasManager.Instance.SkillGhost.BeginDrag);
    //     ib.EndDragNeuron.Add(ComplexView.SetInteractableToTrue);
    //     ib.EndDragNeuron.Add(ComplexView.SetVisibleToTrue);
    //     ib.EndDragNeuron.Add(CanvasManager.Instance.SkillGhost.EndDrag);
    //     ib.DragNeuron.Add(CanvasManager.Instance.SkillGhost.Drag);
    //
    //     SetToIdle();
    // }
    //
    // private void OnDisable()
    // {
    //     InteractBehaviour ib = ComplexView.GetInteractBehaviour();
    //
    //     ib.HoverNeuron.Remove(AnimateToHover);
    //     ib.UnhoverNeuron.Remove(AnimateToIdle);
    //
    //     ib.BeginDragNeuron.Remove(ComplexView.SetInteractableToFalse);
    //     ib.BeginDragNeuron.Remove(ComplexView.SetVisibleToFalse);
    //     ib.BeginDragNeuron.Remove(CanvasManager.Instance.SkillAnnotation.SetAddressToNull);
    //     // ComplexView.AddressBehaviour.GetAddress().Append(".Skill")
    //     ib.BeginDragNeuron.Remove(CanvasManager.Instance.SkillGhost.SetAddressFromIB);
    //     ib.BeginDragNeuron.Remove(CanvasManager.Instance.SkillGhost.BeginDrag);
    //     ib.EndDragNeuron.Remove(ComplexView.SetInteractableToTrue);
    //     ib.EndDragNeuron.Remove(ComplexView.SetVisibleToTrue);
    //     ib.EndDragNeuron.Remove(CanvasManager.Instance.SkillGhost.EndDrag);
    //     ib.DragNeuron.Remove(CanvasManager.Instance.SkillGhost.Drag);
    // }













    // [SerializeField] private ItemBehaviour ItemBehaviour;
    // [SerializeField] private InteractBehaviour InteractBehaviour;
    // [SerializeField] private AnimateBehaviour AnimateBehaviour;
    // [SerializeField] private PivotBehaviour PivotBehaviour;
    // [SerializeField] private SelectBehaviour SelectBehaviour;
    //
    // public ItemBehaviour GetItemBehaviour() => ItemBehaviour;
    // public InteractBehaviour GetInteractBehaviour() => InteractBehaviour;
    // public AnimateBehaviour GetAnimateBehaviour() => AnimateBehaviour;
    // public SelectBehaviour GetSelectBehaviour() => SelectBehaviour;
    // public PivotBehaviour GetPivotBehaviour() => PivotBehaviour;
    //
    // public void RefreshPivots()
    //     => AnimateBehaviour.AnimateToIdle();
    //
    // private bool _visible;
    // public bool IsVisible() => _visible;
    // public void SetVisible(bool visible)
    // {
    //     _visible = visible;
    //
    //     // AddressBehaviour.SetVisible(_visible);
    // }
    //
    // public void SetVisibleToTrue(InteractBehaviour ib, PointerEventData eventData)
    //     => SetVisible(true);
    //
    // public void SetVisibleToFalse(InteractBehaviour ib, PointerEventData eventData)
    //     => SetVisible(false);
    //
    // private bool _interactable;
    // public virtual bool IsInteractable() => _interactable;
    // public virtual void SetInteractable(bool interactable)
    // {
    //     _interactable = interactable;
    //
    //     // if (InteractBehaviour != null)
    //     //     InteractBehaviour.SetInteractable(_interactable);
    // }
    //
    // public void SetInteractableToTrue(InteractBehaviour ib, PointerEventData eventData)
    //     => SetInteractable(true);
    //
    // public void SetInteractableToFalse(InteractBehaviour ib, PointerEventData eventData)
    //     => SetInteractable(false);
    //
    // public Neuron<InteractBehaviour, PointerEventData> HoverNeuron = new();
    // public Neuron<InteractBehaviour, PointerEventData> UnhoverNeuron = new();
    // public Neuron<InteractBehaviour, PointerEventData> PointerMoveNeuron = new();
    // public Neuron<InteractBehaviour, PointerEventData> BeginDragNeuron = new();
    // public Neuron<InteractBehaviour, PointerEventData> EndDragNeuron = new();
    // public Neuron<InteractBehaviour, PointerEventData> DragNeuron = new();
    // public Neuron<InteractBehaviour, PointerEventData> LeftClickNeuron = new();
    // public Neuron<InteractBehaviour, PointerEventData> RightClickNeuron = new();
    // public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DropNeuron = new();
}
