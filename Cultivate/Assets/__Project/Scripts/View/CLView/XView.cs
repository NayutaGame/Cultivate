
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class XView : MonoBehaviour
{
    public abstract SimpleView GetView();

    public abstract RectTransform GetViewTransform();
    public abstract void SetViewTransform(RectTransform pivot);

    [SerializeField] private InteractBehaviour InteractBehaviour;
    public InteractBehaviour GetInteractBehaviour() => InteractBehaviour;
    
    private XBehaviour[] _behaviours;
    public XBehaviour[] GetBehaviours() => _behaviours;
    public T GetBehaviour<T>() where T : XBehaviour => _behaviours.FirstObj(b => b is T) as T;
    public ItemBehaviour GetItemBehaviour() => Get<ItemBehaviour>();
    public SelectBehaviour GetSelectBehaviour() => Get<SelectBehaviour>();

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

        _behaviours ??= GetComponents<XBehaviour>();
        _behaviours.Do(b => b.Init(this));

        _sm = InitSM();
    }

    private TableSM _sm;
    private TableSM InitSM()
    {
        // 0 for inactive, 1 for idle
        TableSM sm = new(2);
        sm[-1, 0] = GoToInactive;
        sm[-1, 1] = GoToIdle;
        return sm;
    }

    private void GoToInactive()
    {
        if (InteractBehaviour != null)
            InteractBehaviour.SetInteractable(false);
        GetView().SetVisible(false);
    }

    private void GoToIdle()
    {
        if (InteractBehaviour != null)
            InteractBehaviour.SetInteractable(true);
        GetView().SetVisible(true);
    }

    public void SetIdle(InteractBehaviour ib, PointerEventData d)
        => _sm.SetState(1);

    public void SetInactive(InteractBehaviour ib, PointerEventData d)
        => _sm.SetState(0);

    public void SetVisible(bool value)
    {
        GetView().SetVisible(value);
    }
    
    public abstract Address GetAddress();
    public abstract T Get<T>() where T : class;
    public abstract void SetAddress(Address address);
    public abstract void Refresh();
}
