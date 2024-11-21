
using System;
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;

public class XView : MonoBehaviour
{
    [NonSerialized] public RectTransform RectTransform;
    
    
    
    
    protected CanvasGroup CanvasGroup;
    public void SetVisible(bool value)
    {
        if (CanvasGroup != null)
            CanvasGroup.alpha = value ? 1 : 0;
    }


    
    
    private InteractBehaviour _interactBehaviour;
    public InteractBehaviour GetInteractBehaviour() => _interactBehaviour;
    public void SetInteractBehaviour(InteractBehaviour ib) => _interactBehaviour = ib;

    private XBehaviour[] _behaviours;
    public XBehaviour[] GetBehaviours() => _behaviours;
    public ItemBehaviour GetItemBehaviour() => GetBehaviour<ItemBehaviour>();
    public SelectBehaviour GetSelectBehaviour() => GetBehaviour<SelectBehaviour>();
    public T GetBehaviour<T>() where T : XBehaviour => _behaviours.FirstObj(b => b is T) as T;

    [NonSerialized] public bool _hasAwoken;
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
        RectTransform = GetComponent<RectTransform>();
        
        _behaviours ??= GetComponents<XBehaviour>();
        _behaviours.Do(b => b.AwakeFunction(this));

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
        InteractBehaviour ib = GetInteractBehaviour();
        if (ib != null)
            ib.SetInteractable(true);
        SetVisible(true);
    }

    public void Hide()
    {
        InteractBehaviour ib = GetInteractBehaviour();
        if (ib != null)
            ib.SetInteractable(false);
        SetVisible(false);
    }

    public void SetShow(InteractBehaviour ib, PointerEventData d)
        => _sm.SetState(1);

    public void SetShow()
        => _sm.SetState(1);

    public void SetHide(InteractBehaviour ib, PointerEventData d)
        => _sm.SetState(0);
    
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
