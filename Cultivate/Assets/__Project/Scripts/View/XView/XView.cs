
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class XView : MonoBehaviour
{
    private RectTransform _rect;
    public RectTransform GetRect() => _rect;

    [SerializeField] protected InteractBehaviour _interactBehaviour;
    public InteractBehaviour GetInteractBehaviour() => _interactBehaviour;
    
    private List<XBehaviour> _behaviours;
    public List<XBehaviour> GetBehaviours() => _behaviours;
    public T GetBehaviour<T>() where T : XBehaviour => _behaviours.FirstObj(b => b is T) as T;

    public void JoinBehaviour(XBehaviour behaviour)
    {
        if (_behaviours.Contains(behaviour))
            return;
        
        _behaviours.Add(behaviour);
        behaviour.SetView(this);
        behaviour.CheckAwake();
    }
    
    private bool _hasAwoken;
    private Animator _animator;
    public Animator GetAnimator() => _animator;
    public void SetAnimator(Animator animator) => _animator = animator;

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

    protected virtual void AwakeFunction()
    {
        _rect ??= GetComponent<RectTransform>();

        _behaviours ??= GetComponents<XBehaviour>().ToList();
        _behaviours.Do(b =>
        {
            b.SetView(this);
            b.CheckAwake();
        });

        _animator = InitAnimator();
        
        if (_interactBehaviour != null)
        {
            _interactBehaviour.SetView(this);
            _interactBehaviour.CheckAwake();
            SetInteractBehaviour(_interactBehaviour);
        }
    }

    protected virtual Animator InitAnimator()
    {
        return null;
    }

    public virtual void SetInteractBehaviour(InteractBehaviour ib)
    {
        _interactBehaviour = ib;
        _behaviours.Do(b => b.SetInteractBehaviour(ib));
    }

    private Address _address;
    public virtual Address GetAddress() => _address;
    public virtual T Get<T>() where T : class => _address?.Get<T>();
    public virtual void SetAddress(Address address)
    {
        _address = address;
        CheckAwake();
    }

    public virtual void Refresh()
    {
    }
}
