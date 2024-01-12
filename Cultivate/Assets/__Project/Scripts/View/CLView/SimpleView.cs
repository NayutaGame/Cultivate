
using UnityEngine;

public class SimpleView : MonoBehaviour, CLView
{
    public SimpleView GetSimpleView() => this;

    [SerializeField] private RectTransform RectTransform;
    public RectTransform GetDisplayTransform() => RectTransform;

    [SerializeField] private InteractBehaviour InteractBehaviour;
    public InteractBehaviour GetInteractBehaviour() => InteractBehaviour;

    [SerializeField] private ItemBehaviour ItemBehaviour;
    public ItemBehaviour GetItemBehaviour() => ItemBehaviour;

    [SerializeField] private SelectBehaviour SelectBehaviour;
    public SelectBehaviour GetSelectBehaviour() => SelectBehaviour;

    [SerializeField] private StateBehaviour StateBehaviour;
    public StateBehaviour GetStateBehaviour() => StateBehaviour;

    public void Awake()
    {
        if (InteractBehaviour != null)
            InteractBehaviour.Init(this);

        if (ItemBehaviour != null)
            ItemBehaviour.Init(this);

        if (SelectBehaviour != null)
            SelectBehaviour.Init(this);
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
