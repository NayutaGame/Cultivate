
using CLLibrary;
using UnityEngine;

public class XView : MonoBehaviour
{
    private RectTransform _rect;

    // [SerializeField] private InteractBehaviour _interactBehaviour;
    // public InteractBehaviour GetInteractBehaviour() => _interactBehaviour;
    
    private XBehaviour[] _behaviours;
    public XBehaviour[] GetBehaviours() => _behaviours;
    public T GetBehaviour<T>() where T : XBehaviour => _behaviours.FirstObj(b => b is T) as T;
    // public ItemBehaviour GetItemBehaviour() => Get<ItemBehaviour>();
    // public SelectBehaviour GetSelectBehaviour() => Get<SelectBehaviour>();
    
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
        // _interactBehaviour ??= GetComponent<InteractBehaviour>();
        // if (InteractBehaviour != null)
        //     InteractBehaviour.Init(this);
        
        _rect ??= GetComponent<RectTransform>();

        _behaviours ??= GetComponents<XBehaviour>();
        _behaviours.Do(b =>
        {
            b.CheckAwake();
            b.SetView(this);
        });

        // _sm = InitSM();
    }

    // private TableSM _sm;
    // private TableSM InitSM()
    // {
    //     // 0 for inactive, 1 for idle
    //     TableSM sm = new(2);
    //     sm[-1, 0] = GoToInactive;
    //     sm[-1, 1] = GoToIdle;
    //     return sm;
    // }
    //
    // private void GoToInactive()
    // {
    //     if (InteractBehaviour != null)
    //         InteractBehaviour.SetInteractable(false);
    //     GetView().SetVisible(false);
    // }
    //
    // private void GoToIdle()
    // {
    //     if (InteractBehaviour != null)
    //         InteractBehaviour.SetInteractable(true);
    //     GetView().SetVisible(true);
    // }
    //
    // public void SetIdle(LegacyInteractBehaviour ib, PointerEventData d)
    //     => _sm.SetState(1);
    //
    // public void SetInactive(LegacyInteractBehaviour ib, PointerEventData d)
    //     => _sm.SetState(0);
    //
    // public void SetVisible(bool value)
    // {
    //     GetView().SetVisible(value);
    // }

    private Address _address;
    public virtual Address GetAddress() => _address;
    public virtual T Get<T>() where T : class => _address?.Get<T>();
    public virtual void SetAddress(Address address)
    {
        _address = address;
    }

    public virtual void Refresh()
    {
    }
}
