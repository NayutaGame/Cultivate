
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CLView : MonoBehaviour
{
    public abstract SimpleView GetSimpleView();

    public abstract RectTransform GetDisplayTransform();
    public abstract void SetDisplayTransform(RectTransform pivot);

    [SerializeField] private InteractBehaviour InteractBehaviour;
    public InteractBehaviour GetInteractBehaviour() => InteractBehaviour;

    private ItemBehaviour ItemBehaviour;
    public ItemBehaviour GetItemBehaviour() => ItemBehaviour;

    private SelectBehaviour SelectBehaviour;
    public SelectBehaviour GetSelectBehaviour() => SelectBehaviour;

    private ExtraBehaviour[] ExtraBehaviours;
    public ExtraBehaviour[] GetExtraBehaviours() => ExtraBehaviours;
    public T GetExtraBehaviour<T>() where T : ExtraBehaviour => ExtraBehaviours.FirstObj(b => b is T) as T;

    public virtual void Awake()
    {
        InteractBehaviour ??= GetComponent<InteractBehaviour>();
        if (InteractBehaviour != null)
            InteractBehaviour.Init(this);

        ItemBehaviour ??= GetComponent<ItemBehaviour>();
        if (ItemBehaviour != null)
            ItemBehaviour.Init(this);

        SelectBehaviour ??= GetComponent<SelectBehaviour>();
        if (SelectBehaviour != null)
            SelectBehaviour.Init(this);

        ExtraBehaviours ??= GetComponents<ExtraBehaviour>();
        ExtraBehaviours.Do(b => b.Init(this));
    }

    private void SetInteractable(bool value)
    {
        if (InteractBehaviour != null)
            InteractBehaviour.SetInteractable(value);
    }

    private void SetVisible(bool value)
        => GetSimpleView().SetVisible(value);

    public void SetInteractableToTrue(InteractBehaviour ib, PointerEventData d)
        => SetInteractable(true);

    public void SetInteractableToTrue()
        => SetInteractable(true);

    public void SetInteractableToFalse(InteractBehaviour ib, PointerEventData d)
        => SetInteractable(false);

    public void SetInteractableToFalse()
        => SetInteractable(false);

    public void SetVisibleToTrue(InteractBehaviour ib, PointerEventData d)
        => SetVisible(true);

    public void SetVisibleToTrue()
        => SetVisible(true);

    public void SetVisibleToFalse(InteractBehaviour ib, PointerEventData d)
        => SetVisible(false);

    public void SetVisibleToFalse()
        => SetVisible(false);
}
