
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
        AwakeFunction();
    }

    public virtual void AwakeFunction()
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

        _sm = InitSM();
    }

    private TableSM _sm;

    private TableSM InitSM()
    {
        // 0 for hide, 1 for show
        TableSM sm = new(2);
        sm[-1, 1] = Show;
        sm[-1, 0] = Hide;
        return sm;
    }

    private void Show()
    {
        if (InteractBehaviour != null)
            InteractBehaviour.SetInteractable(true);
        GetSimpleView().SetVisible(true);
    }

    private void Hide()
    {
        if (InteractBehaviour != null)
            InteractBehaviour.SetInteractable(false);
        GetSimpleView().SetVisible(false);
    }

    public void SetShow(InteractBehaviour ib, PointerEventData d)
        => _sm.SetState(1);

    public void SetShow()
        => _sm.SetState(1);

    public void SetHide(InteractBehaviour ib, PointerEventData d)
        => _sm.SetState(0);
    
    public abstract Address GetAddress();
    public abstract T Get<T>() where T : class;
    public abstract void SetAddress(Address address);
    public abstract void Refresh();
}
