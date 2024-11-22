
using System;
using UnityEngine;

[RequireComponent(typeof(XView))]
public class ItemBehaviour : MonoBehaviour
{
    [NonSerialized] public int PrefabIndex;

    private XView _xView;
    public SimpleView GetSimpleView() => _xView.GetSimpleView();
    public RectTransform GetDisplayTransform() => _xView.GetDisplayTransform();
    public InteractBehaviour GetInteractBehaviour() => _xView.GetInteractBehaviour();
    public SelectBehaviour GetSelectBehaviour() => _xView.GetSelectBehaviour();
    public XBehaviour[] GetExtraBehaviours() => _xView.GetBehaviours();
    public T GetExtraBehaviour<T>() where T : XBehaviour => _xView.GetBehaviour<T>();

    public void Init(XView xView)
    {
        _xView = xView;
    }
}
