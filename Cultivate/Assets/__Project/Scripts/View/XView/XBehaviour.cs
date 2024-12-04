
using UnityEngine;

[RequireComponent(typeof(XView))]
public class XBehaviour : MonoBehaviour
{
    private XView _view;
    
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
    }

    public void SetView(XView view) => _view = view;

    public XView GetView() => _view;
    // public LegacyInteractBehaviour GetInteractBehaviour() => View.GetInteractBehaviour();
    // public LegacySelectBehaviour GetSelectBehaviour() => View.GetSelectBehaviour();
    public XBehaviour[] GetBehaviours() => _view.GetBehaviours();
    public T GetBehaviour<T>() where T : XBehaviour => _view.GetBehaviour<T>();

    public T Get<T>() where T : class => _view.Get<T>();
    public virtual Address GetAddress() => _view.GetAddress();
}
