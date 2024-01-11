
using System;
using UnityEngine;

[RequireComponent(typeof(CLView))]
public class ItemBehaviour : MonoBehaviour
{
    [NonSerialized] public int PrefabIndex;

    [NonSerialized] public CLView CLView;
    public SimpleView GetSimpleView() => CLView.GetSimpleView();










    // public SelectBehaviour GetSelectBehaviour();
    // public InteractBehaviour GetInteractBehaviour();
    // public void RefreshPivots();
}
