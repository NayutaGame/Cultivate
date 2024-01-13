
using System;
using UnityEngine;

[RequireComponent(typeof(CLView))]
public class ItemBehaviour : MonoBehaviour
{
    [NonSerialized] public int PrefabIndex;

    private CLView CLView;
    public SimpleView GetSimpleView() => CLView.GetSimpleView();
    public RectTransform GetDisplayTransform() => CLView.GetDisplayTransform();
    public InteractBehaviour GetInteractBehaviour() => CLView.GetInteractBehaviour();
    public SelectBehaviour GetSelectBehaviour() => CLView.GetSelectBehaviour();
    public ExtraBehaviour[] GetExtraBehaviours() => CLView.GetExtraBehaviours();
    public T GetExtraBehaviour<T>() where T : ExtraBehaviour => CLView.GetExtraBehaviour<T>();

    public void Init(CLView clView)
    {
        CLView = clView;
    }
}
