
using UnityEngine;

[RequireComponent(typeof(LegacyView))]
public abstract class LegacyBehaviour : MonoBehaviour
{
    protected LegacyView View;

    public virtual void Init(LegacyView view)
    {
        View = view;
    }

    public LegacySimpleView GetSimpleView() => View.GetView();
    public RectTransform GetDisplayTransform() => View.GetViewTransform();
    public LegacyInteractBehaviour GetInteractBehaviour() => View.GetInteractBehaviour();
    public LegacySelectBehaviour GetSelectBehaviour() => View.GetSelectBehaviour();
    public LegacyBehaviour[] GetBehaviours() => View.GetBehaviours();
    public T GetBehaviour<T>() where T : LegacyBehaviour => View.GetBehaviour<T>();
}
