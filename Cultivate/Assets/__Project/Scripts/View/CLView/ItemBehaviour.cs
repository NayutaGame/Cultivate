
using System;
using UnityEngine;

public class ItemBehaviour : XBehaviour
{
    [NonSerialized] public int PrefabIndex;

    public SimpleView GetSimpleView() => View.GetSimpleView();
    public RectTransform GetDisplayTransform() => View.GetDisplayTransform();
    public InteractBehaviour GetInteractBehaviour() => View.GetInteractBehaviour();
    public SelectBehaviour GetSelectBehaviour() => View.GetSelectBehaviour();
    public XBehaviour[] GetExtraBehaviours() => View.GetBehaviours();
    public T GetExtraBehaviour<T>() where T : XBehaviour => View.GetBehaviour<T>();
}
