
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class XView : MonoBehaviour
{
    public abstract SimpleView GetSimpleView();

    public abstract RectTransform GetDisplayTransform();
    public abstract void SetDisplayTransform(RectTransform pivot);

    [SerializeField] private InteractBehaviour InteractBehaviour;
    public InteractBehaviour GetInteractBehaviour() => InteractBehaviour;

    public ItemBehaviour GetItemBehaviour() => Get<ItemBehaviour>();

    private SelectBehaviour SelectBehaviour;
    public SelectBehaviour GetSelectBehaviour() => SelectBehaviour;

    private XBehaviour[] _behaviours;
    public XBehaviour[] GetBehaviours() => _behaviours;
    public T GetBehaviour<T>() where T : XBehaviour => _behaviours.FirstObj(b => b is T) as T;

    private bool _hasAwoken;

    public virtual void Awake()
    {
        CheckAwake();
    }

    public void CheckAwake()
    {
        if (_hasAwoken)
            return;
        _hasAwoken = true;
        AwakeFunction();
    }

    public virtual void AwakeFunction()
    {
        InteractBehaviour ??= GetComponent<InteractBehaviour>();
        if (InteractBehaviour != null)
            InteractBehaviour.Init(this);

        SelectBehaviour ??= GetComponent<SelectBehaviour>();
        if (SelectBehaviour != null)
            SelectBehaviour.Init(this);

        _behaviours ??= GetComponents<XBehaviour>();
        _behaviours.Do(b => b.Init(this));

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

    public void Hide()
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

    public void SetVisible(bool value)
    {
        GetSimpleView().SetVisible(value);
    }
    
    public abstract Address GetAddress();
    public abstract T Get<T>() where T : class;
    public abstract void SetAddress(Address address);
    public abstract void Refresh();
}
