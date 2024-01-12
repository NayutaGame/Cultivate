
using System;
using UnityEngine;

[RequireComponent(typeof(CLView))]
public class ItemBehaviour : MonoBehaviour
{
    [NonSerialized] public int PrefabIndex;

    private CLView CLView;
    public SimpleView GetSimpleView() => CLView.GetSimpleView();
    public InteractBehaviour GetInteractBehaviour() => CLView.GetInteractBehaviour();
    public SelectBehaviour GetSelectBehaviour() => CLView.GetSelectBehaviour();
    public RectTransform GetDisplayTransform() => CLView.GetDisplayTransform();

    public void Init(CLView clView)
    {
        CLView = clView;
    }
}
