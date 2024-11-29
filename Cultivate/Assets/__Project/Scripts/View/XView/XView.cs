
using CLLibrary;
using UnityEngine;

public class XView : MonoBehaviour
{
    private RectTransform _rect;
    public RectTransform GetRect() => _rect;
    
    private XBehaviour[] _behaviours;
    public XBehaviour[] GetBehaviours() => _behaviours;
    public T GetBehaviour<T>() where T : XBehaviour => _behaviours.FirstObj(b => b is T) as T;
    public InteractBehaviour GetInteractBehaviour() => GetBehaviour<InteractBehaviour>();
    public ItemBehaviour GetItemBehaviour() => Get<ItemBehaviour>();
    // public SelectBehaviour GetSelectBehaviour() => Get<SelectBehaviour>();
    
    private bool _hasAwoken;
    private Animator _animator;
    protected Animator GetAnimator() => _animator;

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
        _rect ??= GetComponent<RectTransform>();

        _behaviours ??= GetComponents<XBehaviour>();
        _behaviours.Do(b =>
        {
            b.CheckAwake();
            b.SetView(this);
        });

        _animator = InitAnimator();
        
        InteractBehaviour ib = GetInteractBehaviour();
        if (ib != null)
            InitInteractBehaviour(ib);
    }

    protected virtual Animator InitAnimator()
    {
        return null;
        // 0. inactive
        // 1. hide
        // TableSM sm = new(2);
        // sm[-1, 0] = GoToHide;
        // sm[-1, 1] = GoToIdle;
        // return sm;
    }

    protected virtual void InitInteractBehaviour(InteractBehaviour ib) { }

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
