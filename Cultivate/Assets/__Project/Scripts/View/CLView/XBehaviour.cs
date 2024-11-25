
using UnityEngine;

[RequireComponent(typeof(XView))]
public abstract class XBehaviour : MonoBehaviour
{
    protected XView View;

    public virtual void Init(XView view)
    {
        View = view;
    }

    public SimpleView GetSimpleView() => View.GetView();
    public RectTransform GetDisplayTransform() => View.GetViewTransform();
    public InteractBehaviour GetInteractBehaviour() => View.GetInteractBehaviour();
    public SelectBehaviour GetSelectBehaviour() => View.GetSelectBehaviour();
    public XBehaviour[] GetBehaviours() => View.GetBehaviours();
    public T GetBehaviour<T>() where T : XBehaviour => View.GetBehaviour<T>();
}
