
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class LegacyView : MonoBehaviour
{
    public abstract LegacySimpleView GetView();

    public abstract RectTransform GetViewTransform();
    public abstract void SetViewTransform(RectTransform pivot);

    [SerializeField] private LegacyInteractBehaviour InteractBehaviour;
    public LegacyInteractBehaviour GetInteractBehaviour() => InteractBehaviour;
    
    private LegacyBehaviour[] _behaviours;
    public LegacyBehaviour[] GetBehaviours() => _behaviours;
    public T GetBehaviour<T>() where T : LegacyBehaviour => _behaviours.FirstObj(b => b is T) as T;
    public LegacyItemBehaviour GetItemBehaviour() => Get<LegacyItemBehaviour>();
    public LegacySelectBehaviour GetSelectBehaviour() => Get<LegacySelectBehaviour>();

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
        InteractBehaviour ??= GetComponent<LegacyInteractBehaviour>();
        if (InteractBehaviour != null)
            InteractBehaviour.Init(this);

        _behaviours ??= GetComponents<LegacyBehaviour>();
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

    public void SetIdle(LegacyInteractBehaviour ib, PointerEventData d)
        => _sm.SetState(1);

    public void SetInactive(LegacyInteractBehaviour ib, PointerEventData d)
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
