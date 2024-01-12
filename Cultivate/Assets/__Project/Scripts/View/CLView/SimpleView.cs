
using UnityEngine;

public class SimpleView : MonoBehaviour, CLView
{
    public SimpleView GetSimpleView() => this;

    private RectTransform RectTransform;
    public RectTransform GetDisplayTransform() => RectTransform;

    [SerializeField] private InteractBehaviour InteractBehaviour;
    public InteractBehaviour GetInteractBehaviour() => InteractBehaviour;

    private ItemBehaviour ItemBehaviour;
    public ItemBehaviour GetItemBehaviour() => ItemBehaviour;

    private SelectBehaviour SelectBehaviour;
    public SelectBehaviour GetSelectBehaviour() => SelectBehaviour;

    private StateBehaviour StateBehaviour;
    public StateBehaviour GetStateBehaviour() => StateBehaviour;

    public void Awake()
    {
        RectTransform ??= GetComponent<RectTransform>();

        InteractBehaviour ??= GetComponent<InteractBehaviour>();
        if (InteractBehaviour != null)
            InteractBehaviour.Init(this);

        ItemBehaviour ??= GetComponent<ItemBehaviour>();
        if (ItemBehaviour != null)
            ItemBehaviour.Init(this);

        SelectBehaviour ??= GetComponent<SelectBehaviour>();
        if (SelectBehaviour != null)
            SelectBehaviour.Init(this);

        StateBehaviour ??= GetComponent<StateBehaviour>();
        if (StateBehaviour != null)
            StateBehaviour.Init(this);
    }

    public void SetDisplayTransform(RectTransform pivot)
    {
        RectTransform.position = pivot.position;
        RectTransform.localScale = pivot.localScale;
    }

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    public virtual void SetAddress(Address address)
    {
        _address = address;
    }

    public virtual void Refresh()
    {
    }


    // private bool _visible;
    // public bool IsVisible() => _visible;
    // public void SetVisible(bool visible)
    // {
    //     _visible = visible;
    //
    //     // AddressBehaviour.SetVisible(_visible);
    // }
    //
    // public void SetVisibleToTrue(MonoBehaviour behaviour, PointerEventData eventData)
    //     => SetVisible(true);
    //
    // public void SetVisibleToFalse(MonoBehaviour behaviour, PointerEventData eventData)
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
    // public void SetInteractableToTrue(MonoBehaviour behaviour, PointerEventData eventData)
    //     => SetInteractable(true);
    //
    // public void SetInteractableToFalse(MonoBehaviour behaviour, PointerEventData eventData)
    //     => SetInteractable(false);
    //
    // // [SerializeField] private CanvasGroup CanvasGroup;
    // //
    // // public void SetVisible(bool visible)
    // // {
    // //     CanvasGroup.alpha = visible ? 1 : 0;
    // // }
}
